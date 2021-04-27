using System.Windows;
using System;
using System.IO;
//using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
/*using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;*/
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
/*using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;*/
using ArtCritic_Desctop.core;
using System.Text.RegularExpressions;

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
       // /// <summary>
        /// Регулярное выражения для имени директорий
        /// </summary>
       // public Regex directory_name;
        private CreatePack a;

        int answer_sum_video_correct = 0;
        int answer_sum_image_correct = 0;

        public BitmapImage Back_Ground { get; set; }
        public BitmapImage Exit_Button { get; set; }
        public BitmapImage Enter_Button { get; set; }
        public BitmapImage Check_Button { get; set; }

        private List<VideoQuestion> db_video;
        int video_counter = 0;
        string currentAnswer_video;
        MediaPlayer mediaplayer = new MediaPlayer();


        Player Player = new Player();
        private int iter = 0; //пока я не сделаю нормальную обертку итерируем вопросы этой штукой
        private List<Music_question> music_Questions = new List<Music_question>();
        private List<Uri> uris = new List<Uri>();
        private QuestionKeeper question;
        private TextQuestion textQuestion;
        


        private PlayerStat statistics;
        // Для БД
        private String dbFileName;
        private SQLiteConnection m_dbConn;
        private SQLiteCommand m_sqlCmd;


        public MainWindow()
        {
            InitializeComponent();
            CreateAndCheckDb();

            Player.statistic = 0;
            statistics = GetStatistics();
            creat_start_menu();
            string[] test_answers = new string[2];
            test_answers[0] = "Спанч Боб";
            test_answers[1] = "Спанч Боб Скрепенс";
            question = new QuestionKeeper("кто проживает на дне океана?", test_answers);
            //окно выбора типа игры
            this.Game_stat.Visibility = Visibility.Hidden;
            this.Type_of_game.Visibility = Visibility.Hidden;
            //окно тестовой игры Данила
            this.Test_game_with_Image.Visibility = Visibility.Hidden;
            //окно игры с картинками
            this.Image_game.Visibility = Visibility.Hidden;
            //окно игры с видео
            this.Video_game.Visibility = Visibility.Hidden;
            //создание пака
            this.Pack_create.Visibility = Visibility.Hidden;
            //Создание пака Картинки
            this.Pack_create_Image.Visibility = Visibility.Hidden;
            //создание пака музыки
            this.Pack_create_Music.Visibility = Visibility.Hidden;
            //создание пака видео
            this.Pack_create_Video.Visibility = Visibility.Hidden;
            //создание пака mixed
            this.Pack_create_Mixed.Visibility = Visibility.Hidden;
            //кнопки для соответствующих созданий паков (для принятия ответа на картинку/видео/музыку)
            this.Accept_Create_Answer_Image.Visibility = Visibility.Hidden;
            this.Accept_Create_Answer_Video.Visibility = Visibility.Hidden;
            this.Accept_Create_Answer_Mixed.Visibility = Visibility.Hidden;
            this.Accept_Create_Answer_Music.Visibility = Visibility.Hidden;
            //картинка для показа (в создании mixed-картиночного вопроса)
            this.Image_for_create_Mixed.Visibility = Visibility.Hidden;
            //видео для показа (в создании mixed-видео вопроса)
            this.Video_for_create_Mixed.Visibility = Visibility.Hidden;
            //картинка для показа (в создании mixed-музыкального пака)
            this.Image_for_create_Mixed_for_Music.Visibility = Visibility.Hidden;
            //окно игры с музыкой
            Music_question_window.Visibility = Visibility.Hidden;
        }

        private void creat_start_menu()
        {
            string I_Game_Path = System.IO.Path.GetFullPath("..\\..\\..\\Buttons\\button_game.png");
            string I_Settings_Path = System.IO.Path.GetFullPath("..\\..\\..\\Buttons\\button_settings.png");
            string I_Statistics_Path = System.IO.Path.GetFullPath("..\\..\\..\\Buttons\\button_statistics.png");
            string I_Creat_Pack_Path = System.IO.Path.GetFullPath("..\\..\\..\\Buttons\\button_download.png");
            string I_Exit_Path = System.IO.Path.GetFullPath("..\\..\\..\\Buttons\\button_exit.png");
            string I_Background_Path = System.IO.Path.GetFullPath("..\\..\\..\\wallpapers__for_menu_music_pictures\\menu.jpg");
            Uri I_Game_U_Path = new Uri(I_Game_Path, UriKind.RelativeOrAbsolute);
            Uri I_Settings_U_Path = new Uri(I_Settings_Path, UriKind.RelativeOrAbsolute);
            Uri I_Statistics_U_Path = new Uri(I_Statistics_Path, UriKind.RelativeOrAbsolute);
            Uri I_Creat_Pack_U_Path = new Uri(I_Creat_Pack_Path, UriKind.RelativeOrAbsolute);
            Uri I_Exit_U_Path = new Uri(I_Exit_Path, UriKind.RelativeOrAbsolute);
            Uri I_Background_U_Path = new Uri(I_Background_Path, UriKind.RelativeOrAbsolute);
            BitmapImage I_Game_Bitmap = new BitmapImage(I_Game_U_Path);
            BitmapImage I_Settings_Bitmap = new BitmapImage(I_Settings_U_Path);
            BitmapImage I_Statistics_Bitmap = new BitmapImage(I_Statistics_U_Path);
            BitmapImage I_Creat_Pack_Bitmap = new BitmapImage(I_Creat_Pack_U_Path);
            BitmapImage I_Exit_Bitmap = new BitmapImage(I_Exit_U_Path);
            BitmapImage I_Background_Bitmap = new BitmapImage(I_Background_U_Path);
            this.I_Exit.Source = I_Exit_Bitmap;
            this.I_Game.Source = I_Game_Bitmap;
            this.I_Settings.Source = I_Settings_Bitmap;
            this.I_Statistics.Source = I_Statistics_Bitmap;
            this.I_Background.Source = I_Background_Bitmap;
            this.I_Create_Pack_Button.Source = I_Creat_Pack_Bitmap;
        }

        // Проверяем существование базы данных, если её нет - создаём
        private void CreateAndCheckDb()
        {
            m_dbConn = new SQLiteConnection();
            m_sqlCmd = new SQLiteCommand();
            dbFileName = "db.sqlite";

            if (!File.Exists(dbFileName))
                SQLiteConnection.CreateFile(dbFileName);

            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;

                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS player_stat (id INTEGER PRIMARY KEY UNIQUE NOT NULL, played_games INTEGER NOT NULL, total_questions INTEGER NOT NULL, total_correct_answers INTEGER NOT NULL, current_result DOUBLE NOT NULL)";
                m_sqlCmd.ExecuteNonQuery();

                m_sqlCmd.CommandText = "SELECT id FROM player_stat WHERE ID = 1";
                SQLiteDataReader reader = m_sqlCmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    m_sqlCmd.CommandText = "INSERT INTO player_stat(id, played_games, total_questions, total_correct_answers, current_result) VALUES (1, 0, 0, 0, 0.0)";
                    m_sqlCmd.ExecuteNonQuery();
                }
                reader.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ошибка подключения к БД: " + ex.Message);
                this.Close();
            }
        }

        private PlayerStat GetStatistics()
        {
            PlayerStat result;
            try
            {
                m_sqlCmd.CommandText = "SELECT * FROM player_stat WHERE ID = 1";
                SQLiteDataReader reader = m_sqlCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    result = new PlayerStat(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetDouble(4));
                    return result;
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                this.Close();
            }
            return null;
        }


        private void updateStatistics(int correctAnswer, int totalAnswer)
        {
            statistics.CurrentResult = (statistics.CurrentResult * statistics.PlayedGames + correctAnswer * 100 / totalAnswer) / (statistics.PlayedGames + 1);
            statistics.PlayedGames++;
            statistics.TotalQuestions += totalAnswer;
            statistics.TotalCorrectAnswers += correctAnswer;
            saveStatistics();
        }

        private void saveStatistics()
        {
            try
            {
                m_sqlCmd.CommandText = "UPDATE player_stat SET played_games = '"
                    + statistics.PlayedGames.ToString() + "', total_questions = '"
                    + statistics.TotalQuestions.ToString() + "', total_correct_answers = '"
                    + statistics.TotalCorrectAnswers.ToString() + "', current_result = '"
                    + statistics.CurrentResult.ToString() + "' WHERE id = '1'";
                m_sqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                this.Close();
            }
        }


        //Элементы main меню

        private void Create_Pack_Button_Click(object sender, RoutedEventArgs e)
        {
            Main_menu.Visibility = Visibility.Hidden;
            Pack_create.Visibility = Visibility.Visible;
        }
        private void Game_Click(object sender, RoutedEventArgs e)
        {
            Main_menu.Visibility = Visibility.Hidden;
            Type_of_game.Visibility = Visibility.Visible;
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("пока в разработке");
        }
        private void Satistyc_Click(object sender, RoutedEventArgs e)
        {
            Played_Games_Label2.Content = statistics.PlayedGames;
            TotalQuestionsLabe2.Content = statistics.TotalQuestions;
            TotalCorrectAnswersLabe2.Content = statistics.TotalCorrectAnswers;
            CurrentResultLabe2.Content = statistics.CurrentResult + "%";
            Game_stat.Visibility = Visibility.Visible;
            Main_menu.Visibility = Visibility.Hidden;
        }

        private void Statictics_Back(object sender, RoutedEventArgs e)
        {
            Game_stat.Visibility = Visibility.Hidden;
            Main_menu.Visibility = Visibility.Visible;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Конец элементов main меню

        //Начало элементов для создания паков

        /// <summary>
        /// Проверка корректности имени (только латинские и цифры размер максимум 1000 символов)
        /// </summary>
        /// <param name="pack_name"></param>
        /// <returns></returns>
        public bool name_control(string pack_name)
        {
            return (!string.IsNullOrEmpty(pack_name) && Regex.IsMatch(pack_name, @"^([A-Za-z0-9]){1,1000}$", RegexOptions.Multiline));
        }

        private void Create_Image_Pack_Click(object sender, RoutedEventArgs e)
        {
            Pack_create.Visibility = Visibility.Hidden;

            Pack_create_Image.Visibility = Visibility.Visible;

            Create_Question_Image_TBlock.Text = "Введите название пака";
        }
        private void Accept_Create_Name_Image_Pack_Click(object sender, RoutedEventArgs e)
        {
            if (name_control(Creat_Answer_Image_Texbox.Text))
            {
                CreatePack image_pack = new CreatePack(2, Creat_Answer_Image_Texbox.Text, Image_for_create, Creat_Answer_Image_Texbox, Create_Question_Image_TBlock);
                a = image_pack;
                this.Closing += a.Delete_Naher;
                if (a.is_create == true) { this.Close(); }
                Accept_Create_Name_Image_Pack.Visibility = Visibility.Hidden;
                Accept_Create_Answer_Image.Visibility = Visibility.Visible;
                Creat_Answer_Image_Texbox.Text = "";
            }
            else
            {
                MessageBox.Show("Введите название состоящее только из букв(eng) и цифр.Без пробелов!");
                Creat_Answer_Image_Texbox.Text = "";
            }
        }
        private void Accept_Create_Answer_Image_Click(object sender, RoutedEventArgs e)
        {
            a.User_Create_CLick(sender, e);
            Creat_Answer_Image_Texbox.Text = "";
            if (a.is_create == true) { this.Close(); }
        }

        private void Create_Music_Pack_Click(object sender, RoutedEventArgs e)
        {
            Pack_create.Visibility = Visibility.Hidden;

            Pack_create_Music.Visibility = Visibility.Visible;

            Create_Question_Music_TBlock.Text = "Как вы назовёте свой пак? ";
        }
        private void Accept_Create_Name_Music_Pack_Click(object sender, RoutedEventArgs e)
        {
            if (name_control(Creat_Answer_Music_Texbox.Text))
            {
                CreatePack Music_pack = new CreatePack(1, Creat_Answer_Music_Texbox.Text, mediaplayer, Creat_Answer_Music_Texbox, Create_Question_Music_TBlock);
                a = Music_pack;
                this.Closing += a.Delete_Naher;
                if (a.is_create == true) { this.Close(); }
                Accept_Create_Name_Music_Pack.Visibility = Visibility.Hidden;
                Accept_Create_Answer_Music.Visibility = Visibility.Visible;
                Creat_Answer_Music_Texbox.Text = "";
            }
            else
            {
                MessageBox.Show("Введите название состоящее только из букв(eng) и цифр.Без пробелов!");
                Creat_Answer_Music_Texbox.Text = "";
            }
        }
        private void Accept_Create_Answer_Music_Click(object sender, RoutedEventArgs e)
        {
            a.User_Create_CLick(sender, e);
            Creat_Answer_Music_Texbox.Text = "";
            if (a.is_create == true) { this.Close(); }
        }

        private void Create_Video_Pack_Click(object sender, RoutedEventArgs e)
        {
            Pack_create.Visibility = Visibility.Hidden;

            Pack_create_Video.Visibility = Visibility.Visible;

            Create_Question_Video_TBlock.Text = "Как вы назовёте свой пак? ";
        }
        private void Accept_Create_Name_Video_Pack_Click(object sender, RoutedEventArgs e)
        {
            if (name_control(Creat_Answer_Video_Texbox.Text))
            {
                CreatePack Video_pack = new CreatePack(3, Creat_Answer_Video_Texbox.Text, Video_for_create, Creat_Answer_Video_Texbox, Create_Question_Video_TBlock);
                a = Video_pack;
                this.Closing += a.Delete_Naher;
                if (a.is_create == true) { this.Close(); }
                Accept_Create_Name_Video_Pack.Visibility = Visibility.Hidden;
                Accept_Create_Answer_Video.Visibility = Visibility.Visible;
                Creat_Answer_Video_Texbox.Text = "";
            }
            else
            {
                MessageBox.Show("Введите название состоящее только из букв(eng) и цифр.Без пробелов!");
                Creat_Answer_Video_Texbox.Text = "";
            }
        }
        private void Accept_Create_Answer_Video_Click(object sender, RoutedEventArgs e)
        {
            a.User_Create_CLick(sender, e);
            Creat_Answer_Video_Texbox.Text = "";
            if (a.is_create == true) { this.Close(); }
        }


        private void Create_Mixed_Pack_Click(object sender, RoutedEventArgs e)
        {
            Pack_create.Visibility = Visibility.Hidden;
            Pack_create_Mixed.Visibility = Visibility.Visible;
            Create_Question_Mixed_TBlock.Text = "Как вы назовёте свой пак? ";
        }
        private void Accept_Create_Name_Mixed_Pack_Click(object sender, RoutedEventArgs e)
        {
            if (name_control(Creat_Answer_Mixed_Texbox.Text))
            {

                CreatePack Mixed_pack = new CreatePack(4, Creat_Answer_Mixed_Texbox.Text, Video_for_create_Mixed, Image_for_create_Mixed, mediaplayer, Creat_Answer_Mixed_Texbox, Create_Question_Mixed_TBlock);
                a = Mixed_pack;
                if (a.is_image == true)
                {
                    Image_for_create_Mixed.Visibility = Visibility.Visible;
                }
                if (a.is_video == true)
                {
                    Video_for_create_Mixed.Visibility = Visibility.Visible;
                }
                if (a.is_music == true)
                {
                    Image_for_create_Mixed_for_Music.Visibility = Visibility.Visible;
                }
                this.Closing += a.Delete_Naher;
                if (a.is_create == true) { this.Close(); }
                Accept_Create_Name_Mixed_Pack.Visibility = Visibility.Hidden;
                Accept_Create_Answer_Mixed.Visibility = Visibility.Visible;
                Creat_Answer_Mixed_Texbox.Text = "";
            }
            else
            {
                MessageBox.Show("Введите название состоящее только из букв(eng) и цифр.Без пробелов!");
                Creat_Answer_Mixed_Texbox.Text = "";
            }
        }
        private void Accept_Create_Answer_Mixed_Click(object sender, RoutedEventArgs e)
        {
            Image_for_create_Mixed.Visibility = Visibility.Hidden;
            Video_for_create_Mixed.Visibility = Visibility.Hidden;
            Image_for_create_Mixed_for_Music.Visibility = Visibility.Hidden;
            a.User_Create_CLick(sender, e);
            Creat_Answer_Mixed_Texbox.Text = "";
            if (a.is_image == true)
            {
                Image_for_create_Mixed.Visibility = Visibility.Visible;
            }
            if (a.is_video == true)
            {
                Video_for_create_Mixed.Visibility = Visibility.Visible;
            }
            if (a.is_music == true)
            {
                Image_for_create_Mixed_for_Music.Visibility = Visibility.Visible;
            }
            if (a.is_create == true)
            {
                this.Close();
            }
        }

        //Конец элементов для создания паков

        //Элементы меню Game

        private void Video_question_Click(object sender, RoutedEventArgs e)
        {

            Type_of_game.Visibility = Visibility.Hidden;
            Video_game.Visibility = Visibility.Visible;

            Back_Ground = new BitmapImage();
            Back_Ground.BeginInit();
            string Background_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\wallpapers__for_menu_music_pictures\guess_film.jpg");
            Back_Ground.UriSource = new Uri(Background_Image_Path, UriKind.RelativeOrAbsolute);
            Back_Ground.EndInit();
            Video_Background.Source = Back_Ground;

            Exit_Button = new BitmapImage();
            Exit_Button.BeginInit();
            string Exit_Button_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\Buttons\button_exit_game.png");
            Exit_Button.UriSource = new Uri(Exit_Button_Image_Path, UriKind.RelativeOrAbsolute);
            Exit_Button.EndInit();
            Exit_For_Video.Source = Exit_Button;

            Check_Button = new BitmapImage();
            Check_Button.BeginInit();
            string Check_Button_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\Buttons\button_next.png");
            Check_Button.UriSource = new Uri(Check_Button_Image_Path, UriKind.RelativeOrAbsolute);
            Check_Button.EndInit();
            Accept_Answer_Video.Source = Check_Button;

            Create_Video_List();
            LoadNewVideoQuestion();
        }
        private void Music_question_Click(object sender, RoutedEventArgs e)
        {
            Creat_Music();
            Type_of_game.Visibility = Visibility.Hidden;
            Music_question_window.Visibility = Visibility.Visible;
            Music_question_xaml.Text = music_Questions[iter].Text;
            music_Questions[iter].Play();
        }
        private void Image_question_Click(object sender, RoutedEventArgs e)
        {

            Type_of_game.Visibility = Visibility.Hidden;
            Image_game.Visibility = Visibility.Visible;



            Back_Ground = new BitmapImage();
            Back_Ground.BeginInit();
            string Background_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\wallpapers__for_menu_music_pictures\guess_picture.jpg");
            Back_Ground.UriSource = new Uri(Background_Image_Path, UriKind.RelativeOrAbsolute);
            Back_Ground.EndInit();
            Image_Background.Source = Back_Ground;

            Exit_Button = new BitmapImage();
            Exit_Button.BeginInit();
            string Exit_Button_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\Buttons\button_exit_game.png");
            Exit_Button.UriSource = new Uri(Exit_Button_Image_Path, UriKind.RelativeOrAbsolute);
            Exit_Button.EndInit();
            exit_pic.Source = Exit_Button;

            Check_Button = new BitmapImage();
            Check_Button.BeginInit();
            string Check_Button_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\Buttons\button_next.png");
            Check_Button.UriSource = new Uri(Check_Button_Image_Path, UriKind.RelativeOrAbsolute);
            Check_Button.EndInit();
            Accept_Answer_Image.Source = Check_Button;


            Create_Image_List();
            LoadNewImageQuestion();
        }
        private void Mixed_questions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("пока в разработке");
        }

        //Конец элементов меню Game

        //Элементы для режима игры Видео

        /// <summary>
        /// Создаётся List элементов Video_question
        /// </summary>
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
        /// Нажатие на кнопку "Ввести"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept_Answer_Video_Click(object sender, RoutedEventArgs e)
        {
            Check_Answer_video();
        }
        /// <summary>
        /// проверка ответа на видео
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
            else {
                MessageBox.Show("Молодец! твой результат: " + answer_sum_video_correct + "/" + db_video.Count);
                updateStatistics(answer_sum_video_correct, db_video.Count);
                Close();
            }
        }

        //Конец элементов для режима Видео

        //Элементы для режима игры Музыка

        void Creat_Music()
        {
            StreamReader streamReader = new StreamReader(@"..\..\..\Links.txt");
            for (int i = 0; i < 6; ++i)
            {
                
                string textFromFile = streamReader.ReadLine();
                string[] cloud_answers;
                cloud_answers = textFromFile.Split('|');
                string[] ans = new string[1];
                ans[0] = cloud_answers[1];
                music_Questions.Add(new Music_question("угадайте название песни", ans, new Uri(cloud_answers[0], UriKind.Relative)));
                music_Questions[i].Stop();
            }
            string I_Music_Replay_Path = System.IO.Path.GetFullPath("..\\..\\..\\Buttons\\button_repeat.png");
            string I_Music_Accept_Path = System.IO.Path.GetFullPath("..\\..\\..\\Buttons\\button_next.png");
            string I_Music_Exit_Path = System.IO.Path.GetFullPath("..\\..\\..\\Buttons\\button_exit_game.png");
            string I_Background_Path = System.IO.Path.GetFullPath("..\\..\\..\\wallpapers__for_menu_music_pictures\\guess_music.jpg");
            Uri I_Music_Replay_U_Path = new Uri(I_Music_Replay_Path, UriKind.RelativeOrAbsolute);
            Uri I_Music_Accept_U_Path = new Uri(I_Music_Accept_Path, UriKind.RelativeOrAbsolute);
            Uri I_Music_Exit_U_Path = new Uri(I_Music_Exit_Path, UriKind.RelativeOrAbsolute);
            Uri I_Background_U_Path = new Uri(I_Background_Path, UriKind.RelativeOrAbsolute);
            BitmapImage I_Music_Replay_Bitmap = new BitmapImage(I_Music_Replay_U_Path);
            BitmapImage I_Music_Accept_Bitmap = new BitmapImage(I_Music_Accept_U_Path);
            BitmapImage I_Music_Exit_Bitmap = new BitmapImage(I_Music_Exit_U_Path);
            BitmapImage I_Background_Bitmap = new BitmapImage(I_Background_U_Path);
            this.I_Music_Exit.Source = I_Music_Exit_Bitmap;
            this.I_Music_accept.Source = I_Music_Accept_Bitmap;
            this.I_Music_replay.Source = I_Music_Replay_Bitmap;           
            this.I_Music_background.Source = I_Background_Bitmap;            
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
                MessageBox.Show("Вы отгадали верно " + Player.statistic);
                updateStatistics(Player.statistic, iter);
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
        private void Music_exit_Click(object sender, RoutedEventArgs e)
        {
            music_Questions[iter].Stop();
            iter = 0;
            Music_question_window.Visibility = Visibility.Hidden;
            Type_of_game.Visibility = Visibility.Visible;
            
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
                MessageBox.Show("Вы отгадали правильно " + Player.statistic);
                updateStatistics(Player.statistic, iter);

                iter = 0;
            }
            else
                music_Questions[iter].Play();
        }
        private void Music_exit_image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            music_Questions[iter].Stop();
            iter = 0;
            Music_question_window.Visibility = Visibility.Hidden;
            Type_of_game.Visibility = Visibility.Visible;

        }
        private void I_Music_replay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            music_Questions[iter].Stop();
            music_Questions[iter].Play();
        }

        //Конец элементов для режима игры Музыка

        //Элементы для режима игры Картинка

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
        private void LoadNewImageQuestion()
        {

            //var q = db[image_counter % db.Count];
            var q = db[image_counter];
            pice.Source = q.Picture;
            currentAnswer_image = q.Answer;
            image_counter++;

        }
        private void Accept_Answer_Image_Click(object sender, RoutedEventArgs e)
        {
            Check_Answer_Image();
            Answer_Image_Texbox.Text = "";
        }
        public void Check_Answer_Image()
        {
            string word = String.Empty;
            word = Answer_Image_Texbox.Text;

            if (word == currentAnswer_image)
            {
                answer_sum_image_correct++;
            }
            if (image_counter != db.Count)
            {
                LoadNewImageQuestion();
            }
            else
            {
                MessageBox.Show("Молодец! твой результат: " + answer_sum_image_correct + "/" + db.Count); Close();
                updateStatistics(answer_sum_image_correct, db.Count);
            }
        }

        
 

        //Конец элементов для режима игры Картинка
        
        //Какая-то вещь известного назначения

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
            else { MessageBox.Show("Неверно"); }
            Test_game_with_Image.Visibility = Visibility.Hidden;
            Type_of_game.Visibility = Visibility.Visible;
        }

        private void I_Game_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main_menu.Visibility = Visibility.Hidden;
            Type_of_game.Visibility = Visibility.Visible;
        }

        private void I_Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Пока в разработке");
        }

        private void I_Statistics_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Played_Games_Label2.Content = statistics.PlayedGames;
            TotalQuestionsLabe2.Content = statistics.TotalQuestions;
            TotalCorrectAnswersLabe2.Content = statistics.TotalCorrectAnswers;
            CurrentResultLabe2.Content = statistics.CurrentResult + "%";
            Game_stat.Visibility = Visibility.Visible;
            Main_menu.Visibility = Visibility.Hidden;
        }

        private void I_Create_Pack_Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main_menu.Visibility = Visibility.Hidden;
            Pack_create.Visibility = Visibility.Visible;
        }

       
    }

}
