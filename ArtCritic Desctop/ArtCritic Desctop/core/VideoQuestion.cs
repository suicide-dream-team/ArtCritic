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


        public VideoQuestion(string PathBitmapVideoSource, string AnswerSource)
        {
            this.Path_To_Video = new Uri(PathBitmapVideoSource, UriKind.RelativeOrAbsolute);
            this.Answer_for_video = AnswerSource;

        }








    }


    // Uri lol = new Uri("C:/Users/Ярослав/source/repos/ArtCritic/ArtCritic Desctop/ArtCritic Desctop/core/Videos/Video.mp4");


}

