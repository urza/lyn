using System;
using Lyn.Types.Bolt;
using Lyn.Types.Fundamental;

namespace Lyn.Protocol.Tests
{
    public static class RandomMessages
    {
        private static Random _random = new Random();
        
        public static ChannelId NewRandomChannelId()
        {
            return new ChannelId(GetRandomByteArray(ChannelId.LENGTH));
        }
            
        public static ShortChannelId NewRandomShortChannelId()
        {
            return new ShortChannelId(GetRandomByteArray(ShortChannelId.LENGTH));
        }
        
        public static CompressedSignature NewRandomCompressedSignature()
        {
            return new CompressedSignature(GetRandomByteArray(CompressedSignature.LENGTH));
        }

        public static PublicKey NewRandomPublicKey()
        {
            return new PublicKey(GetRandomByteArray(PublicKey.LENGTH));
        }
        
        public static ChainHash NewRandomChainHash()
        {
            return new ChainHash(GetRandomByteArray(32));
        }
        
        public static byte[] GetRandomByteArray(int length)
        {
            var bytes = new byte[length];
            
            _random.NextBytes(bytes);

            return bytes;
        }

        public static uint GetRandomNumberUInt32()
        {
            return (uint) _random.Next(0, int.MaxValue);
        }
        
        public static ushort GetRandomNumberUInt16()
        {
            return (ushort) _random.Next(0, short.MaxValue);
        }
    }
}