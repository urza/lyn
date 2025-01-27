using System.Buffers;
using Lyn.Types.Bolt;
using Lyn.Types.Bolt.Messages;

namespace Lyn.Types.Serialization.Serializers
{
    public class ErrorMessageSerializer : IProtocolTypeSerializer<ErrorMessage>
    {
        public int Serialize(ErrorMessage typeInstance, IBufferWriter<byte> writer,
            ProtocolTypeSerializerOptions? options = null)
        {
            var size = 0;
            size += writer.WriteBytes(typeInstance.ChannelId);
            size += writer.WriteUShort(typeInstance.Len, true);
            size += writer.WriteBytes(typeInstance.Data);

            return size;
        }

        public ErrorMessage Deserialize(ref SequenceReader<byte> reader, ProtocolTypeSerializerOptions? options = null)
        {
            var channelId = reader.ReadBytes(32).ToArray();
            ushort len = reader.ReadUShort(true);

            return new ErrorMessage
            {
                ChannelId = new ChannelId(channelId), 
                Len = len, 
                Data = reader.ReadBytes(len).ToArray()
            };
        }
    }
}