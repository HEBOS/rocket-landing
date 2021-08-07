using RocketLanding.Service.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RocketLanding.Service.Services
{
    public class LandingService
    {
        const string InvalidPlatformSizeConfiguration = "Invalid configuration. You need to provide platform-size configuration setting in format width:height.";

        private readonly Size _landingArea = new(100, 100);
        private const int PlatformGap = 5;

        private readonly Size _platformSize;
        private readonly List<Platform> _platforms = new();
        private LandQuery? _lastCheck;

        public LandingService()
        {
            string platformSetting = ConfigurationManager.AppSettings["platform"];

            if (string.IsNullOrWhiteSpace(platformSetting) || !platformSetting.Contains(":"))
                throw new ConfigurationErrorsException(InvalidPlatformSizeConfiguration);

            string[] platformSplit = platformSetting.Split(":");
            if (int.TryParse(platformSplit[0], out int platformWidth) && int.TryParse(platformSplit[1], out int platformHeight))
                _platformSize = new Size(platformWidth, platformHeight);
            else
                throw new ConfigurationErrorsException(InvalidPlatformSizeConfiguration);

            BuildPlatforms();
        }


        private void BuildPlatforms()
        {
            var maxRows = _landingArea.Height / (_platformSize.Height + PlatformGap);
            var maxColumns = _landingArea.Width / (_platformSize.Width + PlatformGap);

            for(var row = 0; row < maxRows; row++)
            {
                for(var col = 0; col < maxColumns; col++)
                {
                    _platforms.Add(new Platform()
                    {
                        Location = new Location(
                            (col == 0 ? PlatformGap : 0) + col * (_platformSize.Width + PlatformGap),
                            (row == 0 ? PlatformGap : 0) + row * (_platformSize.Height + PlatformGap)),
                        Size = _platformSize
                    });
                }
            }
        }

        public LandingQueryResult CanLand(string rocketId, int x, int y)
        {
            if (_lastCheck != null && _lastCheck?.RocketId != rocketId)
            {
                var request = new LandQuery(rocketId, x, y);
                if (_lastCheck?.Location.Touches(request.Location) ?? false)
                {
                    return LandingQueryResult.Clash;
                }
            }

            // Check if the rocket can find the platform to land
            var platform = _platforms.FirstOrDefault(p =>
                x >= p.Location.X && x <= p.Location.X + p.Size.Width &&
                y >= p.Location.Y && y <= p.Location.Y + p.Size.Height);

            if (platform != null)
            {
                _lastCheck = new LandQuery(rocketId, x, y);
                return LandingQueryResult.Ok;
            }
            else
            {
                return LandingQueryResult.OutOfPlatform;
            }
        }
    }
}
