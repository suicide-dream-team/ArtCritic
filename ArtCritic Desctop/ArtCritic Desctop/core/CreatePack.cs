using System;
using System.Collections.Generic;
using System.IO;//для директорий
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Ionic.Zip;//зип прикол

namespace ArtCritic_Desctop.core
{
    class CreatePack
    {
        /// <summary>
        /// Имя пака 
        /// </summary>
        public string Namepack;
        /// <summary>
        /// тип созданной игры 4 потому что ещё не выбран тип
        /// </summary>
        public int type_OF_create_game = 4;
        //0 text game only questions and answers-
        //1 music game-
        //2 image game+
        //3 game_with_video+
        //4 mixed-
        /// <summary>
        /// итератор для прохода под массиву файлов
        /// </summary>
        public int iter = 0;
        /// <summary>
        /// количество файлов в папке кроме текстового
        /// </summary> 
        public int kol_vo_in_dir = 0;
        /// <summary>
        /// Создался пак или нет(чтобы закрыть окно)
        /// </summary>
        public bool is_create=false;
        /// <summary>
        /// главный массив для файлов хранит все кроме текстового
        /// </summary>
        public FileInfo[] Files;
        /// <summary>
        /// Текстбокс для Юзера чтобы он вводил имя/ответ на картинку
        /// </summary>
        public TextBox Users_Answer;
        /// <summary>
        /// Текстблок для вывода вопроса для юзера(Какое имя пака/ответ для этой картинки)
        /// </summary>
        public TextBlock Question_for_user;
        /// <summary>
        /// Картинка для юзера
        /// </summary>
        public Image img_for_user;
        /// <summary>
        /// медиафайл для юзера
        /// </summary>
        public MediaElement video_for_user;
        //создаётся папка в которую пользователь в зависимости от выбора кидает картинки/видео/музыку
        //внутри папки создаётся файл с путями+ответами 
        //вся папка архивируется архив сохраняется папка удаляется

        /// <summary>
        /// Создаём папку и текстовый файл
        /// </summary>
        /// <param name="type_of_game"></param>
        /// <param name="pack_name"></param>
        /// <param name="video_for_create_user_pack"></param>
        /// <param name="TB_For_Answer"></param>
        /// <param name="TBck_for_user"></param>
        public CreatePack(int type_of_game, string pack_name, MediaElement video_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user)
        {
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            Question_for_user = TBck_for_user;
            video_for_user = video_for_create_user_pack;
            DirectoryInfo dirInfo = new DirectoryInfo(@"\PacksCreated\" + pack_name);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            //открытие папки которую создал пользователь 
            System.Diagnostics.Process.Start("explorer", @"\PacksCreated\" + pack_name);
                MessageBoxResult messageBoxResult = MessageBox.Show("Скидывайте свои видео(.mp4) в папку как закинете нажмите ok ", " Закинули? ", MessageBoxButton.OK);
            //нажатие на ОК в мессдж боксе   
            if (messageBoxResult == MessageBoxResult.OK)
                {
                    //txt файл создание
                    File.Create(@"\PacksCreated\" + pack_name + @"\answersV.txt").Close();
                    //выцепляем только mp4 
                    FileInfo[] GraphicFiles = dirInfo.GetFiles("*.mp4");
                    Files = GraphicFiles;
                    kol_vo_in_dir = GraphicFiles.Length;
                }
            Create_Video_for_user();
        }

        public CreatePack(int type_of_game, string pack_name, Image image_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user)
        {
           
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            img_for_user = image_for_create_user_pack;
            Question_for_user = TBck_for_user;

            DirectoryInfo dirInfo = new DirectoryInfo(@"\PacksCreated\" + pack_name);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            // чтобы пользователь сам писал ответ для картинки // окно вывода+textblock+image
            System.Diagnostics.Process.Start("explorer", @"\PacksCreated\" + pack_name);
                MessageBoxResult messageBoxResult = MessageBox.Show("Скидывайте свои картинки(.jpg) в папку как закинете нажмите ok ", " Закинули? ", MessageBoxButton.OK);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    File.Create(@"\PacksCreated\" + pack_name + @"\answersI.txt").Close();
                    FileInfo[] GraphicFiles = dirInfo.GetFiles("*.jpg");
                    Files = GraphicFiles;
                    kol_vo_in_dir = GraphicFiles.Length;
                }
                Create_Image_for_user(); 
        }
        public void User_Create_CLick(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"\PacksCreated\" + Namepack);

            if (type_OF_create_game == 3) {
                
                string nameFile = Files[iter].Name;
                using (StreamWriter w = File.AppendText(@"\PacksCreated\" + Namepack + @"\answersV.txt"))
                {
                    w.WriteLine(@"\Packs\" + Namepack + @"\" + nameFile + "|" + Users_Answer.Text);
                }
                iter++;
                if (iter < kol_vo_in_dir)
                {
                    Create_Video_for_user();
                }
                else
                {
                    //создание архива
                    ZipFile zf = new ZipFile(@"\PacksCreated\" + Namepack + ".zip");
                    zf.AddDirectory(@"\PacksCreated\" + Namepack);
                    zf.Save();


                    Directory.Delete(@"\..\..\..\PacksCreated\" + Namepack, true);

                    MessageBox.Show("Пак успешно создан");
                    is_create = true;
                }

            }

                if (type_OF_create_game == 2)
            { 
                    string nameFile = Files[iter].Name;
                    using (StreamWriter w = File.AppendText(@"\PacksCreated\" + Namepack + @"\answersI.txt"))
                    {
                        
                        w.WriteLine(@"\Packs\" + Namepack + @"\" + nameFile + "|" + Users_Answer.Text);
                    }
                    iter++;
                if (iter < kol_vo_in_dir)
                {
                    Create_Image_for_user();
                }
                else
                {
                    //создание архива
                    ZipFile zf = new ZipFile(@"\PacksCreated\" + Namepack + ".zip");
                    zf.AddDirectory(@"\PacksCreated\" + Namepack);
                    zf.Save();

                    Directory.Delete(@"\PacksCreated\" + Namepack, true);

                    MessageBox.Show("Пак успешно создан");
                    is_create = true;
                }
            }
        }
        /// <summary>
        /// Показывает видео на экране
        /// </summary>
        public void Create_Video_for_user() 
        {
            Question_for_user.Text = "Какой вы хотите ответ для этого видео?";
            string Path_toImage = Path.GetFullPath(Files[iter].FullName);
            this.video_for_user.Source = new Uri(Path_toImage, UriKind.Absolute);
        }

        /// <summary>
        /// Показывает картинку на экране 
        /// </summary>
        public void Create_Image_for_user() 
        {

          Question_for_user.Text ="Какой вы хотите ответ для этой картинки?";
            string Path_toImage = Path.GetFullPath(Files[iter].FullName);
            setImageSource(Path_toImage);
        }
        /// <summary>
        /// Нужно было так сделать чтобы картинку можно было потом удалить(иначе выводит что картинка используется другим приложением)
        /// </summary>
        /// <param name="file"></param>
        void setImageSource(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                 img_for_user.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }

        }
    }
}

