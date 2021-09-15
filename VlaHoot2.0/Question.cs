using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VlaHoot2._0
{
    public class Question
    {
        // Тип вопроса
        public enum Type { SelectableAnswer, SelectableImage, MultySelectableAnswer };
        // Время для ответа
        public TimeSpan timeToAnswer;

        // Сам вопрос
        public string Name;
        // Подсказка вопроса
        public string Tip;
        // Описание, почему именно такой-то вариант правильный
        public string Descr;
        // Текущий тип вопроса 
        public Type type;
        // Правильные ответы
        public string[] corAnswers;
        // Неправильные ответы
        public string[] uncorAnswers;
    }
}
