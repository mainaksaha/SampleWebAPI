using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;

namespace SampleWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
            aiOptions.EnableAdaptiveSampling = true;
            services.AddApplicationInsightsTelemetry(aiOptions);
            //services.AddApplicationInsightsTelemetryProcessor<AdaptiveTelemetryProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();

            SamplingPercentageEstimatorSettings samplingPercentageEstimatorSettings = new SamplingPercentageEstimatorSettings();
            //samplingPercentageEstimatorSettings.EvaluationInterval = new TimeSpan(0, 0, 15);
            samplingPercentageEstimatorSettings.InitialSamplingPercentage = 100;
            samplingPercentageEstimatorSettings.MovingAverageRatio = 0.90; //Highly reactive to sudden changes
            //samplingPercentageEstimatorSettings.SamplingPercentageIncreaseTimeout = new TimeSpan(0,0,15);
            samplingPercentageEstimatorSettings.EvaluationInterval = new TimeSpan(0, 0, 10); //shorten the interval to catch the certain burst
            samplingPercentageEstimatorSettings.MaxTelemetryItemsPerSecond = 2;
            AdaptiveSamplingPercentageEvaluatedCallback adaptiveSamplingPercentageEvaluatedCallback = AdaptiveTelCallback;

            //    var tpBuilder = TelemetryConfiguration.Active.DefaultTelemetrySink.TelemetryProcessorChainBuilder;
            //    //tpBuilder.UseAdaptiveSampling(maxTelemetryItemsPerSecond: 1, excludedTypes: "Exception");
            //    tpBuilder.UseAdaptiveSampling(samplingPercentageEstimatorSettings, adaptiveSamplingPercentageEvaluatedCallback, excludedTypes: "Exception", includedTypes: "Request;Trace");
            //    tpBuilder.Build();
            var builder = configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder;
            builder.UseAdaptiveSampling(samplingPercentageEstimatorSettings, adaptiveSamplingPercentageEvaluatedCallback, excludedTypes: "Exception");
            //builder.Use((next) => new AdaptiveTelemetryProcessor(next));
            //builder.UseSampling(30.0, excludedTypes:"Request;Exception");
            
            builder.Build();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AdaptiveTelCallback(double afterSamplingTelemetryItemRatePerSecond, double currentSamplingPercentage, double newSamplingPercentage, bool isSamplingPercentageChanged, SamplingPercentageEstimatorSettings settings)
        {
            //throw new NotImplementedException();
        }

        //public void Configure(IApplicationBuilder app, IHostEnvironment env, TelemetryConfiguration configuration)
        //{

        //}
    }
}
