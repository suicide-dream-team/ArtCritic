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
using System.Reflection;
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
        MediaPlayer[] mediaPlayer = new MediaPlayer[4];
        public MainWindow()
        {


            InitializeComponent();
            //directory_name = new Regex(@"^([A - Za - z0 - 9]){ 1, 20 }$",RegexOptions.IgnoreCase);
            Player.statistic = 0;
            string[] test_answers = new string[2];
            test_answers[0] = "Спанч Боб";
            test_answers[1] = "Спанч Боб Скрепенс";
            question = new QuestionKeeper("кто проживает на дне океана?", test_answers);
            //окно выбора типа игры
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

        /// <summary>
        /// Проверка корректности имени (только латинские и цифры размер максимум 1000 символов)
        /// </summary>
        /// <param name="pack_name"></param>
        /// <returns></returns>
        public bool name_control(string pack_name)
        {
            return (!string.IsNullOrEmpty(pack_name) && Regex.IsMatch(pack_name, @"^([A-Za-z0-9]){1,1000}$", RegexOptions.Multiline));
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
            else { MessageBox.Show("Молодец! твой результат: " + answer_sum_video_correct + "/" + db_video.Count); Close(); }
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
        /// проверка ответа на картинку
        /// </summary>
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
            }
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

            Back_Ground = new BitmapImage();
            Back_Ground.BeginInit();
            string Background_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\source_2.0\guess_film.jpg");
            Back_Ground.UriSource = new Uri(Background_Image_Path, UriKind.RelativeOrAbsolute);
            Back_Ground.EndInit();
            Video_Background.Source = Back_Ground;



            Exit_Button = new BitmapImage();
            Exit_Button.BeginInit();
            string Exit_Button_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\source_2.0\exit.png");
            Exit_Button.UriSource = new Uri(Exit_Button_Image_Path, UriKind.RelativeOrAbsolute);
            Exit_Button.EndInit();
            Exit_For_Video.Source = Exit_Button;

            Check_Button = new BitmapImage();
            Check_Button.BeginInit();
            string Check_Button_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\source_2.0\check.png");
            Check_Button.UriSource = new Uri(Check_Button_Image_Path, UriKind.RelativeOrAbsolute);
            Check_Button.EndInit();
            Accept_Answer_Video.Source = Check_Button;

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


            
            Back_Ground = new BitmapImage();
            Back_Ground.BeginInit();
            string Background_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\source_2.0\guess_picture.jpg");
            Back_Ground.UriSource = new Uri(Background_Image_Path, UriKind.RelativeOrAbsolute);
            Back_Ground.EndInit();
            Image_Background.Source = Back_Ground;



            Exit_Button = new BitmapImage();
            Exit_Button.BeginInit();
            string Exit_Button_Image_Path = System.IO.Path.GetFullPath(@"..\..\..\source_2.0\exit.png");
            Exit_Button.UriSource = new Uri(Exit_Button_Image_Path, UriKind.RelativeOrAbsolute);
            Exit_Button.EndInit();
            exit_pic.Source = Exit_Button;

            Check_Button = new BitmapImage();
            Check_Button.BeginInit();
            string Check_Button_Image_Path= System.IO.Path.GetFullPath(@"..\..\..\source_2.0\check.png");
            Check_Button.UriSource = new Uri(Check_Button_Image_Path, UriKind.RelativeOrAbsolute);
            Check_Button.EndInit();
            Accept_Answer_Image.Source = Check_Button;


            Create_Image_List();
            LoadNewImageQuestion();
        }
        private void Accept_Answer_Image_Click(object sender, RoutedEventArgs e)
        {
            Check_Answer_Image();
            Answer_Image_Texbox.Text = "";
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
            Main_menu.Visibility = Visibility.Hidden;
            Pack_create.Visibility = Visibility.Visible;
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
        private void Accept_Create_Image_Pack_Click(object sender, RoutedEventArgs e)
        {
            Pack_create.Visibility = Visibility.Hidden;

            Pack_create_Image.Visibility = Visibility.Visible;

            Create_Question_Image_TBlock.Text = "Введите название пака";
        }
        private void Accept_Create_Answer_Image_Click(object sender, RoutedEventArgs e)
        {
            a.User_Create_CLick(sender, e);
            Creat_Answer_Image_Texbox.Text = "";
            if (a.is_create == true) { this.Close(); }
        }
        private void Accept_Create_Name_Image_Pack_Click(object sender, RoutedEventArgs e)
        {
            if (name_control(Creat_Answer_Image_Texbox.Text)) 
            {
                CreatePack image_pack = new CreatePack(2, Creat_Answer_Image_Texbox.Text, Image_for_create, Creat_Answer_Image_Texbox, Create_Question_Image_TBlock);
                a = image_pack;
                this.Closing += a.Delete_Naher;
                if (a.is_create == true) {  this.Close(); }
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

        private void Accept_Create_Answer_Music_Click(object sender, RoutedEventArgs e)
        {
            a.User_Create_CLick(sender, e);
            Creat_Answer_Music_Texbox.Text = "";
            if (a.is_create == true) { this.Close(); }
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

        private void Create_Music_Pack_Click(object sender, RoutedEventArgs e)
        {
            Pack_create.Visibility = Visibility.Hidden;

            Pack_create_Music.Visibility = Visibility.Visible;

            Create_Question_Music_TBlock.Text = "Как вы назовёте свой пак? ";
        }
    }
}
