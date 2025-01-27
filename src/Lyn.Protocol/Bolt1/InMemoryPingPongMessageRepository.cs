using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lyn.Protocol.Bolt1.Entities;
using Lyn.Protocol.Common;
using Lyn.Types.Bolt.Messages;
using Lyn.Types.Fundamental;

namespace Lyn.Protocol.Bolt1
{
    public class InMemoryPingPongMessageRepository : IPingPongMessageRepository
    {
        private readonly ConcurrentDictionary<PublicKey, List<TrackedPingPong>> _dictionary = new ();
        private readonly IDateTimeProvider _dateTimeProvider;

        public InMemoryPingPongMessageRepository(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public ValueTask AddPingMessageAsync(PublicKey nodeId,DateTime dateTimeGenerated, PingMessage pingMessage)
        {
            var trackingPing = new TrackedPingPong {Received = dateTimeGenerated, PingMessage = pingMessage};
            
            _dictionary.AddOrUpdate(nodeId,
                _ => new List<TrackedPingPong> {trackingPing},
                (_, pongs) => { pongs.Add(trackingPing); return pongs; });

            return new ValueTask();
        }

        public ValueTask<bool> PendingPingExistsForIdAsync(PublicKey nodeId,ushort pongId)
        {
            var result = _dictionary.ContainsKey(nodeId) &&
                         _dictionary[nodeId].Any(_ => _.PingMessage.PongId == pongId);

            return new ValueTask<bool>(result);
        }

        public ValueTask<bool> MarkPongReplyForPingAsync(PublicKey nodeId,ushort pingId)
        {
            if (!_dictionary.ContainsKey(nodeId))
                return new ValueTask<bool>(false);

            _dictionary[nodeId]
                .Single(_ => _.PingMessage.PongId == pingId)
                .Received = _dateTimeProvider.GetUtcNow();

            return new ValueTask<bool>(true);
        }
    }
}