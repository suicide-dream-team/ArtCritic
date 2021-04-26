using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop.core
{

    class PlayerStat
    {
        public int Id { get; set; }
        public int PlayedGames { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public double CurrentResult { get; set; }

        public PlayerStat() { }
        public PlayerStat(int Id, int PlayedGames, int TotalQuestions, int TotalCorrectAnswers, double CurrentResult)
        {
            this.Id = Id;
            this.PlayedGames = PlayedGames;
            this.TotalQuestions = TotalQuestions;
            this.TotalCorrectAnswers = TotalCorrectAnswers;
            this.CurrentResult = CurrentResult;
        }
    }
}
