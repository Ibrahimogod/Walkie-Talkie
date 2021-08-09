using System;
using System.Collections.Generic;
using System.Text;

namespace Walkie_Talkie.Connection.Connector.Audio
{
    public class AudioMessage
    {
        public bool IsMessageBeginning { get; set; }

        public bool IsMessageEnd { get; set; }

        public byte[] MessageRecord { get; set; }

        public static AudioMessage Create(bool isMessageBeginning,bool isMessageEnd, byte[] messageRecord)
            => new AudioMessage() { IsMessageBeginning = isMessageBeginning, IsMessageEnd = isMessageEnd, MessageRecord = messageRecord };
    }
}
