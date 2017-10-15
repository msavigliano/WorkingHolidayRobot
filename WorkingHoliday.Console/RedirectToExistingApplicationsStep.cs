namespace WorkingHoliday.Console
{
    partial class Program
    {
        public class RedirectToExistingApplicationsStep : RedirectionStep
        {
            public override string Path => "https://onlineservices.immigration.govt.nz/WorkingHoliday/default.aspx";

            public override IStep NextStep => new ExistingApplicationsStep();
            public override IStep OnErrorStep => new RedirectToLoginStep();
        }
    }
}
