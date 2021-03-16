using System.Windows;
using System;
using System.IO;
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
using System.Windows.Shapes;

namespace ArtCritic_Desctop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player Player = new Player();
        private int iter = 0;//пока я не сделаю нормальную обертку итерируем вопросы этой штукой
        Music_question[] music_Questions = new Music_question[4];
        Uri[] uris = new Uri[4];
        private QuestionKeeper question;
        private TextQuestion textQuestion;
        MediaPlayer[] mediaPlayer = new MediaPlayer[4];
        public MainWindow()
        {


            InitializeComponent();
            Player.statistic = 0; 
            string[] test_answers = new string[2];
            test_answers[0] = "Спанч Боб";
            test_answers[1] = "Спанч Боб Скрепенс";
            question = new QuestionKeeper("кто проживает на дне океана?", test_answers);
            this.Type_of_game.Visibility = Visibility.Hidden;
            this.Test_game_with_Image.Visibility = Visibility.Hidden;
            this.Image_game.Visibility = Visibility.Hidden;
            Music_question_window.Visibility = Visibility.Hidden;
         



        }


        void Creat_Music()
        {
            uris[0] = new Uri(@"C:\Users\danila\source\repos\ArtCritic\ArtCritic Desctop\ArtCritic Desctop\source\source\Voennayakafedra1.mp3");
            uris[1] = new Uri(@"C:\Users\danila\source\repos\ArtCritic\ArtCritic Desctop\ArtCritic Desctop\source\source\Voennayakafedra2.mp3");
            uris[2] = new Uri(@"C:\Users\danila\source\repos\ArtCritic\ArtCritic Desctop\ArtCritic Desctop\source\source\Voennayakafedra3.mp3");
            uris[3] = new Uri(@"C:\Users\danila\source\repos\ArtCritic\ArtCritic Desctop\ArtCritic Desctop\source\source\Voennayakafedra4.mp3");
            for (int i = 0; i < 4; ++i)
            {
                mediaPlayer[i] = new MediaPlayer();
                mediaPlayer[i].Open(uris[i]);
                mediaPlayer[i].Pause();
            }
            StringReader stringReader = new StringReader("C:\\Users\\danila\\source\\repos\\ArtCritic\\ArtCritic Desctop\\ArtCritic Desctop\\Links.txt");
            for (int i = 0; i < 4; ++i)
            {
                string[] cloud_answers = new string[1];
                string textFromFile; 
                string path = @"C:\Users\danila\source\repos\ArtCritic\ArtCritic Desctop\ArtCritic Desctop\"+(i+1)+".txt";
                using (FileStream fstream = File.OpenRead(path))
                {
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    textFromFile = System.Text.Encoding.Default.GetString(array);
                    cloud_answers[0] = textFromFile;
                }
                music_Questions[i] =new Music_question("угадайте название песни" , cloud_answers);
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("пока в разработке");
        }

        private void Satistyc_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("пока в разработке");

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
            MessageBox.Show("пока в разработке");

        }

        private void Music_question_Click(object sender, RoutedEventArgs e)
        {
            Creat_Music();
            Type_of_game.Visibility = Visibility.Hidden;
            Music_question_window.Visibility = Visibility.Visible;
            Music_question_xaml.Text = music_Questions[iter].Text;
            mediaPlayer[iter].Play();
        }

        private void Mixed_questions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("пока в разработке");
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
           
            if (textQuestion.Check_Answer(Answer.Text))
            {
                MessageBox.Show("Верно");
            }
            else { MessageBox.Show( "Неверно"); }
            Test_game_with_Image.Visibility = Visibility.Hidden;
            Type_of_game.Visibility = Visibility.Visible;
        }

        private void Creat_Question_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Music_exit_Click(object sender, RoutedEventArgs e)
        {
            iter = 0;
            Music_question_window.Visibility = Visibility.Hidden;
            Type_of_game.Visibility = Visibility.Visible;
            mediaPlayer[iter].Stop();
        }

        private void Music_accept_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer[iter].Stop();
            if (music_Questions[iter].Check_Answer(Music_answer.Text))
                Player.statistic = Player.statistic + 1;
            Music_answer.Text = "";
            ++iter;
            if (iter == 4)
            {
                Music_question_window.Visibility = Visibility.Hidden;
                Type_of_game.Visibility = Visibility.Visible;
                MessageBox.Show("Вы отгадали " + Player.statistic);
                iter = 0;
            }
            else
                mediaPlayer[iter].Play();
            
        }

        private void Music_replay_music_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer[iter].Stop();
            mediaPlayer[iter].Play();
        }





        private void Image_question_Click(object sender, RoutedEventArgs e)
        {

            Type_of_game.Visibility = Visibility.Hidden;
            Image_game.Visibility = Visibility.Visible;


        }

        private void Accept_Answer_Image_Click(object sender, RoutedEventArgs e) {

   
    
   
        }

    }







   }
