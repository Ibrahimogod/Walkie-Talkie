using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Walkie_Talkie.Connection.Connector.Audio
{
    public class AudioMessageProtocol : Protocol<AudioMessage>
    {
        protected override AudioMessage Decode(byte[] message)
        {
            byte[] boolbytes = new byte[8];
            Array.Copy(message, boolbytes, 8);
            byte[] firstbool = new byte[4];
            byte[] secondbool = new byte[4];
            Array.ConstrainedCopy(boolbytes, 0, firstbool, 0, 4);
            Array.ConstrainedCopy(boolbytes, 4, secondbool, 0, 4);
            var length = message.Length - 8;
            byte[] messagebody = new byte[length];
            Array.Copy(message, 8, messagebody, 0, length);
            var ismessageBeginning = Convert.ToBoolean(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(firstbool,0)));
            var ismessageEnd = Convert.ToBoolean(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(secondbool, 0)));
            return AudioMessage.Create(ismessageBeginning,ismessageEnd, messagebody);
        }

        protected override byte[] EncodeBody<T>(T message)
        {
            AudioMessage audioMessage = message as AudioMessage;
            int ismessageBeginning = Convert.ToInt32(audioMessage.IsMessageBeginning);
            int ismessageEnd = Convert.ToInt32(audioMessage.IsMessageEnd);
            var messageBeginningbytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(ismessageBeginning));
            var messageEndbytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(ismessageEnd));
            List<byte> body = new List<byte>();
            body.AddRange(messageBeginningbytes);
            body.AddRange(messageEndbytes);
            body.AddRange(audioMessage.MessageRecord);
            return body.ToArray();
        }
    }
}
