using System.Collections.Generic;

namespace submission_api
{
    public class CqrsEntity<TEntity> where TEntity : new()
    {
        public int Version {get; set;}
        public TEntity Entity {get;set;}

        public List<CqrsCommand> Commands {get;set;} = new List<CqrsCommand>();
    }

    public class CqrsCommand
    {
        public string CommandId {get;set;}
        public object Command {get;set;}
    }
}