using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ArtCritic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageQuestionPage : ContentPage
    {
        private List<Image_Question> db;
        private int image_counter = 0;
        private string currentAnswer_image;

        private int answer_sum_image_correct = 0;

        public ImageQuestionPage()
        {
            InitializeComponent();
            Create_Image_List();
            LoadNewImageQuestion();
        }

        /// <summary>
        /// загрузка нового вопроса картинки
        /// </summary>
        private void LoadNewImageQuestion()
        {
            var q = db[image_counter];
            pice.Source = q.Picture.Source;
            currentAnswer_image = q.Answer;
            image_counter++;
        }

        private void Create_Image_List()
        {
            db = new List<Image_Question>();
            image_counter = 0;

            
            pice.Source = ImageSource.FromResource("ArtCritic.source.Images.pic0.jpg");

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ArtCritic.source.Images.answers.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var e = reader.ReadLine();
                    var args = e.Split('|');
                    db.Add(new Image_Question(args[0], args[1]));
                }
            }
        }


        private void Accept_Answer_Image_Click(object sender, EventArgs e)
        {
            Check_Answer_Image();
            Answer_Image_Texbox.Text = "";
        }

        async public void Check_Answer_Image()
        {
            string word = String.Empty;
            word = Answer_Image_Texbox.Text;

            if (word.ToLower() == currentAnswer_image.ToLower())
            {
                answer_sum_image_correct++;
                image_score.Text = answer_sum_image_correct.ToString();
            }

            if (image_counter != db.Count) { LoadNewImageQuestion(); }
            else
            {
                await DisplayAlert("Молодец!", "твой результат: " + answer_sum_image_correct + "/" + db.Count, "OK");
            }
        }
    }
}