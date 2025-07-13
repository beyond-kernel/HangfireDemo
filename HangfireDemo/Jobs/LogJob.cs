namespace HangfireDemo.Jobs
{
    public class LogJob
    {
        private readonly ILogger<LogJob> _logger;

        public LogJob(ILogger<LogJob> logger)
        {
            _logger = logger;
        }

        public void LogRequest(string endpoint, DateTime requestTime)
        {
            _logger.LogInformation(
                $"📌 Requisição em {endpoint} às {requestTime:HH:mm:ss}");
        }
    }
}
