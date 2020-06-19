using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzWithCore.Tasks
{
    public class CheckAvailableTask : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var dataMap = context.JobDetail.JobDataMap;
                var timeRequested = dataMap.GetDateTime("Current Date Time");
                var runTime = dataMap.GetDateTime("Will run Time");
                var ticketsNeeded = dataMap.GetInt("Tickets needed");
                var concertName = dataMap.GetString("Concert Name");


                Debug.WriteLine($"{ticketsNeeded} Tickets to the {concertName} concert on {timeRequested.ToString("MM-dd-yyy HH:mm:ss")}" +
                    $" are availables and must run at: {runTime.ToString("MM-dd-yyy HH:mm:ss")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Task.FromResult(0);
        }
    }
}
