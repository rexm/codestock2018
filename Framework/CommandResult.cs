namespace submission_api
{
    public class CommandResult<TEntity, TEvent>
    {
        private CommandResult()
        {
        }
        public TEntity ResultingEntity {get;set;}
        public TEvent Event {get;set;}

        public static CommandResult<TEntity, TEvent> Create(TEntity entity, TEvent theEvent)
        {
            return new CommandResult<TEntity, TEvent>
            {
                ResultingEntity = entity, 
                Event = theEvent
            };
        }
    }
}