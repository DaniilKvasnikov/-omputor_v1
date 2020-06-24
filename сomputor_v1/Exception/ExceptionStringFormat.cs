namespace сomputor_v1.Exception
{
    public class ExceptionStringFormat: System.Exception
    {
        public ExceptionStringFormat(string message) : base(string.Format($"Error format string: {message}\n"))
        {
        }
    }
}