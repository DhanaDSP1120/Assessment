using Quartz;
using Serilog;

namespace Assessment.Operations
{
    public class CsvImportJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                Log.Information("Daily Data Refersh Triggered");
                CsvImport.ImportCSV("config/assessment.csv");
                Log.Information("Daily Data Refersh Completed");
            }
            catch (Exception ex)
            {
                Log.Error("Error in Daily Data Refersh - "+ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
