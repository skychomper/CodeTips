using System.Diagnostics;
using Quartz;

namespace QuartzDemo {
  public class Program {
    public static void Main(string[] args) {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddRazorPages();

      builder.Services.AddQuartz(q => {
        var jobKey = new JobKey("HelloJob");

        q.AddJob<HelloJob>(opts => opts.WithIdentity(jobKey));

        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("HelloJob-trigger")
            .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds
      });

      builder.Services.AddQuartzHostedService(opt => {
        opt.WaitForJobsToComplete = true;
      });

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (!app.Environment.IsDevelopment()) {
        app.UseExceptionHandler("/Error");
      }
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthorization();

      app.MapRazorPages();

      app.Run();
    }

    public class HelloJob : IJob {
      public async Task Execute(IJobExecutionContext context) {
        Debug.WriteLine("Greetings from HelloJob!");
      }
    }
  }
}
