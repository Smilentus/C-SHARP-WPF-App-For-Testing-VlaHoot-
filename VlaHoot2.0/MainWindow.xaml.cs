using Microsoft.Win32;
using System;
using System.Windows;

namespace VlaHoot2._0
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ЗАПОМНИ ТЕМУ: 
        // ДИСКРЕТНАЯ ФОРМА ПРЕДСТАВЛЕНИЯ ЧИСЛОВОЙ, ТЕКСТОВОЙ, 
        // ГРАФИЧЕСКОЙ И ЗВУКОВОЙ ИНФОРМАЦИИ

        private string helpText = "Привет! \nТы наверное задумался что выбирать и что Ты здесь делаешь? " +
            "\nЕсли тебя заставили пройти это тест, просто нажми секретную комбинацию клавиш в главном меню" +
            "\n[ALT + F4]\n " +
            "\n\n[Авторизация]\nНажмите, чтобы создать новый профиль для сохранения результатов с выбранным уровнем сложности." +
            "\n[Проверка]\nНажмите, чтобы выбрать существующий файл с профилем для загрузки и просмотра результатов." +
            "\n[Гость]\nНажмите, чтобы пройти тесты в режиме инкогнито без сохранения результатов со средней сложностью \"Ударник\"." +
            "\n\nРежимы" +
            "\n[Троечник] Подсказки ЕСТЬ. Вопросы не имеют ограничения по времени." +
            "\n[Ударник] Подсказки ЕСТЬ. Некоторые вопросы имеют ограничение по времени." +
            "\n[Отличник] Подсказок НЕТ. Время на ответ ограничено 90 секундами." +
            "\n[Уникум] Подсказок НЕТ. Время на ответ ограничено 30 секундами.";

        private void NewProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрываем текущее окно для пользователя
            Hide();
            // Объявляем новое окно авторизации
            SignInWindow window = new SignInWindow();
            // Открываем окно авторизации
            window.Show();
            // Закрываем текущее окно
            Close();
        }

        private void ExistProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Инициализируем диалоговое окно выбора файла
            OpenFileDialog fd = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Выберите профиль для проверки ...",
            };

            // Если выбранный файл подходит под наши нужды
            // Открываем его и вызываем метод загрузки данных из файла
            // Иначе сообщаем пользователю, что выбранный файл является некорректным
            try
            {
                if (fd.ShowDialog() == true)
                {
                    Player.LoadFromFile(fd.FileName);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при попытке открыть файл!");
            }
        }

        private void GuestProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // Инициализируем генератор случайных чисел
            Random rnd = new Random();
            // Указываем данные о профиле
            Player.Type = Player.accountType.Guest;
            Player.Name = "Анонимус#" + rnd.Next(1, 1001);
            Player.Age = "Неизвестно";
            Player.Gender = "Неизвестно";
            Player.diff = Player.difficulty.Medium;

            // Скрываем текущее окно для пользователя
            Hide();
            // Объявляем новое окно и открываем его
            ChooseTestWindow window = new ChooseTestWindow();
            window.Show();
            // Закрываем текущее окно
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Отображаем вспомогательную информацию
            MessageBox.Show(helpText);
        }
    }
}
