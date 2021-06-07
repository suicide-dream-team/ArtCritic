using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ArtCritic_Desctop.core.db
{
    /// <summary>
    /// Класс для сущности вопроса.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Возможные типы пакетов и вопросов. Вопросам не следует присваивать тип Mixed.
        /// </summary>
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
        public Question(Pack pack, int type, string text, string answer, string fileName)
        {
            this.Id = 0;
            this.Pack = pack;
            this.Type = (QuestionType)type;
            this.Text = text;
            this.Answer = answer;
            this.FileName = fileName;
        }
        public Question(int id, Pack pack, int type, string text, string answer, string fileName)
        {
            this.Id = id;
            this.Pack = pack;
            this.Type = (QuestionType)type;
            this.Text = text;
            this.Answer = answer;
            this.FileName = fileName;
        }
        public string GetFullPath()
        {
            return String.Format(@"{0}\Packs\{1}\{2}", Directory.GetCurrentDirectory(), Pack.Name, this.FileName);            
        }
    }
}
