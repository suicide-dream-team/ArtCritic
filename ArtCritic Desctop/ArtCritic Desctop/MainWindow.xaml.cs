using System.Windows;
/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;*/

namespace ArtCritic_Desctop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private QuestionKeeper question;
        private TextQuestion textQuestion;
        public MainWindow()
        {
            InitializeComponent();
            string[] test_answers = new string[2];
            test_answers[0] = "Спанч Боб";
            test_answers[1] = "Спанч Боб Скрепенс";
            question = new QuestionKeeper("кто проживает на дне океана?", test_answers);
            this.Type_of_game.Visibility = Visibility.Hidden;
            this.Test_game_with_Image.Visibility = Visibility.Hidden;
        }        

        private void Settings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Satistyc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            Main_menu.Visibility = Visibility.Hidden;
            Type_of_game.Visibility = Visibility.Visible;
        }        

        private void Video_question_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Music_question_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Mixed_questions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_question_Click(object sender, RoutedEventArgs e)
        {

        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            Type_of_game.Visibility = Visibility.Hidden;
            Test_game_with_Image.Visibility = Visibility.Visible;
            textQuestion = question.GetText();
            Text.Text = textQuestion.Text;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0;i<textQuestion.Answers.Length;++i)
            if (Answer.Text == textQuestion.Answers[i])//пока не работает, разбираюсь
            {
                    Accept.Content = "Верно";
            }
                else { Accept.Content = "Неверно," + textQuestion.Answers[0]+textQuestion.Answers[1]; }
        }
    }
}
