using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop
{
    class TextQuestion
    {
        protected string Text;
        protected string[] Answers;
        public TextQuestion(string text, string[] answers) {
            Text = text;
            Answers = new string[answers.Length];
            for (int i = 0; i < answers.Length; ++i)
            {
                Answers[i] = new string(answers[i]);
            }
        }
    }
}
