using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic_Desctop.core.db
{
    public class PlayerStat
    {
        public int Id { get; set; }
        public int PlayedGames { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public double CurrentResult { get; set; }

        public PlayerStat()
        {
            PlayedGames = 0;
            TotalQuestions = 0;
            TotalCorrectAnswers = 0;
            CurrentResult = 0.0;
        }
        public PlayerStat(int id, int playedGames, int totalQuestions, int totalCorrectAnswers, double currentResult)
        {
            this.Id = id;
            this.PlayedGames = playedGames;
            this.TotalQuestions = totalQuestions;
            this.TotalCorrectAnswers = totalCorrectAnswers;
            this.CurrentResult = currentResult;
        }
    }
}
