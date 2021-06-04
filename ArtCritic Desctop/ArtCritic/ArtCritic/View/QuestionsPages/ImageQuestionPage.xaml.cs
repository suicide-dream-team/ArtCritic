using ArtCritic.Controller;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtCritic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageQuestionPage : ContentPage
    {

        private QuestionsController _questionsController;

        public ImageQuestionPage(QuestionsController QuestionsController)
        {
            InitializeComponent();
            _questionsController = QuestionsController;

            // Обновляем Label с количеством очков, так как возможно мы пришли от другого типа вопроса
            ScoreLabel.Text = _questionsController.NumberOfCorrectAnswers.ToString();
            if (_questionsController.IsTheAnyQuestionsAvailable())
            {
                LoadNewQuestion();
            }
        }

        /// <summary>
        /// Загрузка нового вопроса с возможным переходом на другой тип страницы
        /// </summary>
        private void LoadNewQuestion()
        {
            // Получаем следующий вопрос
            string answer = UserAnswerEntry.Text;
            TextQuestion question = _questionsController.GetNextQuestion(answer);

            // Если вопрос - картинка
            if (typeof(ImageQuestion).IsInstanceOfType(question))
            {
                ScoreLabel.Text = _questionsController.NumberOfCorrectAnswers.ToString();
                ImageQuestion imageQuestion = (ImageQuestion)question;
                currentPicture.Source = imageQuestion.Picture.Source;
            }
        }

        /// <summary>
        /// EventHandler нажатия на кнопку ввода ответа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptAnswerImageClick(object sender, EventArgs e)
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
    }
}