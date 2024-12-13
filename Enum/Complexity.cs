using System.Text.Json.Serialization;

namespace Project_Manager.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Complexity
    {
        Easy,
        Medium,
        Complex
    }
}


//to add status enum later on