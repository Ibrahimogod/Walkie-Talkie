using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walkie_Talkie.Models;
using Walkie_Talkie.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Walkie_Talkie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommunicationPage : ContentPage
    {
        CommunicationViewModel VM;

        public CommunicationPage(ClientInfo client)
        {
            InitializeComponent();
            VM = BindingContext as CommunicationViewModel;
            VM.Client = client;
        }

        private void btnRecord_Pressed(object sender, EventArgs e)
        {
            VM.RecordCommand?.Execute(null);
        }

        private void btnRecord_Released(object sender, EventArgs e)
        {
            VM.StopCommand?.Execute(null);
        }
    }
}