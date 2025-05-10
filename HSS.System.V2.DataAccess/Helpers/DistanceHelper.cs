using GeoCoordinatePortable;

namespace HSS.System.V2.DataAccess.Helpers
{
    public static class DistanceHelper
    {
        public static double GetDistance(double? FromLat, double? FromLng, double? ToLat, double? ToLng) //VM : ViewModel
        {
            if (FromLat == null || FromLng == null || ToLat == null || ToLng == null)
                return double.MaxValue;

            if (FromLat > 90 || FromLat < -90)
                return 0;
            if (ToLat > 90 || ToLat < -90)
                return 0;
            if (FromLng > 180 || FromLng < -180)
                return 0;
            if (ToLng > 180 || ToLng < -180)
                return 0;

            // calculate distance
            GeoCoordinate userLocation = new GeoCoordinate(FromLat.Value, FromLng.Value); // userLocation
            GeoCoordinate targetLocation = new GeoCoordinate(ToLat.Value, ToLng.Value);//ResturantLocation

            var distanceBetweenUserAndResturant = (userLocation.GetDistanceTo(targetLocation)) / 1000; //Distance in KM

            return distanceBetweenUserAndResturant; //Adding Distance between User and all Branches in a List

        }
    }
}
