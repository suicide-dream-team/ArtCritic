using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop.core.db
{
    class Pack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Question.QuestionType Type { get; set; }

        public Pack() { }
        public Pack(int Id, string Name, string Path, Question.QuestionType Type)
        {
            this.Id = Id;
            this.Name = Name;
            this.Path = Path;
            this.Type = Type;
        }
        public Pack(int id, string name, string path, int type) : this(id, name, path, (Question.QuestionType)type)
        {}
    }
}
