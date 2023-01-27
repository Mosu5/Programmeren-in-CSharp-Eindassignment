using System.Text;

namespace ChatServer.NET.IO;

public class PacketBuilder
{
    private MemoryStream _ms;

    public PacketBuilder()
    {
        _ms = new MemoryStream();
    }

    public void WriteOpCode(byte opcode)
    {
        _ms.WriteByte(opcode);
    }
    
    /*
     * Writes a string to the packet
     * opcode: 0x01
     * length: 4 bytes
     * string: length in bytes
     */
    public void WriteMessage(string msg)
    {
        var msgLength = msg.Length;
        _ms.Write(BitConverter.GetBytes(msgLength));
        _ms.Write(Encoding.ASCII.GetBytes(msg));
    }
    
    public byte[] GetPacketBytes()
    {
        return _ms.ToArray();
    }
}