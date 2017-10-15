namespace WorkingHoliday.Console
{
    partial class Program
    {
        public abstract class Step : IStep
        {            
            public abstract IStep NextStep { get; }
            public abstract IStep OnErrorStep { get; }

            public void Execute()
            {
                try
                {
                    InnerExecute();
                    NextStep.Execute();
                }
                catch
                {
                    OnErrorStep.Execute();
                }
            }

            public abstract void InnerExecute();
        }
    }
}
