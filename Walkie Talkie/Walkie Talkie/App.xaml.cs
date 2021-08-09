using Xamarin.Forms;
using Xamarin.Essentials;
using Walkie_Talkie.Helpers;

namespace Walkie_Talkie
{
    public partial class App : Application
    {
        public static int Width { get; set; }

        public static int Height { get; set; }

        static string _serverAddress;
        public static string ServerAddress
        {
            get => _serverAddress;
            set
            {
                _serverAddress = value;
            }
        }

        public App()
        {
            InitializeAsync();
            InitializeComponent();
        }

        protected override void OnStart()
        {
        }

        async void InitializeAsync()
        {
            Permissions.LocationAlways locationAlways = new Permissions.LocationAlways();
            Permissions.LocationWhenInUse locationWhenInUse = new Permissions.LocationWhenInUse();
            Permissions.Media media = new Permissions.Media();
            Permissions.StorageRead storageRead = new Permissions.StorageRead();
            Permissions.StorageWrite storageWrite = new Permissions.StorageWrite();
            Permissions.Camera camera = new Permissions.Camera();
            Permissions.Microphone microphone = new Permissions.Microphone();


            await locationAlways.RequestAsync();
            await locationWhenInUse.RequestAsync();
            await media.RequestAsync();
            await storageRead.RequestAsync();
            await storageWrite.RequestAsync();
            await camera.RequestAsync();
            await microphone.RequestAsync();

            MainPage = new NavigationPage(new Views.MainPage());
            if (Device.RuntimePlatform == Device.Android)
                DependencyService.Get<IServerManager>().Start();

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}