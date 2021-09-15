using System;                                                                                                             
using System.Linq;                                                                                                         
using System.Windows;                                                          
using System.Windows.Controls;                                                                                                    
using System.Windows.Media;                                                                                                    
                                                                               
namespace VlaHoot2._0                                                          
{                                                                              
    /// <summary>                                                              
    /// Логика взаимодействия для ChooseTestWindow.xaml                        
    /// </summary>                                                             
    public partial class ChooseTestWindow : Window                             
    {   
        // Умные таймеры                                                       
        System.Windows.Threading.DispatcherTimer timer;                        
        System.Windows.Threading.DispatcherTimer timerToStart;                 
        System.Windows.Threading.DispatcherTimer blinkTimer;                 
                                                                               
        private Random rnd = new Random();                                     
                                                                               
        // Некоторые переменные                                                
        private int curQuestion = 0;
        // Время между вопросами
        private int defaultTimeBtwAnswers = 5;
        private int timeToStart = 5;
        // Время на вопрос
        private TimeSpan timeToAns;                                            
        private int ansPos = 0;
        // Блинкующий label следующего вопроса
        private Label blink;
        // Настройки блика
        private int color = 15;
        private int curColor = 250;
        private int minColor = 100;
        private int maxColor = 250;

        // - 21

        private string[] phrases = {
            "Соберитесь с мыслями!",                                          
            "Утренняя зарядка поддерживает тонус тела в течение всего дня!",  
            "Посмотри на время!",
            "Half-Life 3 не выйдет!",
            "Позитивные мысли способствуют улучшению мозговой активности!",
            "Сделай шаг, и дорога появится сама собой!",
            "Лучше маленькое дело, чем большой план без действия!",
            "В каждом из нас спит гений. И с каждым днем все крепче и крепче…",
            "В мире нет вечных двигателей, зато полно вечных тормозов!",
            "Вы, как и все, имеете право на ошибку!",
            "Не переусердствуйте, распределяйте рабочее время равномерно с перерывами!",
            "Мечты должны быть либо безумными, либо нереальными… Иначе - это просто планы!",
            "Конец - лишь часть пути!",
            "Неудача - это путь к победе!",
        };

        public ChooseTestWindow()
        {
            InitializeComponent();
            // Записываем данные о человеке
            profileLabel.Content = 
                "Профиль: " + Player.typeStr +
                " | Режим: " + Player.diffStr +
                " | Никнейм: " + Player.Name;
            // Инициализируем таймеры
            InitTimers();
            // Инициализируем вопросы
            InitQuestions();
            // Спавним объекты вопросов
            SpawnQuestions();
            // Начинаем тестирование
            StartTest();
        }

        // - 22

        private void InitTimers()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timerTick);

            timerToStart = new System.Windows.Threading.DispatcherTimer();
            timerToStart.Interval = new TimeSpan(0, 0, 1);
            timerToStart.Tick += new EventHandler(timerToStartTick);

            blinkTimer = new System.Windows.Threading.DispatcherTimer();
            blinkTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            blinkTimer.Tick += new EventHandler(blinkTimerTick);
        }

        // - 23

        private void InitTimerToAnswer(TimeSpan time)
        {
            timeToAns = time;
            timeToAnswer.Content = "Осталось: " + timeToAns.ToString();
            timer.Start();
        }

        // - 24

        private void timerTick(object sender, EventArgs e)
        {
            if (timeToAns.TotalSeconds <= 0)
            {
                timer.Stop();
                Player.skippedQuestions++;
                HidePanel();
            }
            else
            {
                timeToAns = timeToAns.Subtract(new TimeSpan(0, 0, 1));
            }

            timeToAnswer.Content = "Осталось: " + timeToAns.ToString();
        }

        // - 25

        private void timerToStartTick(object sender, EventArgs e)
        {
            if(timeToStart > 1)
            {
                timeToStart--;
                countdownLabel.Content = timeToStart + "...";
            }
            else
            {
                timerToStart.Stop();
                questionsPanel.Visibility = Visibility.Visible;
                readyCanvas.Visibility = Visibility.Hidden;
                NextQuestion();
            }
        }

        // - 26

        private void blinkTimerTick(object sender, EventArgs e)
        {
            if (curColor >= maxColor)
                color = -15;
            if (curColor <= minColor)
                color = 15;

            curColor += color;

            blink.Background = new SolidColorBrush(Color.FromRgb((byte)curColor, (byte)curColor, 0));
        }

        // - 27

        // ДЕБУХ функция для получения рандомной строки
        private string rndName()
        {
            string str = "qwertyuiopasdfghjklzxcvbnmйцукенгшщзфывапролдячсмитьбюэхъ";
            string s = "";

            for(int i = 0; i < 20; i++)
            {
                s += str[rnd.Next(0, str.Length)];
            }

            return s;
        }
        // ДЕБУХ фукнция для рандомизации вопросов
        private void RndQuest()
        {
            for(int i = 0; i < 25; i++)
            {
                TestsLib.allQuestions[i].Name = rndName();
                TestsLib.allQuestions[i].Descr = rndName();
                TestsLib.allQuestions[i].corAnswers = new string[] { rndName() };
                TestsLib.allQuestions[i].uncorAnswers = new string[] { rndName() };
                TestsLib.allQuestions[i].Tip = rndName();
            }
        }
        
        // Инициализация вопросов
        private void InitQuestions()
        {
            // Инициализируем и объявляем вопросы
            TestsLib.allQuestions = new Question[24];

            // Перевод из 2-й в 10-ю
            TestsLib.allQuestions[0] = new Question()
            {
                Name = "Переведите двоичное число 0110 1101 в десятичную систему счисления.",
                corAnswers = new string[] { "109" },
                uncorAnswers = new string[] { "111", "110", "112" },
                Descr = "0110 1101 (2) = 1 * 2^6 + 1 * 2^5 + 1 * 2^3 + 1 * 2^2 + 1 * 2^0 = 64 + 32 + 16 + 4 + 1 = 109 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[1] = new Question()
            {
                Name = "Переведите двоичное число 0111 0001 в десятичную систему счисления.",
                corAnswers = new string[] { "113" },
                uncorAnswers = new string[] { "105", "114", "117" },
                Descr = "0111 0001 (2) = 1 * 2^6 + 1 * 2^5 + 1 * 2^4 + 1 * 2^0 = 64 + 32 + 16 + 1 = 113 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[2] = new Question()
            {
                Name = "Переведите двоичное число 0110 0111 в десятичную систему счисления.",
                corAnswers = new string[] { "103" },
                uncorAnswers = new string[] { "113", "114", "112" },
                Descr = "0110 0111 (2) = 1 * 2^6 + 1 * 2^5 + 1 * 2^2 + 1 * 2^1 + 1 =  64 + 32 + 4 + 2 + 1 = 103 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[3] = new Question()
            {
                Name = "Переведите двоичное число 0110 1001 в десятичную систему счисления.",
                corAnswers = new string[] { "105" },
                uncorAnswers = new string[] { "106", "100", "101" },
                Descr = "0110 1001 (2) = 1 * 2^6 + 1 * 2^5 + 1 * 2^3 + 1 * 2^0 =  64 + 32 + 8 + 1 =  105 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[4] = new Question()
            {
                Name = "Переведите двоичное число 0111 0011 в десятичную систему счисления.",
                corAnswers = new string[] { "115" },
                uncorAnswers = new string[] { "116", "117", "118" },
                Descr = "0111 0011 (2) = 1 * 2^6 + 1 * 2^5 + 1 * 2^4 + 1 * 2^1 + 1 * 2^0 =  64 + 32 + 16 + 2 + 1 =  115 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[5] = new Question()
            {
                Name = "Переведите двоичное число 0110 0011 в десятичную систему счисления.",
                corAnswers = new string[] { "99" },
                uncorAnswers = new string[] { "87", "47", "97" },
                Descr = "0110 0011 (2) = 1 * 2^6 + 1 * 2^5 + 1 * 2^1 + 1 * 2^0 = 64 + 32 + 2 + 1 =  99 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };

            // Переведите из 10-й в 2-ю
            TestsLib.allQuestions[6] = new Question()
            {
                Name = "Переведите число 100 из десятичной системы счисления в ДВОИЧНУЮ систему счисления. Сколько ЕДИНИЦ содержит полученное число? В ответе укажите одно число — количество единиц.",
                corAnswers = new string[] { "3" },
                uncorAnswers = new string[] { "4", "5", "100" },
                Descr = "Представим число 100 в виде суммы степеней двойки: 100 = 64 + 32 + 4. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 100 0000; 32 = 10 0000; 4 = 100. Следовательно, 100 (10) = 0110 0100 (2).",
                Tip = "Представим число 111 в виде суммы степеней двойки: 111 = 64 + 32 + 8 + 4 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 0100 0000; 32 = 0010 0000; 8 = 1000; 4 = 0100, 2 = 0010, 1 = 0001. Следовательно, 111 (10) = 0110 1111 (2). 6 единиц.",
            };
            TestsLib.allQuestions[7] = new Question()
            {
                Name = "Переведите число 97 из десятичной системы счисления в ДВОИЧНУЮ систему счисления. Сколько ЕДИНИЦ содержит полученное число? В ответе укажите одно число — количество единиц.",
                corAnswers = new string[] { "3" },
                uncorAnswers = new string[] { "2", "110 0100", "110 0110" },
                Descr = "Представим число 97 в виде суммы степеней двойки: 97 = 64 + 32 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 100 0000; 32 = 10 0000; 1 = 1. Следовательно, 97 (10) = 110 0001 (2).",
                Tip = "Представим число 111 в виде суммы степеней двойки: 111 = 64 + 32 + 8 + 4 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 0100 0000; 32 = 0010 0000; 8 = 1000; 4 = 0100, 2 = 0010, 1 = 0001. Следовательно, 111 (10) = 0110 1111 (2). 6 единиц.",
            };
            TestsLib.allQuestions[8] = new Question()
            {
                Name = "Переведите число 132 из десятичной системы счисления в ДВОИЧНУЮ систему счисления. Сколько ЕДИНИЦ содержит полученное число? В ответе укажите одно число — количество единиц.",
                corAnswers = new string[] { "2" },
                uncorAnswers = new string[] { "0", "1", "3" },
                Descr = "Представим число 132 в виде суммы степеней двойки: 100 = 128 + 4. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 128 = 1000 0000; 4 = 100. Следовательно, 132 (10) = 1000 0100 (2).",
                Tip = "Представим число 111 в виде суммы степеней двойки: 111 = 64 + 32 + 8 + 4 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 0100 0000; 32 = 0010 0000; 8 = 1000; 4 = 0100, 2 = 0010, 1 = 0001. Следовательно, 111 (10) = 0110 1111 (2). 6 единиц.",
            };
            TestsLib.allQuestions[9] = new Question()
            {
                Name = "Переведите число А2 из шестнадцатеричной системы счисления в ДЕСЯТИЧНУЮ систему счисления.",
                corAnswers = new string[] { "162" },
                uncorAnswers = new string[] { "254", "17", "1010 1011" },
                Descr = "Представим число A2 в виде суммы степеней числа шестнадцать с соответствующими множителями: A2 (16) = 10 * 16 + 2 * 1 = 162 (10).",
                Tip = "Переведём число FE в 10-ую CC: FE (16) = 254 (10). Представим 254 в виде суммы степеней двойки: 254 = 128 + 64 + 32 + 16 + 8 + 4 + 2. Теперь переведём каждое из слагаемых в 2-ую СС и сложим результаты: 128 = 1000 0000; 64 = 100 0000; 32 = 100 000; 16 = 10 000; 8 = 1 000; 4 = 100; 2 = 10. Следовательно, 254 (10) = 1111 1110 (2)."
            };
            TestsLib.allQuestions[10] = new Question()
            {
                Name = "Переведите число 1010 1001 из двоичной системы счисления в ДЕСЯТИЧНУЮ систему счисления.",
                corAnswers = new string[] { "169" },
                uncorAnswers = new string[] { "168", "170", "180" },
                Descr = "10101001 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^3 + 1 * 2^0 = 128 + 32 + 8 + 1 = 169 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)"
            };
            TestsLib.allQuestions[11] = new Question()
            {
                Name = "Переведите число 0110 1011 из двоичной системы счисления в ДЕСЯТИЧНУЮ систему счисления.",
                corAnswers = new string[] { "" },
                uncorAnswers = new string[] { "", "", "" },
                Descr = "1101011 (2) = 1 * 2^6 + 1 * 2^5 + 1 * 2^3 + 1 * 2^1 + 1 * 2^0 = 64 + 32 + 8 + 2 + 1 = 107 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)"
            };
            TestsLib.allQuestions[12] = new Question()
            {
                Name = "Переведите число 156 из десятичной системы счисления в ДВОИЧНУЮ систему счисления. Сколько единиц содержит полученное число?",
                corAnswers = new string[] { "4" },
                uncorAnswers = new string[] { "6", "7", "5" },
                Descr = "Представим число 156 в виде суммы степеней двойки: 156 = 128 + 16 + 8 + 4. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 128 = 1000 0000, 16 = 0001 0000, 8 = 1000, 4 = 00100. Следовательно, 156 (10) = 1001 1100 (2).",
                Tip = "Представим число 111 в виде суммы степеней двойки: 111 = 64 + 32 + 8 + 4 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 0100 0000; 32 = 0010 0000; 8 = 1000; 4 = 0100, 2 = 0010, 1 = 0001. Следовательно, 111 (10) = 0110 1111 (2). 6 единиц.",
            };
            TestsLib.allQuestions[13] = new Question()
            {
                Name = "Переведите число 147 из десятичной системы счисления в ДВОИНЧУЮ систему счисления. Сколько ЕДИНИЦ содержит полученное число?",
                corAnswers = new string[] { "4" },
                uncorAnswers = new string[] { "6", "7", "5" },
                Descr = "Представим число 147 в виде суммы степеней двойки: 147 = 128 + 16 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 128 = 1000 0000, 16 = 0001 0000, 2 = 0010, 1 = 0001. Следовательно, 147 (10) = 1001 0011 (2).",
                Tip = "Представим число 111 в виде суммы степеней двойки: 111 = 64 + 32 + 8 + 4 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 0100 0000; 32 = 0010 0000; 8 = 1000; 4 = 0100, 2 = 0010, 1 = 0001. Следовательно, 111 (10) = 0110 1111 (2). 6 единиц.",
            };
            TestsLib.allQuestions[14] = new Question()
            {
                Name = "Переведите число 245 из десятичной системы счисления в ДВОИЧНУЮ систему счисления. Сколько ЕДИНИЦ содержит полученное число?",
                corAnswers = new string[] { "6" },
                uncorAnswers = new string[] { "7", "4", "5" },
                Descr = "Представим число 245 в виде суммы степеней двойки: 245 = 128 + 64 + 32 + 16 + 4 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 128 = 1000 0000, 64 = 0100 0000, 32 = 0010 0000, 16 = 0001 0000, 4 = 0100, 1 = 0001. Следовательно, 245 (10) = 1111 0101 (2).",
                Tip = "Представим число 111 в виде суммы степеней двойки: 111 = 64 + 32 + 8 + 4 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 0100 0000; 32 = 0010 0000; 8 = 1000; 4 = 0100, 2 = 0010, 1 = 0001. Следовательно, 111 (10) = 0110 1111 (2). 6 единиц.",
            };
            TestsLib.allQuestions[15] = new Question()
            {
                Name = "Переведите число 143 из десятичной системы счисления в ДВОИЧНУЮ систему счисления. Сколько ЗНАЧАЩИХ НУЛЕЙ содержит полученное число?",
                corAnswers = new string[] { "3" },
                uncorAnswers = new string[] { "2", "4", "5" },
                Descr = "Представим число 143 в виде суммы степеней двойки: 143 = 128 + 8 + 4 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 128 = 1000 0000, 8 = 1000, 4 = 0100, 2 = 0010, 1 = 0001. Следовательно, 143 (10) = 10001111 (2). В записи данного числа 3 нуля.",
                Tip = "Представим число 111 в виде суммы степеней двойки: 111 = 64 + 32 + 8 + 4 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 0100 0000; 32 = 0010 0000; 8 = 1000; 4 = 0100, 2 = 0010, 1 = 0001. Следовательно, 111 (10) = 0110 1111 (2). 6 единиц.",
            };
            TestsLib.allQuestions[16] = new Question()
            {
                Name = "Переведите число 305 из десятичной системы счисления в ДВОИЧНУЮ систему счисления. Сколько ЕДИНИЦ содержит полученное число?",
                corAnswers = new string[] { "4" },
                uncorAnswers = new string[] { "3", "5", "6" },
                Descr = "Представим число 305 в виде суммы степеней двойки: 305 = 256 + 32 + 16 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 256 = 0001 0000 0000, 32 = 0010 0000, 16 = 0001 0000 1 = 0001. Следовательно, 305 (10) = 1001 10001 (2).",
                Tip = "Представим число 111 в виде суммы степеней двойки: 111 = 64 + 32 + 8 + 4 + 2 + 1. Теперь переведём каждое из слагаемых в двоичную систему счисления и сложим результаты: 64 = 0100 0000; 32 = 0010 0000; 8 = 1000; 4 = 0100, 2 = 0010, 1 = 0001. Следовательно, 111 (10) = 0110 1111 (2). 6 единиц.",
            };
            TestsLib.allQuestions[17] = new Question()
            {
                Name = "Переведите число 0010 1110 из двоичной системы счисления в ДЕСЯТИЧНУЮ систему счисления.",
                corAnswers = new string[] { "46" },
                uncorAnswers = new string[] { "47", "48", "45" },
                Descr = "Представим число 0010 1110 в виде суммы степеней двойки: 0010 1110 (2) = 1 * 2^5 + 1 * 2^3 + 1 * 2^2 + 1 * 2^1 = 32 + 8 + 4 + 2 = 46 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[18] = new Question()
            {
                Name = "Переведите число 0101 1101 из двоичной системы счисления в ДЕСЯТИЧНУЮ систему счисления.",
                corAnswers = new string[] { "93" },
                uncorAnswers = new string[] { "97", "45", "117" },
                Descr = "Представим число 0101 1101 в виде суммы степеней двойки: 0101 1101 (2) = 1 * 2^6 + 1 * 2^4 + 1 * 2^3 + 1 * 2^2 + 1 * 2^0 = 64 + 16 + 8 + 4 + 1 = 93 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[19] = new Question()
            {
                Name = "Переведите число 0010 1010 из двоичной системы счисления в ДЕСЯТИЧНУЮ систему счисления.",
                corAnswers = new string[] { "42" },
                uncorAnswers = new string[] { "47", "45", "24" },
                Descr = "Представим число 0010 1010 в виде суммы степеней двойки: 0010 1010 (2) = 1 * 2^5 + 1 * 2^3 + 1 * 2^1 = 32 + 8 + 2 = 42 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[20] = new Question()
            {
                Name = "Переведите число 0011 0101 из двоичной системы счисления в ДЕСЯТИЧНУЮ систему счисления. ",
                corAnswers = new string[] { "" },
                uncorAnswers = new string[] { "", "", "" },
                Descr = "Представим число 0011 0101 в виде суммы степеней двойки: 0011 0101 (2) = 1 * 2^5 + 1 * 2^4 + 1 * 2^2 + 1 = 32 + 16 + 4 + 1 = 53 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[21] = new Question()
            {
                Name = "Переведите двоичное число 0111 0110 в ДЕСЯТИЧНУЮ систему счисления.",
                corAnswers = new string[] { "118" },
                uncorAnswers = new string[] { "48", "136", "140" },
                Descr = "0111 0110 (2) = 1 * 2^6 + 1 * 2^5 + 1 * 2^4 + 1 * 2^2 + 1 * 2^1 =  64 + 32 + 16 + 4 + 2 = 118 (10)",
                Tip = "1010 0101 (2) = 1 * 2^7 + 1 * 2^5 + 1 * 2^2 + 1 * 2^0 = 165 (10)",
            };
            TestsLib.allQuestions[22] = new Question()
            {
                Name = "Переведите число 189 из десятичной в ДВОИЧНУЮ систему счисления.",
                corAnswers = new string[] { "1011 1101" },
                uncorAnswers = new string[] { "1011 1011", "117", "1000 1001" },
                Descr = "189 (10) =  128 + 32 + 16 + 8 + 4 + 1 = 1 * 2^7 + 1 * 2^5 + 1 * 2^4 + 1 * 2^3 + 1 * 2^2 + 1 * 2^0 = 1011 1101 (2)",
                Tip = "",
            };

            // Инициализируем массив правильных ответов
            TestsLib.answers = new Answer[TestsLib.allQuestions.Length];

            SetDifficulty();
        }

        // - 28

        // Устанавливаем сложность вопросов в зависимости от выбранной сложности
        private void SetDifficulty()
        {
            switch(Player.diff)
            {
                case Player.difficulty.Easy:
                    for(int i = 0; i < TestsLib.allQuestions.Length; i++)
                    {
                        TestsLib.allQuestions[i].timeToAnswer = new TimeSpan(0, 0, 0);
                    }
                    break;
                case Player.difficulty.Medium:
                    for (int i = 0; i < TestsLib.allQuestions.Length; i++)
                    {
                        if (i % 3 != 0)
                            TestsLib.allQuestions[i].Tip = "";
                    }
                    break;
                case Player.difficulty.Hard:
                    for (int i = 0; i < TestsLib.allQuestions.Length; i++)
                    {
                        TestsLib.allQuestions[i].timeToAnswer = new TimeSpan(0, 1, 30);
                        TestsLib.allQuestions[i].Tip = "";
                    }
                    break;
                case Player.difficulty.Unikum:
                    for (int i = 0; i < TestsLib.allQuestions.Length; i++)
                    {
                        TestsLib.allQuestions[i].timeToAnswer = new TimeSpan(0, 0, 30);
                        TestsLib.allQuestions[i].Tip = "";
                    }
                    break;
            }
        }

        // - 29

        public void SpawnQuestions()
        {
            for (int i = 1; i < TestsLib.allQuestions.Length; i++)
            {
                if (TestsLib.allQuestions[i] != null)
                {
                    Label lbl = new Label()
                    {
                        Content = "Вопрос №" + (i + 1),
                        Background = new SolidColorBrush(Color.FromRgb(95, 95, 95)),
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        Height = 50,
                        FontSize = 25,
                        Margin = new Thickness(1, 1, 1, 1),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                    };

                    questionsPanel.Children.Add(lbl);
                }
            }
        }

        // - 30

        public void SpawnAnswers(int question)
        {
            // Номер вопроса
            questionNameText.Text = TestsLib.allQuestions[question].Name;
            questionNumberText.Content = "Вопрос №" + (question + 1);

            // Время на ответ, если оно есть
            if (TestsLib.allQuestions[question].timeToAnswer.TotalSeconds > 0)
            {
                InitTimerToAnswer(TestsLib.allQuestions[question].timeToAnswer);
                timeToAnswer.Visibility = Visibility.Visible;
            }
            else
            {
                timeToAnswer.Visibility = Visibility.Hidden;
            }

            int len = TestsLib.allQuestions[question].corAnswers.Length + TestsLib.allQuestions[question].uncorAnswers.Length;
            int c = 0, u = 0;

            // Удаление кнопок
            if (answerStackPanel.Children.Count > 0)
            {
                answerStackPanel.Children.Clear();
            }

            // Изменение размера под кол-во кнопок
            answerStackPanel.Height = 40 * (TestsLib.allQuestions[question].corAnswers.Length + TestsLib.allQuestions[question].uncorAnswers.Length);

            // TODO: Нормальный разброс вариантов ответа
            // Сгенерированные кнопки
            Button[] btns = new Button[len];

            // - 31

            // Спавн кнопок
            for (int i = 0; i < len; i++)
            {
                Button btn = new Button()
                {
                    Background = new SolidColorBrush(Color.FromRgb(226, 226, 226)),
                    Height = 40,
                };

                if (c < TestsLib.allQuestions[question].corAnswers.Length)
                {
                    btn.Content = TestsLib.allQuestions[question].corAnswers[c];
                    c++;
                    btn.Click += CorrectAnswer;
                }
                else
                {
                    btn.Content = TestsLib.allQuestions[question].uncorAnswers[u];
                    u++;
                    btn.Click += UnCorrectAnswer;
                }

                btns[i] = btn;
            }

            // Перемешиваем числа в случайном порядке
            int[] perm = Enumerable.Range(0, len).ToArray();
            for (int i = len - 1; i >= 1; i--)
            {
                int j = rnd.Next(i + 1);
                int temp = perm[j];
                perm[j] = perm[i];
                perm[i] = temp;
            }

            // Добавляем кнопки в соответствии с замешиванием
            for (int i = 0; i < btns.Length; i++)
            {
                answerStackPanel.Children.Add(btns[perm[i]]);
            }
        }

        // - 32

        private void ShowPanel()
        {
            answerPanel1.Visibility = Visibility.Visible;
            blinkTimer.Stop();
        }

        // - 33

        private void StartTimerBtwQuestions()
        {
            // Небольшая пауза между вопросами
            timeToStart = defaultTimeBtwAnswers;
            countdownLabel.Content = timeToStart + "...";
            timerToStart.Start();
            readyCanvas.Visibility = Visibility.Visible;

            // Показываем случайную фразу
            randomFactLabel.Content = phrases[rnd.Next(0, phrases.Length)];

            // Добавляем мелькающий бокс выделения следующего вопроса
            blink = new Label()
            {
                Content = "Вопрос №" + (curQuestion + 1),
                Background = new SolidColorBrush(Color.FromRgb(250, 250, 0)),
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Height = 50,
                FontSize = 25,
                Margin = new Thickness(1, 1, 1, 1),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            questionsPanel.Children.Insert(0, blink);
            blinkTimer.Start();
        }

        // - 34

        private void HidePanel()
        {
            // Убираем предыдущий вопрос
            answerPanel1.Visibility = Visibility.Hidden;
            if (questionsPanel.Children.Count > 1)
            {
                questionsPanel.Children.RemoveAt(0);
                questionsPanel.Children.RemoveAt(0);
            }

            // Останавливаем таймер ответа на вопрос
            timer.Stop();

            if (curQuestion < TestsLib.allQuestions.Length)
            {
                StartTimerBtwQuestions();
            }
            else
            {
                Hide();
                ResultWindow window = new ResultWindow();
                window.Show();
                Close();
            }
        }

        // - 35

        private string GetBtnName(object obj)
        {
            if(obj is Button btn)
            {
                return btn.Content.ToString();
            }
            else
            {
                return "...";
            }
        }

        // - --

        public void AddAnswer(int type, string ans)
        {
            TestsLib.answers[ansPos] = new Answer() { ansType = type, choosedAnswer = ans };
            ansPos++;
        }

        // - 36

        private void CorrectAnswer(object sender, RoutedEventArgs e)
        {
            Player.correctAnswers++;
            AddAnswer(1, GetBtnName(sender));
            HidePanel();
        }

        private void UnCorrectAnswer(object sender, RoutedEventArgs e)
        {
            Player.unCorrectAnswers++;
            AddAnswer(0, GetBtnName(sender));
            HidePanel();
        }

        private void SkipQuestion()
        {
            Player.skippedQuestions++;
            AddAnswer(2, "...");
            HidePanel();
        }

        // - 37

        private void ShowTip()
        {
            MessageBox.Show("Подсказка: " + TestsLib.allQuestions[curQuestion - 1].Tip);
        }

        // - 38

        public void NextQuestion()
        {
            if (string.IsNullOrEmpty(TestsLib.allQuestions[curQuestion].Tip))
            {
                tipBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                tipBtn.Visibility = Visibility.Visible;
            }

            // Генерация вопросов
            SpawnAnswers(curQuestion);

            // Открываем панель
            ShowPanel();

            curQuestion++;
        }

        // - 39

        private void skipBtn_Click(object sender, RoutedEventArgs e)
        {
            SkipQuestion();
        }

        private void tipBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowTip();
        }

        private void StartTest()
        {
            StartTimerBtwQuestions();
        }

        // - 40
    }
}
