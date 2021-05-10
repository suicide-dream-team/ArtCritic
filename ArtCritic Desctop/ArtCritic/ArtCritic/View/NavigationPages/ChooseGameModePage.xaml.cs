using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ArtCritic.View.QuestionsPages;

namespace ArtCritic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseGameModePage : ContentPage
    {
        public ChooseGameModePage()
        {
            InitializeComponent();
        }
        async private void Video_question_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VideoQuestionPage());
        }


        async private void Music_question_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MusicQuestionPage());
        }

        async private void Mixed_questions_Click(object sender, EventArgs e)
        {
            await DisplayAlert("Не работает", "Пока в разработке", "OK");
        }

        async private void Image_question_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImageQuestionPage());
        }
    }
}