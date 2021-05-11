using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop
{
    [Serializable]
    class QuestionKeeper//пока что не определили общий вид запросов пусть объект вопроса в игре выглядит так
    {
        public enum State { None, Text, Image, Video, Music, Eror };
        protected State state = State.None;
        public string Text { get; set; }
        public string Link { get; set; }       
        public string[] Answers { get; set; }

        public QuestionKeeper() { }

        public QuestionKeeper(string text, string[] answers) {
            Text = text;
            state = State.Text;
            Answers = new string[answers.Length];
            for (int i = 0; i < answers.Length; ++i) {
                Answers[i] = new string(answers[i]);
            }
        }

        public QuestionKeeper(string text, string new_Link, int newState, string[] answers)
        {
            Text = text;
            Answers = new string[answers.Length];
            for (int i = 0; i < answers.Length; ++i)
            {
                Answers[i] = new string(answers[i]);
            }
            switch (newState)
            {
                case 2:
                    state = State.Image;
                    break;
                case 3:
                    state = State.Video;
                    break;
                case 4:
                    state = State.Music;
                    break;
                default:
                    state = State.Eror;
                    break;
            }
            Link = new_Link;
        }

        public State GetState() { return state; }
        
        public TextQuestion GetText()
        {
            if (state == State.Text)
                return new TextQuestion(Text, Answers);
            return null;
        }

        public string getTestText() { return "i run"; }
    }
}
