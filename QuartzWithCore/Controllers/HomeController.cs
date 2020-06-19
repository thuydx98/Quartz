using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using QuartzWithCore.Models;
using QuartzWithCore.Tasks;

namespace QuartzWithCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScheduler _scheduler;
        private const string GROUP_JOBS = "MARKETING_EMAIL";
        DateTime time = DateTime.Parse("2020/06/04 11:00:00");
        DateTime newTime = DateTime.Parse("2020/06/04 10:41:00");

        public HomeController(IScheduler factory)
        {
            _scheduler = factory;
        }

        public async Task<IActionResult> Index()
        {
            IDictionary<string, object> map = new Dictionary<string, object>()
            {
                {"Current Date Time", $"{DateTime.Now}" },
                {"Will run Time", $"{time}" },
                {"Tickets needed", $"5" },
                {"Concert Name", $"Rock" }
            };

            IJobDetail job = JobBuilder.Create<CheckAvailableTask>()
                .WithIdentity($"Check Availablity-{time.ToString()}", GROUP_JOBS)
                .SetJobData(new JobDataMap(map))
                .RequestRecovery()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"Check Availablity-{time.ToString()}", GROUP_JOBS)
                .StartAt(new DateTimeOffset(time))
                //.WithSimpleSchedule(x => x.WithRepeatCount(20).WithInterval(TimeSpan.FromSeconds(5)))
                .WithPriority(1)
                .Build();

            Debug.WriteLine($"{job.Key} will run at: {trigger.GetNextFireTimeUtc()}");
            await _scheduler.ScheduleJob(job, trigger);

            //await UpdateTriggerAsync();
            //DeleteSchedule();

            return View();
        }

        private async Task UpdateTriggerAsync()
        {
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"Check Availablity-{time.ToString()}", GROUP_JOBS)
                .StartAt(new DateTimeOffset(newTime))
                .WithPriority(1)
                .Build();

            await _scheduler.RescheduleJob(new TriggerKey($"Check Availablity-{time.ToString()}", GROUP_JOBS), trigger);
        }

        /// <summary>
        /// Done
        /// </summary>
        private void DeleteSchedule()
        {
            _scheduler.UnscheduleJob(new TriggerKey($"Check Availablity-{time.ToString()}", GROUP_JOBS));
        }
    }
}
