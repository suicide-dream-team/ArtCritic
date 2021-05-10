using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Core;

namespace ArtCritic.View.QuestionsPages
{
    public partial class VideoQuestionPage : ContentPage
    {

        private List<VideoQuestion> db_video;
        int video_counter = 0;
        int answer_sum_video_correct = 0;
        string currentAnswer_video;

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
            currentAnswer_video = newVideoQuestion.Answer_for_video;
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
            word = Answer_Video_Texbox.Text;
            Answer_Video_Texbox.Text = "";

            if (word.ToLower() == currentAnswer_video.ToLower())
            {
                answer_sum_video_correct++;
                video_score.Text = answer_sum_video_correct.ToString();
            }

            if (video_counter != db_video.Count) { LoadNewVideoQuestion(); }
            else
            {
                await DisplayAlert("Молодец!", "твой результат: " + answer_sum_video_correct + "/" + db_video.Count, "OK");
            }
        }

    }
}