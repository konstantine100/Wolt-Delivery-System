using RestSharp;
using Newtonsoft.Json.Linq;
namespace Wolt.Services;

public class RoadCalculator
{
    static public (decimal, decimal) GetCoordinates(string address)
    {
        var client = new RestClient("https://nominatim.openstreetmap.org/search");
        var request = new RestRequest("/", Method.Get);
        request.AddParameter("q", address);
        request.AddParameter("format", "json");

        var response = client.Execute(request);
        var data = JArray.Parse(response.Content);

        if (data.Count > 0)
        {
            decimal lat = decimal.Parse(data[0]["lat"].ToString());
            decimal lon = decimal.Parse(data[0]["lon"].ToString());
            return (lat, lon);
        }
        else
        {
            throw new Exception($"Address '{address}' not found.");
        }
    }
    
    static public decimal GetRoadDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        var client = new RestClient($"http://router.project-osrm.org/route/v1/driving/{lon1},{lat1};{lon2},{lat2}?overview=false");
        var request = new RestRequest("/", Method.Get);

        var response = client.Execute(request);
        var data = JObject.Parse(response.Content);

        if (data["routes"] != null && data["routes"].HasValues)
        {
            return data["routes"][0]["distance"].ToObject<decimal>() / 1000; 
        }
        else
        {
            throw new Exception("Could not calculate distance.");
        }
    }
}