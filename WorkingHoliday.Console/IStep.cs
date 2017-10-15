namespace WorkingHoliday.Console
{
    partial class Program
    {
        public interface IStep
        {
            IStep NextStep { get; }
            IStep OnErrorStep { get; }

            void Execute();
        }
    }
}
