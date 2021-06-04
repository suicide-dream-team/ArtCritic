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
        private List<ImageQuestion> db;
        private int image_counter = 0;
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
            image_counter++;
        }

        private void Create_Image_List()
        {
            db = new List<ImageQuestion>();
            image_counter = 0;


            pice.Source = ImageSource.FromResource("ArtCritic.Data.Images.pic0.jpg");

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ArtCritic.Data.Images.answers.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var e = reader.ReadLine();
                    var args = e.Split('|');
                    db.Add(new ImageQuestion(args[0], args[1]));
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

            if (db[image_counter].CheckAnswer(word))
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