using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Walkie_Talkie.Helpers
{
    public interface IRecorderService
    {
        void startRecording(Func<byte[],Task> handleAudioInput);

        void stopRecording();
    }
}
