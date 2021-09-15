using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VlaHoot2._0
{
    public static class Player
    {
        private static Random rnd = new Random();
        // Кодировка для файлов
        private static Encoding encoding = Encoding.UTF8;

        public enum accountType
        {
            Guest,
            User,
            Admin
        }

        public enum difficulty
        {
            Easy,
            Medium,
            Hard,
            Unikum,
        }

        // Данные об игроке
        public static accountType Type;
        public static string Name;
        public static string Gender;
        public static string Age;
        public static string fileName;
        public static difficulty diff;

        // Данные об ответах
        public static int correctAnswers = 0;
        public static int unCorrectAnswers = 0;
        public static int skippedQuestions = 0;

        // - 13

        // Строковое представление типа профиля
        public static string typeStr
        {
            get
            {
                switch (Type)
                {
                    case accountType.Guest:
                        return "Гость";
                    case accountType.Admin:
                        return "Администратор";
                    case accountType.User:
                        return "Пользователь";
                    default:
                        return "Неизвестно";
                }
            }
        }

        // - 14
    
        public static string diffStr
        {
            get
            {
                switch (diff)
                {
                    case difficulty.Easy:
                        return "Троечник";
                    case difficulty.Medium:
                        return "Ударник";
                    case difficulty.Hard:
                        return "Отличник";
                    case difficulty.Unikum:
                        return "Уникум";
                    default:
                        return "Ударник";
                }
            }
        }

        // - 15

        private static string randomName()
        {
            string engAl = "qwertyuiopasdfghjklzxcvbnm";
            string str = "";

            for(int i = 0; i < 9; i++)
            {
                str += engAl[rnd.Next(0, engAl.Length)];
            }

            fileName = str;

            return str;
        }

        // - 16

        public static void SaveToFile()
        {
            // Если папки с профилями не существует, то создаём её
            if(!Directory.Exists("profiles"))
            {
                Directory.CreateDirectory("profiles");
            }

            string[] str = TestsLib.GetAnswers();
            str[0] = "Тип профиля: " + typeStr;
            str[1] = "Режим: " + diffStr;
            str[2] = "Пользователь: " + Name;
            str[3] = "Возраст: " + Age;
            str[4] = "Пол: " + Gender;

            string date = DateTime.Now.ToShortDateString().Replace("/", ".");
            string time = DateTime.Now.ToShortTimeString().Replace(":", ".");

            // Записываем все данные в файл
            if (Type == accountType.Guest)
                File.WriteAllLines("profiles/" + randomName() + "_" + date + "_" + time + ".txt", str);
            else
                File.WriteAllLines("profiles/" + typeStr + "_" + date + "_" + time + ".txt", str);
        }

        // - 17

        public static void LoadFromFile(string path)
        {
            // Если файл по пути существует, то выводим информацию
            if(File.Exists(path))
            {
                string[] data = File.ReadAllLines(path, encoding);
                string str = "";
                foreach (var obj in data)
                    str += "\n" + obj;
                MessageBox.Show("Данные загружены! " + str);
            }
            else
            {
                MessageBox.Show("Не удалось загрузить данные!");
            }
        }

        // - 18
    }
}
