using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ArtCritic.Controller
{

    public class QuestionsController
    {
        private List<TextQuestion> _Questions = new List<TextQuestion>();
        private int _currentIndexOfQuestion = 0;
        private int _numberOfCorrectAnswers = 0;
        public int NumberOfCorrectAnswers
        {
            get
            {
                return _numberOfCorrectAnswers;
            }
        }

        public int CurrentIndexOfQuestion
        {
            get
            {
                return _currentIndexOfQuestion;
            }
        }

        public int GetNumberOfQuestions()
        {
            return _Questions.Count;
        }

        public void ShuffleQuestionsList()
        {
            Random random = new Random();
            int n = _Questions.Count;
            while (n > 1)
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
            return _currentIndexOfQuestion != _Questions.Count - 1;
        }

        public TextQuestion GetCurrentQuestion()
        {
            return _Questions[_currentIndexOfQuestion];
        }

        public TextQuestion GetNextQuestion(string UserAnswer)
        {
            if (_currentIndexOfQuestion != -1 && _Questions[_currentIndexOfQuestion].CheckAnswer(UserAnswer.ToLower()))
            {
                _numberOfCorrectAnswers++;
            }
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
