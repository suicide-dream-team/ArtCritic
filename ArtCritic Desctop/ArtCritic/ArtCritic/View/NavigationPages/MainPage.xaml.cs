using ArtCritic.Controller;
using ArtCritic.View.QuestionsPages;
using System;
using Xamarin.Forms;

namespace ArtCritic
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async private void HelpClick(object sender, EventArgs e)
        {
            await DisplayAlert("Помощь", "Данное приложения является портом Desktop-игры, разработанной в ходе предмета Операционные системы", "OK");
        }

        async private void GameClick(object sender, EventArgs e)
        {
            QuestionsController questionsController = new QuestionsController();
            questionsController.LoadImageQuestions();
            questionsController.LoadVideoQuestions();
            questionsController.LoadMusicQuestions();
            questionsController.ShuffleQuestionsList();
            TextQuestion question = questionsController.GetCurrentQuestion();
            if (typeof(VideoQuestion).IsInstanceOfType(question))
            {
                await Navigation.PushAsync(new VideoQuestionPage(questionsController));
            }
            else if (typeof(ImageQuestion).IsInstanceOfType(question))
            {
                await Navigation.PushAsync(new ImageQuestionPage(questionsController));
            }
            else if (typeof(MusicQuestion).IsInstanceOfType(question))
            {
                await Navigation.PushAsync(new MusicQuestionPage(questionsController));
            }
            //await Navigation.PushAsync(new ChooseGameModePage());
        }

    }

}
