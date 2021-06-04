using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic
{
    public class VideoQuestion : TextQuestion
    {
        public Uri Path_To_Video;

        public VideoQuestion(string PathBitmapVideoSource, string AnswerSource) : base("", AnswerSource)
        {
            this.Path_To_Video = new Uri(PathBitmapVideoSource, UriKind.RelativeOrAbsolute);
        }
    }
}

