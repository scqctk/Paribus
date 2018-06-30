using ArchApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace ArchApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        private async Task Donum_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Donum.DonatePage());
        }

        private async Task Auxillium_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Auxillium.LoginReceive());
        }

    }
}
