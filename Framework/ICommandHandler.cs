namespace submission_api
{
    public interface ICommandHandler<TEntity, TCommand, TEvent>
    {
        CommandResult<TEntity, TEvent> HandleCommand(TEntity entity, TCommand command);
    }
}