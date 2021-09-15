using System.Windows;
using System.Windows.Controls;

namespace VlaHoot2._0
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow()
        {
            InitializeComponent();

            SetupScrollPanelHeight();
            ShowResults();
            Player.SaveToFile();
        }

        // - 41

        // Устанавливаем настройки высоты панели для корректного отображения
        private void SetupScrollPanelHeight()
        {
            int height = 240;

            for(int i = 0; i < TestsLib.answers.Length; i++)
            {
                if(TestsLib.answers[i].ansType == 1)
                {
                    height += 130;
                }
                else
                {
                    height += 310;
                }
            }

            resultStackPanel.Height = height;
        }

        // - 42

        // Результаты отображения тестирования
        public void  ShowResults()
        {
            // Отображаем ответы и т.п.
            for(int i = 0; i < TestsLib.answers.Length; i++)
            {
                // Пробуем новую фичу с ресурсами стилей XAML - УСПЕШНО :P

                // Полотно с объектами
                Canvas canvas = new Canvas();

                // Label номера вопроса
                Label numLbl = new Label();
                numLbl.Style = this.Resources["numLbl"] as Style;
                numLbl.Content = "Вопрос #" + (i+1);

                // Label правильности ответа
                Label corLbl = new Label();

                // Label выбранного варианта
                Label chLbl = new Label();
                chLbl.Style = this.Resources["chLbl"] as Style;
                chLbl.Content = "Ваш выбор: " + TestsLib.answers[i].choosedAnswer + 
                    "\nПравильный выбор: " + TestsLib.allQuestions[i].corAnswers[0];
                // Добавляем текст выбранного варианта

                // Описание правильного ответа
                TextBlock tb = new TextBlock();
                if (TestsLib.answers[i].ansType != 1)
                {
                    tb.Style = this.Resources["descrText"] as Style;
                    tb.Text = TestsLib.allQuestions[i].Descr;
                    canvas.Children.Add(tb);
                }

                // Если на ответ был дан правильный вопрос ...
                if (TestsLib.answers[i].ansType == 1)
                {
                    canvas.Style = this.Resources["correctAnswerCanvas"] as Style;
                    corLbl.Style = this.Resources["corLbl"] as Style;
                    corLbl.Content = "Ответ: Правильный";
                }
                else if(TestsLib.answers[i].ansType == 0) // Если на ответ был дан неправильный вопрос ...
                { 
                    canvas.Style = this.Resources["unMissAnswerCanvas"] as Style;
                    corLbl.Style = this.Resources["uncorLbl"] as Style;
                    corLbl.Content = "Ответ: Неправильный";
                }
                else // Если вопрос был пропущен ...
                {
                    canvas.Style = this.Resources["unMissAnswerCanvas"] as Style;
                    corLbl.Style = this.Resources["missLbl"] as Style;
                    corLbl.Content = "Вопрос пропущен";
                }

                // Добавляем оставшиеся элементы на канвас
                canvas.Children.Add(numLbl);
                canvas.Children.Add(corLbl);
                canvas.Children.Add(chLbl);

                resultStackPanel.Children.Add(canvas);
            }

            // - 43

            // Итоговая информация о тестировании
            for(int i = 0; i < 4; i++)
            {
                Label cnv = new Label();
                cnv.Style = this.Resources["totalResultLbl"] as Style;

                if (i == 0)
                    cnv.Content = "Итоговая статистика";
                if (i == 1)
                    cnv.Content = "Всего правильных ответов: " + Player.correctAnswers;
                if (i == 2)
                    cnv.Content = "Всего неправильных ответов: " + Player.unCorrectAnswers;
                if (i == 3)
                    cnv.Content = "Всего пропущенных ответов: " + Player.skippedQuestions;

                resultStackPanel.Children.Add(cnv);
            }
        }

        // - 44
    }
}
