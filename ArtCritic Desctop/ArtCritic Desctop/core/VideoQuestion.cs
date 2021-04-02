using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ArtCritic_Desctop
{
    public class VideoQuestion
    {

        public Uri Path_To_Video;

        //Ответ на картинку
        public string Answer_for_video { get; set; }

        public string Question_for_video { get; set; }


        public VideoQuestion(string PathBitmapImageSource, string AnswerSource, string QuestionSource)
        {
            this.Path_To_Video = new Uri(PathBitmapImageSource, UriKind.RelativeOrAbsolute);
            this.Question_for_video = QuestionSource;
            this.Answer_for_video = AnswerSource;

        }








    }


    // Uri lol = new Uri("C:/Users/Ярослав/source/repos/ArtCritic/ArtCritic Desctop/ArtCritic Desctop/core/Videos/Video.mp4");


}

