using System.Linq;
using NUnit.Framework;
using RocketLanding.Service.Models;
using RocketLanding.Service.Services;

namespace RocketLanding.Service.Tests
{
    public class Tests
    {
        private LandingService _landingService;

        [OneTimeSetUp]
        public void Setup()
        {
            _landingService = new LandingService();
        }

        [Test, Order(1)]
        public void AvoidLandingOutOfPlatform()
        {
            var result = _landingService.CanLand("A1", 16, 15);
            Assert.AreEqual(result, LandingQueryResult.Ok);
        }

        [Test, Order(2)]
        public void LandA1RocketSafely()
        {
            var result = _landingService.CanLand("A1", 5, 5);
            Assert.AreEqual(result, LandingQueryResult.Ok);

            // Retest the same rocket using the same location
            result = _landingService.CanLand("A1", 5, 5);
            Assert.AreEqual(result, LandingQueryResult.Ok);
        }

        [Test, Order(3)]
        public void AvoidClashA2RocketWithA1Rocket()
        {
            var result = _landingService.CanLand("A2", 5, 5);
            Assert.AreEqual(result, LandingQueryResult.Clash);
        }

        [Test, Order(4)]
        public void AvoidLandingNextToAnotherRocket()
        {
            // Ask for landing of A1, so we can test if A2 will clash
            _landingService.CanLand("A1", 7, 7);

            LandingQueryResult[] results =
            {
                
                _landingService.CanLand("A2", 7, 8),
                _landingService.CanLand("A2", 6, 7),
                _landingService.CanLand("A2", 6, 6)
            };
            
            Assert.AreEqual(results.All(r => r == LandingQueryResult.Clash), true);
        }
    }
}