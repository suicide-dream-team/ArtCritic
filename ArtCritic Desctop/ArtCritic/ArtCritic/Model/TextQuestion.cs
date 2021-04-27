using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic
{
    class TextQuestion
    {
        public readonly string Text;//публично временно, на время тестов
        protected string[] Answers;//публично временно, на время тестов
        public TextQuestion(string text, string[] answers) {
            Text = text;
            Answers = answers;
        }
        public bool Check_Answer(string ans) {
            //foreach(string ch_ans in Answers)
            for (int i = 0; i < Answers.Length; ++i)
            {
                bool check = (Answers[i].ToLower()==ans.ToLower());
                if (check)                    
                { 
                    return true; 
                }
               
            }
            return false;
        }
    }
}
