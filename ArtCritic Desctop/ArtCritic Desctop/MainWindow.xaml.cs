﻿using System.Windows;
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
using System.Reflection;

namespace ArtCritic_Desctop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Image_Question> db;
        int image_counter = 0;
        string currentAnswer_image;


        int answer_sum_video_correct = 0;
        int answer_sum_image_correct = 0;

        private List<VideoQuestion> db_video;
        int video_counter = 0;
        string currentAnswer_video;

        Player Player = new Player();
        private int iter = 0; //пока я не сделаю нормальную обертку итерируем вопросы этой штукой
        private List<Music_question> music_Questions = new List<Music_question>();
        private List<Uri> uris = new List<Uri>();
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
            /*BitmapImage[] bitmapImages = new BitmapImage[3];
            bitmapImages[0].BaseUri = new Uri(@"..\..\..\source_2.0\check.png", UriKind.Relative);
            bitmapImages[0].BeginInit();
            //bitmapImages[1] = 
            //bitmapImages[2]
            Music_accept_image.Source = bitmapImages[0];*/
            







            this.Type_of_game.Visibility = Visibility.Hidden;
            this.Test_game_with_Image.Visibility = Visibility.Hidden;
            this.Image_game.Visibility = Visibility.Hidden;
            this.Video_game.Visibility = Visibility.Hidden;

            Music_question_window.Visibility = Visibility.Hidden;



        }



        void Create_Video_List()
        {

            db_video = new List<VideoQuestion>();
            video_counter = 0;
            var dataFile = File.ReadAllLines(@"..\..\..\Videos\answersV.txt");
            foreach (var e in dataFile)
            {
                var args = e.Split('|');
                db_video.Add(new VideoQuestion(args[0], args[1]));

            }

        }

        /// <summary>
        /// загрузка нового вопроса видео
        /// </summary>
        private void LoadNewVideoQuestion()
        {

            // var g = db_video[video_counter % db_video.Count];

            var g = db_video[video_counter];
            video.Source = g.Path_To_Video;
            currentAnswer_video = g.Answer_for_video;
            video_counter++;
        }

        /// <summary>
        /// проверка вопроса видео
        /// </summary>
        public void Check_Answer_video()
        {
            string word = String.Empty;
            word = Answer_Video_Texbox.Text;

            if (word == currentAnswer_video)
            {

                answer_sum_video_correct++;

            }

            if (video_counter != db_video.Count) { LoadNewVideoQuestion(); }
            else { MessageBox.Show("Молодец! твой результат: " + answer_sum_video_correct + "/" + db_video.Count); Close(); }



            //    if (word == currentAnswer_video)
            //  {
            //  MessageBox.Show("Молодец");
            //LoadNewVideoQuestion();
            //}
            //else { MessageBox.Show("Неверно"); }




        }



        /// <summary>
        /// загрузка нового вопроса картинки
        /// </summary>
        private void LoadNewImageQuestion()
        {

            //var q = db[image_counter % db.Count];
            var q = db[image_counter];
            pice.Source = q.Picture;
            currentAnswer_image = q.Answer;
            image_counter++;

        }

        /// <summary>
        /// проверка вопроса картинок
        /// </summary>
        public void Check_Answer_Image()
        {
            string word = String.Empty;
            word = Answer_Image_Texbox.Text;

            if (word == currentAnswer_image)
            {
                answer_sum_image_correct++;
            }



            //  if (word == currentAnswer_image)
            //   {
            //     MessageBox.Show("Молодец");

            if (image_counter != db.Count) { LoadNewImageQuestion(); }
            else { MessageBox.Show("Молодец! твой результат: " + answer_sum_image_correct + "/" + db.Count); Close(); }
            //  }
            // else { MessageBox.Show("Неверно"); }
        }



        /// <summary>
        /// создание листа для картиночных вопросов
        /// </summary>
        void Create_Image_List()
        {

            db = new List<Image_Question>();
            image_counter = 0;
            var dataFile = File.ReadAllLines(@"..\..\..\Images\answers.txt");
            foreach (var e in dataFile)
            {
                var args = e.Split('|');
                db.Add(new Image_Question(args[0], args[1]));
            }

        }





        void Creat_Music()
        {           
            for (int i = 0; i < 6; ++i)
            {
                StreamReader streamReader = new StreamReader(@"..\..\..\Links.txt");
                string textFromFile = streamReader.ReadLine();
                string[] cloud_answers;
                cloud_answers = textFromFile.Split('|');
                string[] ans = new string[1];
                ans[0] = cloud_answers[1];
                music_Questions.Add(new Music_question("угадайте название песни", ans, new Uri(cloud_answers[0], UriKind.Relative)));
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

            Type_of_game.Visibility = Visibility.Hidden;
            Video_game.Visibility = Visibility.Visible;
            Create_Video_List();
            LoadNewVideoQuestion();

        }

        private void Accept_Answer_Video_Click(object sender, RoutedEventArgs e)
        {
            Check_Answer_video();
        }


        private void Music_question_Click(object sender, RoutedEventArgs e)
        {
            Creat_Music();
            Type_of_game.Visibility = Visibility.Hidden;
            Music_question_window.Visibility = Visibility.Visible;
            Music_question_xaml.Text = music_Questions[iter].Text;
            music_Questions[iter].Play();
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



        private void Image_question_Click(object sender, RoutedEventArgs e)
        {

            Type_of_game.Visibility = Visibility.Hidden;
            Image_game.Visibility = Visibility.Visible;
            Create_Image_List();
            LoadNewImageQuestion();





        }

        private void Accept_Answer_Image_Click(object sender, RoutedEventArgs e)
        {
            Check_Answer_Image();





        }







        private void Accept_Click(object sender, RoutedEventArgs e)
        {

            if (textQuestion.Check_Answer(Answer.Text))
            {
                MessageBox.Show("Верно");
            }
            else { MessageBox.Show("Неверно"); }
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
            music_Questions[iter].Stop();
        }

        private void Music_accept_Click(object sender, RoutedEventArgs e)
        {
            music_Questions[iter].Stop();
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
                music_Questions[iter].Play();
            
        }

        private void Music_replay_music_Click(object sender, RoutedEventArgs e)
        {
            music_Questions[iter].Stop();
            music_Questions[iter].Play();
        }

        private void exit_pic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Music_accept_image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            music_Questions[iter].Stop();
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
                music_Questions[iter].Play();
        }

        private void Music_exit_image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            iter = 0;
            Music_question_window.Visibility = Visibility.Hidden;
            Type_of_game.Visibility = Visibility.Visible;
            music_Questions[iter].Stop();
        }
    }







}
