using System.Globalization;

namespace RocketLanding.Service.Models
{
    internal struct Location
    {
        internal int X;
        internal int Y;

        internal Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        internal bool Touches(Location nearLocation)
        {
            return nearLocation.X >= X - 1 && nearLocation.X <= X + 1 && 
                   nearLocation.Y >= Y - 1 && nearLocation.Y <= Y + 1;
        }
    }
}
