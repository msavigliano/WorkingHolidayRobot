namespace WorkingHoliday.Console
{
    partial class Program
    {
        public abstract class RedirectionStep : Step
        {
            public abstract string Path { get; }

            public override void InnerExecute()
            {
                DriverInstance.Driver.Navigate().GoToUrl(Path);
            }
        }
    }
}
