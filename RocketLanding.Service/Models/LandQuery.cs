namespace RocketLanding.Service.Models
{
    internal struct LandQuery
    {
        internal string RocketId;
        internal Location Location;

        internal LandQuery(string rocketId, int x, int y)
        {
            Location = new Location(x, y);
            RocketId = rocketId;
        }
    }
}
