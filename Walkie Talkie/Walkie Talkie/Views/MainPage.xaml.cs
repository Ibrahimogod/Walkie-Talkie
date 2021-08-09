using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkie_Talkie.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Walkie_Talkie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        MainViewModel VM;
        public MainPage()
        {
            InitializeComponent();
            VM = BindingContext as MainViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            VM.IsRefreshing = true;
            await VM.ReloadData();
        }
    }
}