using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Walkie_Talkie.Connection;
using Walkie_Talkie.Connection.Connector.Audio;
using Walkie_Talkie.Droid.Service;
using Walkie_Talkie.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Icu.Text.AlphabeticIndex;

namespace Walkie_Talkie.Droid.Service
{
    [Service]
    class ServerBackgroundService : Android.App.Service
    {
        public override IBinder OnBind(Intent intent) => null;

        Server _server;

        static readonly int NOTIFICATION_ID = 2122;
        static readonly string CHANNEL_ID = "bgservice_notification";
        internal static readonly string COUNT_KEY = "count";

        public string ServerAddress => _server.ServerAddress.ToString();

        public override void OnCreate()
        {
            StartForeground();
            base.OnCreate();
        }

        void StartForeground()
        {
#pragma warning disable CS0612 // Type or member is obsolete
            CreateNotificationChannel();
#pragma warning restore CS0612 // Type or member is obsolete
        }

        [Obsolete]
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var name = Resources.GetString(Resource.String.bg_service);
            var description = GetString(Resource.String.bg_description);
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.High)
            {
                Description = description
            };



            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);

            Notification.Builder notificationBuilder = new Notification.Builder(this)
            .SetSmallIcon(Resource.Drawable.ear)
            .SetContentTitle(Resources.GetString(Resource.String.bg_service))
            .SetContentText(Resources.GetString(Resource.String.bg_description))
            .SetChannelId(CHANNEL_ID);

            Notification notification = notificationBuilder.Notification;

            this.StartForeground(NOTIFICATION_ID, notification);

        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Init();
            return base.OnStartCommand(intent, flags, startId);
        }

        void Init()
        {

#pragma warning disable CS0612 // Type or member is obsolete
            _server = Server.GetInstance(ReceivedAudioHandler);
#pragma warning restore CS0612 // Type or member is obsolete
            _server.StartAsync();
            Connectivity.ConnectivityChanged += (o, e) =>
            {
                _server.StartAsync();
            };
        }
        AudioTrack _player;
        int _bufferSize;
        const int SAMPLE_RATE = 16000;
        [Obsolete]
        async Task ReceivedAudioHandler(AudioMessage audio)
        {
            if (audio.IsMessageBeginning)
            {
                _bufferSize = AudioTrack.GetMinBufferSize(SAMPLE_RATE, ChannelOut.Mono, Encoding.Pcm16bit);
                _player = new AudioTrack(Stream.Music, SAMPLE_RATE, ChannelConfiguration.Mono, Encoding.Pcm16bit, _bufferSize, AudioTrackMode.Stream);
                _player.Play();
            }
            else if (audio.IsMessageEnd)
            {
                _player.Stop();
                _player.Release();
                _player.Dispose();
                _player = null;
            }
            _player.Write(audio.MessageRecord,0,audio.MessageRecord.Length);
        }
    }
}