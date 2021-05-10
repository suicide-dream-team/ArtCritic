using System;
using Xamarin.Forms;


namespace ArtCritic
{
    class MusicQuestion : TextQuestion
    {
        public string questionFilename;
        public MusicQuestion(string text, string[] answers, string fileName) : base(text, answers)
        {
            questionFilename = fileName;
        }
    }
}
