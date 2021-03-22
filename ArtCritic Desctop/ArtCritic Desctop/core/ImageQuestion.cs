using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ArtCritic_Desctop
{
    class QuestionDB_Image
    {
        /// <summary>
        /// коллекция вопросов
        /// </summary>
        private List<ImageQuestion> db;

        /// <summary>
        /// индекс текущего вопроса
        /// </summary>
        private int index;

        /// <summary>
        /// текущий вопрос
        /// </summary>
        /// 
        public ImageQuestion CurrentQuestion
        {
            get {
                index++;
                return db[index % db.Count];
            }
        }
        
        /// <summary>
        /// создаёт базу данных для вопросов где хранит путь к картинке и правильный ответ
        /// </summary>
        public QuestionDB_Image() {
            this.db = new List<ImageQuestion>();
            this.index = 0;

            var Image_dataFile = File.ReadAllLines(@"..\..\..\Images\answers.txt");
            foreach (var e in Image_dataFile) {
                var args = e.Split('|');
                db.Add(new ImageQuestion(args[0], args[1]));

            
            }
        
        
        }
 
    }







    class Image_logic
    {
        Image image;

        string current_Image_Answer;

        QuestionDB_Image Image_data;

        public TextBox Image_text_box;

        public void Check_Image_Answer() 
        {
            string word = String.Empty;
            word += Image_text_box.Text;
            if (word == current_Image_Answer) LoadNew_Image_Question();
        }

        private void LoadNew_Image_Question() 
           {
            var q = Image_data.CurrentQuestion;

            image.Source = q.Picture;

            current_Image_Answer = q.Answer_For_Images;
             }

        public Image_logic(TextBox Image_Textbox_for_Answers,Image ImageView) 
        {

            Image_data = new QuestionDB_Image();

            image = ImageView;

            Image_text_box = Image_Textbox_for_Answers;
          
          


            LoadNew_Image_Question();

        }






    }






    class ImageQuestion 
    {
 
       
        
        /// <summary>
        /// Изображение для view
        /// </summary>
        public BitmapImage Picture { get; set; }

        /// <summary>
        /// Ответ на картинку
        /// </summary>
        public string Answer_For_Images { get; set; }





        /// <summary>
        /// создание вопроса для Image режима
        /// </summary>
        /// <param name="text"></param>
        /// <param name="answers"></param>
        public ImageQuestion(string PathBitmapImageSource, string AnswerSource ) 
        {

            this.Picture = new BitmapImage();
            this.Picture.BeginInit();
            this.Picture.UriSource = new Uri(PathBitmapImageSource, UriKind.Relative);
            this.Picture.EndInit();

            this.Answer_For_Images = AnswerSource;
        }


    }

}
