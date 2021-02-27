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

        public QuestionKeeper() { }

        public QuestionKeeper(string text) {
            Text = text;
            state = State.Text;
        }

        public QuestionKeeper(string text, string Link, int newState)
        {
            Text = text;
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

        public string getTestText() { return "i run"; }
    }
}
