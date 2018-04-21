namespace submission_api
{
    public class DecideSubmissionCommandHandler : ICommandHandler<Submission, DecideSubmissionCommand, DecidedSubmissionEvent>
    {
        public CommandResult<Submission, DecidedSubmissionEvent> HandleCommand(Submission submission, DecideSubmissionCommand command)
        {
            submission.IsValid = command.IsValid;
            submission.PayoutAmount = command.PayoutAmount;
            return CommandResult<Submission, DecidedSubmissionEvent>.Create(submission, new DecidedSubmissionEvent{
                IsValid = command.IsValid,
                PayoutAmount = command.PayoutAmount
            });
        }
    }
}