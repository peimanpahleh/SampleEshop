using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Baskets.Infrastructure.Persistence;

public class BsketRedisJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(Basket));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        // Load the JSON for the Result into a JObject
        JObject jo = JObject.Load(reader);

        // Read the properties which will be used as constructor parameters
        var id = Guid.Parse(jo["Id"].ToString());
        var buyerName = jo["BuyerName"].ToString();
        var itemsJson = jo["BasketItems"].ToString();
        var version = int.Parse(jo["Version"].ToString());

        // with NewtonSoft
        var items = JsonConvert.DeserializeObject<List<BasketItem>>(itemsJson);

        // with System.Text.Json
        /*var items = System.Text.Json.JsonSerializer.Deserialize<List<BasketItem>>(itemsJson, new System.Text.Json.JsonSerializerOptions());*/

        // Construct the Result object using the non-default constructor
        Basket result = new Basket(id, buyerName);
        result.AddItems(items);

        result.SetVersion(version);
        return result;
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
    {

    }
}