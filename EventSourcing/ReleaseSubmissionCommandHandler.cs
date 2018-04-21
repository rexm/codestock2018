namespace submission_api
{
    public class ReleaseSubmissionCommandHandler : ICommandHandler<Submission, ReleaseSubmissionCommand, ReleasedSubmissionEvent>
    {
        public CommandResult<Submission, ReleasedSubmissionEvent> HandleCommand(Submission submission, ReleaseSubmissionCommand command)
        {
            submission.State = SubmissionState.Released;
            return CommandResult<Submission, ReleasedSubmissionEvent>.Create(submission, new ReleasedSubmissionEvent());
        }
    }
}