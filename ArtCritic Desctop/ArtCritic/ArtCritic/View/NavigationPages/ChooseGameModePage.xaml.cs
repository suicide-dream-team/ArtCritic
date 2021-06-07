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
            QuestionsController questionsController = new QuestionsController();
            questionsController.LoadVideoQuestions();
            await Navigation.PushAsync(new VideoQuestionPage(questionsController));
        }


        async private void Music_question_Click(object sender, EventArgs e)
        {
            QuestionsController questionsController = new QuestionsController();
            questionsController.LoadMusicQuestions();
            await Navigation.PushAsync(new MusicQuestionPage(questionsController));
        }

        async private void Mixed_questions_Click(object sender, EventArgs e)
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
        }

        async private void ImageQuestionClick(object sender, EventArgs e)
        {
            QuestionsController questionsController = new QuestionsController();
            questionsController.LoadImageQuestions();
            await Navigation.PushAsync(new ImageQuestionPage(questionsController));
        }
    }
}