using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop
{
    class QuestionKeeper//пока что не определили общий вид запросов пусть объект вопроса в игре выглядит так
    {
        public enum State { None, Text, Image, Video, Music, Eror };
        protected State state = State.None;
        protected string Text;
        protected string ImageLink;
        protected string VideoLink;
        protected string MusicLink;
        protected string[] Answers;

        public QuestionKeeper() { }

        public QuestionKeeper(string text, string[] answers) {
            Text = text;
            state = State.Text;
            Answers = new string[answers.Length];
            for (int i = 0; i < answers.Length; ++i) {
                Answers[i] = new string(answers[i]);
            }
        }

        public QuestionKeeper(string text, string Link, int newState, string[] answers)
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
                    ImageLink = Link;                    
                    break;
                case 3:
                    state = State.Video;
                    VideoLink = Link;                    
                    break;
                case 4:
                    state = State.Music;
                    MusicLink = Link;
                    break;
                default:
                    state = State.Eror;
                    break;
            }
        }

        public State GetState() { return state; }

        public ImageQuestion GetIm() {
            if (state == State.Image)
                return new ImageQuestion();
            return null;
        }

        public VideoQuestion GetVideo()
        {
            if (state == State.Video)
                return new VideoQuestion();
            return null;
        }

        public TextQuestion GetText()
        {
            if (state == State.Text)
                return new TextQuestion(Text, Answers);
            return null;
        }

        public string getTestText() { return "i run"; }
    }
}
