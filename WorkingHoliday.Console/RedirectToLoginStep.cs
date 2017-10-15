namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class RedirectToLoginStep : RedirectionStep
        {
            public override string Path => "https://onlineservices.immigration.govt.nz/?STATUS";

            public override IStep NextStep => new DoLoginStep();
            public override IStep OnErrorStep => this;
        }
    }
}
