using System;
using Xamarin.Forms;


namespace ArtCritic
{
    class Music_question : TextQuestion
    {
        public string questionFilename;
        public Music_question(string text, string[] answers, string fileName) : base(text, answers)
        {
            questionFilename = fileName;
        }
    }
}
