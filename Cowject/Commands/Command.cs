namespace Cowject.Commands
{
    public abstract class Command
    {
        [Inject]
        internal Sequencer Sequencer { get; set; }
        
        public bool retain;
        
        public abstract void Execute();
        
        protected void Retain()
        {
            retain = true;
        }
        
        protected void Release()
        {
            retain = false;
            Sequencer?.ReleaseCommand(this);
        }
    }
}