using ArtCritic.Controller;
using ArtCritic.View.QuestionsPages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        async private void ImageQuestionClick(object sender, EventArgs e)
        {
            QuestionsController questionsController = new QuestionsController();
            questionsController.LoadImageQuestions();
            await Navigation.PushAsync(new ImageQuestionPage(questionsController));
        }
    }
}