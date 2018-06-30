using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArchApp.Views.Donum
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DonatePage : ContentPage
    {
        private Button DonateButton = new Button();

        public DonatePage()
        {
            InitializeComponent();
            DonateButton = Donate_Now_Button;
        }

        private async Task Donate_Now_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new loginPage());
        }
    }
}