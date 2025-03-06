using CaptainWatch.Scheduler.Services.Movies;
using Hangfire;

namespace CaptainWatch.Scheduler
{
	public static class ConfigRecurringJobs
	{
		public static void ConfigureJobs()
		{
			//In this method we can configure all the recurring jobs that we need

			RecurringJob.AddOrUpdate<MovieWriteService>(
				"movies-update-wish-count",
				service => service.UpdateWishCount(),
				"0 1 * * *", //Every day at 1h00 UTC
				new RecurringJobOptions
				{
					TimeZone = TimeZoneInfo.Utc
				}
			);
		}
	}
}
