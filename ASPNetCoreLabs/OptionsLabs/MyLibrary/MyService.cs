﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MyLibrary
{
    public interface IMyService
    {
        void DoWork();
    }

    public class MyService : IMyService
    {
        private readonly ILogger<MyService> _logger;
        private readonly string _option1;
        private readonly bool _option2;

        public MyService(ILogger<MyService> logger, IOptions<MyServiceOptions> options)
        {
            _logger = logger;
            _option1 = options.Value.Option1;
            _option2 = options.Value.Option2;
        }

        public void DoWork()
        {
            _logger.LogInformation("Begin workouts...");
            _logger.LogInformation(_option1);
            if (_option2)
            {
                _logger.LogInformation("100 sit-ups");
            }
            _logger.LogInformation("Done.");
        }
    }
}
