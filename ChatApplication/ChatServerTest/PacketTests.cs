using System;
using System.IO;
using System.Text;
using ChatServer.NET.IO;

namespace ChatApplicationTest
{
    public class PacketTests
    {
        private PacketBuilder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new PacketBuilder();
        }

        [Test]
        public void WriteOpCode_ShouldWriteTheGivenOpCodeToThePacket()
        {
            // Arrange
            byte opcode = 0x01;

            // Act
            _builder.WriteOpCode(opcode);

            // Assert
            byte[] packetBytes = _builder.GetPacketBytes();
            Assert.That(packetBytes, Has.Length.EqualTo(1));
            Assert.That(packetBytes[0], Is.EqualTo(opcode));
        }

        [Test]
        public void WriteMessage_ShouldWriteTheGivenMessageToThePacket()
        {
            // Arrange
            string message = "Hello, World!";

            // Act
            _builder.WriteMessage(message);

            // Assert
            byte[] packetBytes = _builder.GetPacketBytes();
            var ms = new MemoryStream(packetBytes);
            var reader = new BinaryReader(ms);
            var msgLength = reader.ReadInt32();
            var msg = Encoding.ASCII.GetString(reader.ReadBytes(msgLength));
            Assert.That(msg, Is.EqualTo(message));
        }

        [Test]
        public void GetPacketBytes_ShouldReturnTheBytesOfThePacket()
        {
            // Arrange
            byte opcode = 0x01;
            string message = "Hello, World!";

            // Act
            _builder.WriteOpCode(opcode);
            _builder.WriteMessage(message);
            byte[] packetBytes = _builder.GetPacketBytes();

            // Assert
            var ms = new MemoryStream();
            ms.WriteByte(opcode);
            var msgLength = message.Length;
            ms.Write(BitConverter.GetBytes(msgLength));
            ms.Write(Encoding.ASCII.GetBytes(message));
            var expectedBytes = ms.ToArray();
            Assert.That(packetBytes, Is.EqualTo(expectedBytes));
        }
    }
}