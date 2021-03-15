using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop
{
    [Serializable]
    class QuestionKeeper//пока что не определили общий вид запросов пусть объект вопроса в игре выглядит так
    {
        public enum State { None, Text, Image, Video, Music, Eror };
        public State state = State.None;
        public string Text { get; set; }
        public string ImageLink { get; set; }
        public string VideoLink { get; set; } 
        public string MusicLink { get; set; }
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
                return new ImageQuestion(Text, Answers);
            return null;
        }

        public VideoQuestion GetVideo()
        {
            if (state == State.Video)
                return new VideoQuestion(Text, Answers);
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
