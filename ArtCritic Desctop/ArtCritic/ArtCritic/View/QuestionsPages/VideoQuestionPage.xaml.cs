using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ArtCritic.Controller;

using Xamarin.Forms;

namespace ArtCritic.View.QuestionsPages
{
    public partial class VideoQuestionPage : ContentPage
    {
        private QuestionsController _questionsController;

        public VideoQuestionPage(QuestionsController questionsController)
        {
            InitializeComponent();
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
            VideoQuestion question = (VideoQuestion)_questionsController.GetCurrentQuestion();
            mediaElement.Source = question.Path_To_Video;
        }

        /// <summary>
        /// загрузка нового вопроса видео
        /// </summary>
        private void LoadNewQuestion()
        {
            // Получаем следующий вопрос
            string answer = UserAnswerEntry.Text;
            TextQuestion question = _questionsController.GetNextQuestion(answer);

            // Если вопрос - картинка
            if (typeof(VideoQuestion).IsInstanceOfType(question))
            {
                ScoreLabel.Text = _questionsController.NumberOfCorrectAnswers.ToString();
                VideoQuestion videoQuestion = (VideoQuestion)question;
                mediaElement.Source = videoQuestion.Path_To_Video;
            }
        }

        /// <summary>
        /// EventHandler нажатия на кнопку ввода ответа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

    }
}