using System;
using System.Collections.Generic;
using System.Text;

namespace Walkie_Talkie.Connection.Connector.Audio
{
    public class AudioChannel : Channel<AudioMessageProtocol, AudioMessage> { }

    public class AudioClientChannel : ClientChannel<AudioMessageProtocol, AudioMessage> { }
}
