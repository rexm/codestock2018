using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Newtonsoft.Json;

namespace submission_api
{
    public class CommandBroker<TEntity> where TEntity : new()
    {
        public async Task IssueCommand(string entityId, string commandId, object command)
        {
            var existingEntity = await GetEntity(entityId);
            var existingCommand = existingEntity.Commands.FirstOrDefault(c => c.CommandId == commandId);
            if(existingCommand != null)
            {
                //command ID already issued
                if(existingCommand.Command.GetType() != command.GetType())
                {
                    //conflicting type
                    throw new InvalidOperationException("Duplicate command ID issued for different command");
                }
                return;
            }
            var commandHandlerType = Type.GetType(command.GetType().FullName.Replace("Command", "CommandHandler"));
            var commandHandler = Activator.CreateInstance(commandHandlerType);
            var methodInfo = commandHandlerType.GetMethod("HandleCommand");
            var commandResult = methodInfo.Invoke(commandHandler, new object[]{ existingEntity.Entity, command });

            Table commandTable = Table.LoadTable(GetDynamoClient(), "sandbox_codestock_commandstore");
            var commandDocument = new Document();
            commandDocument["entity_id"] = entityId;
            commandDocument["command_number"] = existingEntity.Version + 1;
            commandDocument["command_id"] = commandId;
            commandDocument["command"] = JsonConvert.SerializeObject(command);
            commandDocument["command_type"] = command.GetType().FullName;

            await commandTable.PutItemAsync(commandDocument);
        }

        public async Task<CqrsEntity<TEntity>> GetEntity(string entityId)
        {
            Table commandTable = Table.LoadTable(GetDynamoClient(), "sandbox_codestock_commandstore");
            QueryOperationConfig config = new QueryOperationConfig()
            {
                KeyExpression = new Expression{
                    ExpressionStatement = "#entity_id = :entity_id",
                    ExpressionAttributeNames = new Dictionary<string, string>
                    {
                        { "#entity_id", "entity_id" }
                    },
                    ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                    {
                        { ":entity_id", entityId }
                    }
                },
                ConsistentRead = true,
                Select = SelectValues.AllAttributes,
                BackwardSearch = false
            };
            Search search = commandTable.Query(config);
            var results = await search.GetNextSetAsync();
            var cqrsEntity = new CqrsEntity<TEntity> { Entity = new TEntity(), Version = -1 };
            foreach(var resultDocument in results)
            {
                var commandType = Type.GetType(resultDocument["command_type"]);
                var command = JsonConvert.DeserializeObject(resultDocument["command"], commandType);
                var commandHandlerType = Type.GetType(resultDocument["command_type"].ToString().Replace("Command", "CommandHandler"));
                var commandHandler = Activator.CreateInstance(commandHandlerType);
                var methodInfo = commandHandlerType.GetMethod("HandleCommand");
                var commandResult = methodInfo.Invoke(commandHandler, new object[]{ cqrsEntity.Entity, command });
                var resultingEntity = commandResult.GetType().GetProperty("ResultingEntity").GetValue(commandResult);
                cqrsEntity.Commands.Add(new CqrsCommand {
                    CommandId = resultDocument["command_id"],
                    Command = command
                });
                cqrsEntity.Entity = (TEntity)resultingEntity;
                cqrsEntity.Version = cqrsEntity.Version + 1;
            }
            return cqrsEntity;
        }

        private static IAmazonDynamoDB GetDynamoClient()
        {
            var awsKey = "Your key here";
            var awsSecret = "Your secret key here";

            return new AmazonDynamoDBClient(
                new BasicAWSCredentials(awsKey, awsSecret),
                RegionEndpoint.USEast1
            );
        }
    }
}