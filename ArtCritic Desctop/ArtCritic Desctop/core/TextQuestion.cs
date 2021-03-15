using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop
{
    class TextQuestion
    {
        public readonly string Text;//публично временно, на время тестов
        protected string[] Answers;//публично временно, на время тестов
        public TextQuestion(string text, string[] answers) {
            Text = text;
            Answers = new string[answers.Length];
            for (int i = 0; i < answers.Length; ++i)
            {
                Answers[i] = new string(answers[i]);
            }
        }
        public bool Check_Answer(string ans) { 
           foreach(string ch_ans in Answers)
            {
                if (string.CompareOrdinal(ch_ans, ans)==0 ){ return true; }
            }
            return false;
        }
    }
}
