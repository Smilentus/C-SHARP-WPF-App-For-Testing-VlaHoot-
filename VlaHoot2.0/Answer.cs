using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VlaHoot2._0
{
    public class Answer
    {
        public int ansType;
        public string choosedAnswer;

        public string typeStr
        {
            get
            {
                switch(ansType)
                {
                    case 0:
                        return "Ответ: Неправильный";
                    case 1:
                        return "Ответ: Правильный";
                    default:
                        return "Вопрос пропущен";
                }
            }
        }
    }
}
