using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace submission_api
{
    [Route("api/[controller]")]
    public class SubmissionController : Controller
    {
        [HttpGet("{id}")]
        public async Task<Submission> Get(string id)
        {
            var commandBroker = new CommandBroker<Submission>();
            var entity = await commandBroker.GetEntity(id);
            return entity.Entity;
        }

        [HttpPost]
        public Task Post([FromBody]CreateSubmissionRequest request)
        {
            var commandBroker = new CommandBroker<Submission>();
            return commandBroker.IssueCommand(
                request.SubmissionId,
                request.CommandId,
                new CreateSubmissionCommand
                {
                    SubmitterName = request.SubmitterName,
                    Number = "1234-5678"
                });
        }

        [HttpPost("{id}/release")]
        public Task Release(string id, [FromBody]ReleaseSubmissionRequest request)
        {
            var commandBroker = new CommandBroker<Submission>();
            return commandBroker.IssueCommand(
                id,
                request.CommandId,
                new ReleaseSubmissionCommand());
        }
    }

    public class CreateSubmissionRequest
    {
        public string SubmitterName {get;set;}
        public string CommandId {get;set;}
        public string SubmissionId {get;set;}
    }

    public class ReleaseSubmissionRequest
    {
        public string CommandId {get;set;}
    }
}
