using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtCritic.Controller
{

    class QuestionsController
    {
        private List<TextQuestion> _Questions;
        private int _currentIndexOfQuestion = 0;
        private int _NumberOfCorrectAnswers = 0;
        public int NumberOfCorrectAnswers
        {
            get
            {
                return _NumberOfCorrectAnswers;
            }
        }

        public void ShuffleQuestionsList()
        {
            Random random = new Random();
            int n = _Questions.Count;
            while (n>1)
            {
                n--;
                int k = random.Next(n + 1);
                TextQuestion question = _Questions[k];
                _Questions[k] = _Questions[n];
                _Questions[n] = question;
            }
        }

        public bool IsTheAnyQuestionsAvailable()
        {
            return _currentIndexOfQuestion != _Questions.Count-1;
        }

        public TextQuestion GetNextQuestion()
        {
            _currentIndexOfQuestion++;
            return _Questions[_currentIndexOfQuestion];
        }

        public void LoadImageQuestions()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ArtCritic.Data.Images.answers.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var e = reader.ReadLine();
                    var args = e.Split('|');
                    _Questions.Add(new ImageQuestion(args[0], args[1]));
                }
            }
        }

        public bool IsCorrectAnswer(string userAnswer)
        {
            return _Questions[_currentIndexOfQuestion].CheckAnswer(userAnswer.ToLower());
        }

        public void LoadVideoQuestions()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ArtCritic.Data.Videos.answersV.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var e = reader.ReadLine();
                    var args = e.Split('|');
                    _Questions.Add(new VideoQuestion(args[0], args[1]));
                }
            }
        }

        public void LoadMusicQuestions()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ArtCritic.Data.Music.answers.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var e = reader.ReadLine();
                    var args = e.Split('|');
                    string[] ans = new string[1];
                    ans[0] = args[1];
                    _Questions.Add(new MusicQuestion("Угадайте название песни", ans, args[0]));
                }
            }
        }
    }
}
