using System;
using System.Collections.Generic;
using System.Text;

namespace ArtCritic
{
    /// <summary>
    /// Базовый класс вопроса: текстовый вопрос
    /// </summary>
    public class TextQuestion
    {
        private readonly string _text;// текст вопроса
        protected string[] _correctAnswers;// !FIXME публично временно, на время тестов

        public TextQuestion(string text, string[] answers)
        {
            _text = text;
            _correctAnswers = answers;
        }

        public TextQuestion(string text, string answer)
        {
            _correctAnswers = new string[1];
            _correctAnswers[0] = answer;
            _text = text;
        }

        /// <summary>
        /// Проверка ответа на правильность
        /// Ищет ответ в массиве всех ответов
        /// </summary>
        /// <param name="answer">ответ, который нужно проверить </param>
        /// <returns>Верность ответа</returns>
        public bool CheckAnswer(string answerToCheck)
        {
            foreach (string correctAnswer in _correctAnswers)
            {
                if (correctAnswer.ToLower() == answerToCheck.ToLower())
                {
                    return true;
                }

            }
            return false;
        }
    }
}
