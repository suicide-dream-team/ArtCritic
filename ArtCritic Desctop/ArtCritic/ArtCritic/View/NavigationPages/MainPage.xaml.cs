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

        async private void StatisticsClick(object sender, EventArgs e)
        {
            await DisplayAlert("Не работает", "Пока в разработке", "OK");
        }

        async private void GameClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseGameModePage());
        }

    }

}
