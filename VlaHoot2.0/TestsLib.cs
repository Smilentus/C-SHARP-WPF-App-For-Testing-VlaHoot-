using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VlaHoot2._0
{
    public static class TestsLib
    {
        // Хранилище всех вопросов в тесте
        public static Question[] allQuestions;

        // Хранилище ответов на вопросы пользователя
        public static Answer[] answers;

        public static string[] GetAnswers()
        {
            string[] str = new string[allQuestions.Length + 9];

            for(int i = 5; i < str.Length - 5; i++)
            {
                str[i] = "\nВопрос #" + (i - 4) + " | " + answers[i - 5].typeStr 
                    + " | Ответ пользователя: " + answers[i - 5].choosedAnswer 
                    + " | Правильный ответ: " + allQuestions[i - 5].corAnswers[0];
            }

            str[str.Length - 4] = "\n\nИтоговая статистика";
            str[str.Length - 3] = "\nПравильные ответы: " + Player.correctAnswers;
            str[str.Length - 2] = "\nНеправильные ответы: " + Player.unCorrectAnswers;
            str[str.Length - 1] = "\nПропущенные вопросы: " + Player.skippedQuestions;

            return str;
        }
    }
}
