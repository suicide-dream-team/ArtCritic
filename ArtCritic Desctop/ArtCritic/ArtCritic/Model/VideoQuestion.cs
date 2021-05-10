using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic
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
}

