using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop
{
    public class TextQuestion
    {
        public string Text;//публично временно, на время тестов
        public string[] Answers;//публично временно, на время тестов
        public TextQuestion() { }
        public TextQuestion(string text, string[] answers) {
            Text = text;
            Answers = answers;
        }
        public TextQuestion(string text, string answer) {
            Answers = new string[1];
            Answers[0] = answer;
            Text = text;
        }
        public bool Check_Answer(string ans) {
            //foreach(string ch_ans in Answers)
            for (int i = 0; i < Answers.Length; ++i)
            {
                bool check = (Answers[i]==ans);
                if (check)                    
                { 
                    return true; 
                }
               
            }
            return false;
        }
    }
}
