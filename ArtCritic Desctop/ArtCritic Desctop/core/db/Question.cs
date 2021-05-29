using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop.core.db
{
    class Question
    {
        public enum QuestionType
        {
            Mixed,
            Text,
            Picture,
            Audio,
            Video
        }
        public int Id { get; set; }
        public Pack Pack { get; set; }
        public string Text { get; set; }
        public string FileName { get; set; }
        public string Answer { get; set; }
        public QuestionType Type { get; set; }

        public Question() { }
        public Question(int id, Pack pack, int type, string text, string answer, string fileName)
        {
            this.Id = id;
            this.Pack = pack;
            this.Type = (QuestionType)type;
            this.Text = text;
            this.Answer = answer;
            this.FileName = fileName;
        }
    }
}
