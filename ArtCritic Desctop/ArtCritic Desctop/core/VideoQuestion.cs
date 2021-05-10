using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ArtCritic_Desctop
{
    public class VideoQuestion : TextQuestion
    {

        public Uri Path_To_Video;

        //Ответ на картинку
        //public string Answer_for_video { get; set; }
        //унаследовал от текстового

        public VideoQuestion(string PathBitmapVideoSource, string AnswerSource):base("", AnswerSource)
        {
            this.Path_To_Video = new Uri(PathBitmapVideoSource, UriKind.RelativeOrAbsolute);
        }

    }

}

