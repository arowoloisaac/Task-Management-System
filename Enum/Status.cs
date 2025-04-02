
using System.Text.Json.Serialization;

namespace Project_Manager.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Accepted,
        Rejected,
        Pending
    }
}
