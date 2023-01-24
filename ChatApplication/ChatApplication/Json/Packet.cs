using Newtonsoft.Json;

namespace ChatApplication.Json;

/// <summary>
/// The packet class that will be used to send and receive messages
/// The packet will be in the format of:
/// - The sender's name
/// - The receiver's name
/// - The message
///
/// The packet will be serialized to JSON
/// The packet can also be deserialized from JSON
/// </summary>
public class Packet
{
    // The sender's name in JSONProperty
    [JsonProperty("sender")] public User Sender { get; set; }

    // The receiver's name in JSONProperty
    [JsonProperty("receiver")] public User Receiver { get; set; }

    // The message in JSONProperty
    [JsonProperty("message")] public string Message { get; set; }


    // Makes a packet
    public Packet(User sender, User receiver, string message)
    {
        Sender = sender;
        Receiver = receiver;
        Message = message;
    }
    
    // Makes printable packet
    public override string ToString()
    {
        return $"[Packet: Sender={Sender}, Receiver={Receiver}, Message={Message}]";
    }
    
    // Serializes packet to JSON
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
    
    // Deserializes packet from JSON. If it fails, it will return null
    public static Packet? FromJson(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<Packet>(json);
        }
        catch
        {
            return null;
        }
    }
}