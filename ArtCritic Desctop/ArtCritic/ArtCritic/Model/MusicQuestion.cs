using System;
using Xamarin.Forms;


namespace ArtCritic
{
    class MusicQuestion : TextQuestion
    {
        public string QuestionFilename;
        public MusicQuestion(string text, string[] answers, string fileName) : base(text, answers)
        {
            QuestionFilename = fileName;
        }
    }
}
