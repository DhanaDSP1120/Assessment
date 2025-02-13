using Quartz.Impl;
using Quartz;
using Serilog;

namespace Assessment.Operations
{
    public class Schedule
    {
        public static async Task Start()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            var job = JobBuilder.Create<CsvImportJob>().WithIdentity("CsvImportJob", "test").Build();
            var trigger = TriggerBuilder.Create().WithIdentity("CsvImportTrigger", "group1").StartNow().WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever()).Build();
            await scheduler.ScheduleJob(job, trigger);
            Log.Information("Daily Refersh Schedule Created");
            await scheduler.Start();
        }
    }
}
