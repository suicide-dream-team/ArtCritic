using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop.core
{

    class PlayerStat
    {
        private int Id { get; set; }
        private int PlayedGames { get; set; }
        private int TotalQuestions { get; set; }
        private int TotalCorrectAnswers { get; set; }
        private double CurrentResult { get; set; }

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
