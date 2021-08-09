using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Walkie_Talkie.Droid.Service;
using Walkie_Talkie.Helpers;
using Xamarin.Forms;

[assembly:Dependency(typeof(RecorderService))]
namespace Walkie_Talkie.Droid.Service
{
    class RecorderService : IRecorderService
    {
        AudioRecord _record;
        readonly int _bufferSize;
        const int SAMPLE_RATE = 16000;
        Func<byte[], Task> _audioInputHandler;

        public RecorderService()
        {
            _bufferSize = AudioTrack.GetMinBufferSize(SAMPLE_RATE, ChannelOut.Mono, Encoding.Pcm16bit);
        }

        public void startRecording(Func<byte[], Task> handleAudioInput)
        {
            _audioInputHandler = handleAudioInput;
            if (_record == null)
            {
                _record = new AudioRecord(AudioSource.Mic, 16000, ChannelIn.Mono, Encoding.Pcm16bit, _bufferSize);
                _record.StartRecording();
                _ = Task.Run(Start);
            }
        }

        private void Start()
        {
            try
            {
                while (_record.RecordingState == RecordState.Recording)
                {
                    byte[] buffer = new byte[_bufferSize];
                    _record.Read(buffer, 0, _bufferSize);
                    _audioInputHandler(buffer);
                }
            }
            catch
            {
                stopRecording();
            }
        }

        public void stopRecording()
        {
            _record.Stop();
            _record.Release();
            _record.Dispose();
            _record = null;
        }
    }
}