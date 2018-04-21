using System;

namespace submission_api
{
    public class CreateSubmissionCommandHandler : ICommandHandler<Submission, CreateSubmissionCommand, CreatedSubmissionEvent>
    {
        public CommandResult<Submission, CreatedSubmissionEvent> HandleCommand(Submission submission, CreateSubmissionCommand command)
        {
            if(submission.State != SubmissionState.Initialized)
            {
                throw new InvalidOperationException("Submission already went through a created state");
            }
            submission.State = SubmissionState.Pending;
            submission.Number = command.Number;
            submission.SubmitterName = command.SubmitterName;
            return CommandResult<Submission, CreatedSubmissionEvent>.Create(submission, new CreatedSubmissionEvent{
                Number = command.Number,
                SubmitterName = command.SubmitterName
            });
        }
    }
}