using System.Net.Sockets;
using Newtonsoft.Json;

namespace ChatServer.Json;

/// <summary>
/// User class that will hold the user's name and the TcpClient
/// </summary>
public class User
{
    // The user's name in JSONProperty
    [JsonProperty("name")] public string Name { get; set; }

    // The TcpClient in JSONProperty
    [JsonProperty("client")] public TcpClient Client { get; set; }
    
    // Makes a user
    public User(string name, TcpClient client)
    {
        Name = name;
        Client = client;
    }
    
    // Makes printable user
    public override string ToString()
    {
        return $"[User: Name={Name}, Client={Client}]";
    }
    
    // Serializes user to JSON
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
    
    // Deserializes user from JSON. If it fails, it will return null
    public static User? FromJson(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<User>(json);
        }
        catch
        {
            return null;
        }
    }
}