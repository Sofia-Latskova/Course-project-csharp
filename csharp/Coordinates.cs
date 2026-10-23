namespace Firewatch
{
    public struct Coordinates
    {
        public double Latitude; 
        public double Longitude; 

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return $"({Latitude:F2}, {Longitude:F2})";
        }
    }
}