using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ArtCritic_Desctop
{


   public class Image_Question
    {
        //Изображение для View
        public BitmapImage Picture { get; set; }

        //Ответ на картинку
        public string Answer { get; set; }

        public string Question_for_image { get; set; }

        


        public Image_Question(string PathBitmapImageSource, string AnswerSource, string QuestionSource)
        {
            this.Picture = new BitmapImage();
            this.Picture.BeginInit();

            // !FIXME Костыль жуткий, но по другому не работает. 
            // По хорошему нужно сразу передавать в UriSource относительный путь
            string AbsolutePathBitmapImageSource = Path.GetFullPath(PathBitmapImageSource);

            this.Picture.UriSource = new Uri(AbsolutePathBitmapImageSource, UriKind.RelativeOrAbsolute);
            this.Picture.EndInit();
            this.Question_for_image = QuestionSource;
            this.Answer = AnswerSource;
 
        }

      


    }











}
