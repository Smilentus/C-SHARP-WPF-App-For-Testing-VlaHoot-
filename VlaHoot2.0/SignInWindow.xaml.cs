using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VlaHoot2._0
{
    /// <summary>
    /// Логика взаимодействия для SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        public SignInWindow()
        {
            InitializeComponent();
        }

        // Проверка корректности заполнения данных
        private string IsCorrectInput()
        {
            string str = "";

            if (string.IsNullOrEmpty(inputNameBox.Text))
                str += "\nВведите Никнейм.";
            if (string.IsNullOrEmpty(inputAgeText.Text))
                str += "\nУкажите возраст.";
            if (maleBtn.IsChecked == false && femaleBtn.IsChecked == false)
                str += "\nВыберите пол.";
            if (difficultyComboBox.SelectedIndex == -1)
                str += "\nВыберите режим.";

            return str;
        }

        private void InputNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputNameBox.Text))
                nameLabel.Content = "Придумайте никнейм: ...";
            else
                nameLabel.Content = "Придумайте никнейм: " + inputNameBox.Text;
        }

        private void InputAgeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputAgeText.Text))
                ageLabel.Content = "Укажите возраст: ...";
            else
                ageLabel.Content = "Укажите возраст: " + inputAgeText.Text;
        }

        private void MaleBtn_Checked(object sender, RoutedEventArgs e)
        {
            if(maleBtn.IsChecked == true)
                genderLabel.Content = "Выберите пол: Мужской";
            else
                genderLabel.Content = "Выберите пол: Женский";
        }

        private void FemaleBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (femaleBtn.IsChecked == true)
                genderLabel.Content = "Выберите пол: Женский";
            else
                genderLabel.Content = "Выберите пол: Мужской";
        }

        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = (Label)difficultyComboBox.Items[difficultyComboBox.SelectedIndex];
            difficultyLbl.Content = "Выберите режим: " + obj.Content;
        }

        private void InputAgeText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string msg = IsCorrectInput();

            if(msg == "")
            {
                Player.Name = inputNameBox.Text;
                if (maleBtn.IsChecked == true)
                    Player.Gender = "Мужской";
                else
                    Player.Gender = "Женский";
                Player.Age = inputAgeText.Text;
                Player.Type = Player.accountType.User;
                
                switch(difficultyComboBox.SelectedIndex)
                {
                    case 0:
                        Player.diff = Player.difficulty.Easy;
                        break;
                    case 1:
                        Player.diff = Player.difficulty.Medium;
                        break;
                    case 2:
                        Player.diff = Player.difficulty.Hard;
                        break;
                    case 3:
                        Player.diff = Player.difficulty.Unikum;
                        break;
                    default:
                        Player.diff = Player.difficulty.Medium;
                        break;
                } 

                Hide();
                ChooseTestWindow window = new ChooseTestWindow();
                window.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Для продолжения необходимо выполнить следующее: " + msg);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }
    }
}
