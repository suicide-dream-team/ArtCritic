using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Xamarin.Forms;

namespace ArtCritic.View.QuestionsPages
{
    public partial class VideoQuestionPage : ContentPage
    {
        private List<VideoQuestion> db_video;
        int video_counter = 0;
        int answer_sum_video_correct = 0;

        public VideoQuestionPage()
        {
            InitializeComponent();
            Create_Video_List();
            LoadNewVideoQuestion();
        }

        /// <summary>
        /// Создаётся List элементов Video_question
        /// </summary>
        void Create_Video_List()
        {
            db_video = new List<VideoQuestion>();
            video_counter = 0;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ArtCritic.Data.Videos.answersV.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var e = reader.ReadLine();
                    var args = e.Split('|');
                    db_video.Add(new VideoQuestion(args[0], args[1]));
                }
            }
        }

        /// <summary>
        /// загрузка нового вопроса видео
        /// </summary>
        private void LoadNewVideoQuestion()
        {
            var newVideoQuestion = db_video[video_counter];
            mediaElement.Source = newVideoQuestion.Path_To_Video;

            video_counter++;
        }
        /// <summary>
        /// Нажатие на кнопку "Ввести"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept_Answer_Video_Click(object sender, EventArgs e)
        {
            Check_Answer_video();
        }
        /// <summary>
        /// проверка ответа на видео
        /// </summary>
        async public void Check_Answer_video()
        {
            string word = String.Empty;
            word = AnswerTextbox.Text;
            AnswerTextbox.Text = "";

            if (db_video[video_counter].CheckAnswer(word.ToLower()))
            {
                answer_sum_video_correct++;
                video_score.Text = answer_sum_video_correct.ToString();
            }

            if (video_counter != db_video.Count)
            {
                LoadNewVideoQuestion();
            }
            else
            {
                await DisplayAlert("Молодец!", "твой результат: " + answer_sum_video_correct + "/" + db_video.Count, "OK");
            }
        }

    }
}