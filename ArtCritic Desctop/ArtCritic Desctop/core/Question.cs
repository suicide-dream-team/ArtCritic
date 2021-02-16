using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop
{
    class Question//пока что не определили общий вид запросов пусть объект вопроса в игре выглядит так
    {
        protected enum State { None, Text, Image, Video, Eror };
        State state = State.None;
        protected string Text;
        protected string ImageLink;
        protected string VideoLink;

        public Question() { }

        public Question(string text) {
            Text = text;
            state = State.Text;
        }

        public Question(string text, string Link, int newState)
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
                default:
                    state = State.Eror;
                    break;
            }
        }

        public string getTestText() { return "i run"; }
    }
}
