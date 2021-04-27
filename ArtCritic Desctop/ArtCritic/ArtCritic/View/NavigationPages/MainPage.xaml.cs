using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArtCritic
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async private void Settings_Click(object sender, EventArgs e)
        {
            await DisplayAlert("Не работает", "Пока в разработке", "OK");
        }

        async private void Satistyc_Click(object sender, EventArgs e)
        {
            await DisplayAlert("Не работает", "Пока в разработке", "OK");
        }

        async private void Game_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseGameModePage());
        }

    }

}
