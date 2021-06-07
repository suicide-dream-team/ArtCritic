using ArtCritic.Controller;
using ArtCritic.View.QuestionsPages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtCritic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicQuestionPage : ContentPage
    {
        private QuestionsController _questionsController;

        IMusicPlayer musicPlayer;
        public MusicQuestionPage(QuestionsController questionsController)
        {
            InitializeComponent();



            musicPlayer = DependencyService.Get<IMusicPlayer>();
            _questionsController = questionsController;

            // Обновляем Label с количеством очков, так как возможно мы пришли от другого типа вопроса
            ScoreLabel.Text = _questionsController.NumberOfCorrectAnswers.ToString();
            if (_questionsController.IsTheAnyQuestionsAvailable())
            {
                DisplayCurrentQuestion();
            }




        }

        /// <summary>
        /// Отображение текущего вопроса
        /// </summary>
        private void DisplayCurrentQuestion()
        {
            MusicQuestion question = (MusicQuestion)_questionsController.GetCurrentQuestion();
            musicPlayer.Open(question.QuestionFilename);
            musicPlayer.Play();
        }

        private void LoadNewQuestion()
        {
            // Получаем следующий вопрос
            string answer = UserAnswerEntry.Text;
            TextQuestion question = _questionsController.GetNextQuestion(answer);

            // Если вопрос - картинка
            if (typeof(MusicQuestion).IsInstanceOfType(question))
            {
                ScoreLabel.Text = _questionsController.NumberOfCorrectAnswers.ToString();
                MusicQuestion musicQuestion = (MusicQuestion)question;

                musicPlayer.Open(musicQuestion.QuestionFilename);
                musicPlayer.Play();
            }
            else if (typeof(VideoQuestion).IsInstanceOfType(question))
            {
                Navigation.PushAsync(new VideoQuestionPage(_questionsController));
                musicPlayer.Stop();
                NavigationPage navigationPage = (NavigationPage)App.Current.MainPage;
                navigationPage.Navigation.RemovePage(navigationPage.Navigation.NavigationStack[navigationPage.Navigation.NavigationStack.Count - 2]);
            }
            else if (typeof(ImageQuestion).IsInstanceOfType(question))
            {
                Navigation.PushAsync(new ImageQuestionPage(_questionsController));
                musicPlayer.Stop();
                NavigationPage navigationPage = (NavigationPage)App.Current.MainPage;
                navigationPage.Navigation.RemovePage(navigationPage.Navigation.NavigationStack[navigationPage.Navigation.NavigationStack.Count - 2]);
            }
        }

        private void AcceptAnswerClick(object sender, EventArgs e)
        {
            CheckForEnd();
            UserAnswerEntry.Text = "";
        }

        /// <summary>
        /// Загрузка следующего вопроса с проверкой на конец игры
        /// </summary>
        async private void CheckForEnd()
        {
            if (_questionsController.IsTheAnyQuestionsAvailable())
            {
                LoadNewQuestion();
            }
            else
            {
                // Выводим результат
                int numberOfCorrectAnswers = _questionsController.NumberOfCorrectAnswers;
                int numberOfAllQuestions = _questionsController.GetNumberOfQuestions();

                await DisplayAlert("Молодец!", "твой результат: " + numberOfCorrectAnswers + "/" + numberOfAllQuestions, "OK");
            }
        }

        private void Music_replay_music_Click(object sender, EventArgs e)
        {
            DisplayCurrentQuestion();
        }

        protected override void OnDisappearing()
        {
            musicPlayer.Stop();
        }
    }
}