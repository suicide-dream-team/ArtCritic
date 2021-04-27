using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;//для директорий
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ionic.Zip;//зип прикол
using Microsoft.Win32;

namespace ArtCritic_Desctop.core
{
    class CreatePack
    {
        /// <summary>
        /// Имя пака 
        /// </summary>
        public string Namepack;
        /// <summary>
        /// тип созданной игры 5 потому что ещё не выбран тип
        /// </summary>
        public int type_OF_create_game = 5;
        //Type of games:
        //0 text game only questions and answers-
        //1 music game+
        //2 image game+
        //3 game_with_video+
        //4 mixed+
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
        /// главный массив для файлов хранит все кроме текстового файла
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
        public Image image_for_user;
        /// <summary>
        /// медиафайл для юзера
        /// </summary>
        public MediaElement video_for_user;
        /// <summary>
        /// Музыка для юзера
        /// </summary>
        public MediaPlayer music_for_user;
        /// <summary>
        /// Для показывания нужной формы в Mixed
        /// </summary>
        public bool is_image=false;
        /// <summary>
        /// Для показывания нужной формы в Mixed
        /// </summary>
        public bool is_video=false;
        /// <summary>
        /// Для показывания нужной формы в Mixed
        /// </summary>
        public bool is_music=false;
        /// <summary>
        /// Для проверки имени
        /// </summary>
        public bool norm_name = true;
        /// <summary>
        /// для проверок на битые файлы
        /// </summary>
        int temp_kol_vo_in_dir = 0;
        //создаётся папка в которую пользователь в зависимости от выбора кидает картинки/видео/музыку
        //внутри папки создаётся файл с путями+ответами 
        //вся папка архивируется архив сохраняется папка удаляется
        /// <summary>
        /// Создание mixed пака
        /// </summary>
        /// <param name="type_of_game"></param>
        /// <param name="pack_name"></param>
        /// <param name="video_for_create_user_pack"></param>
        /// <param name="TB_For_Answer"></param>
        /// <param name="TBck_for_user"></param>
        public CreatePack(int type_of_game, string pack_name, MediaElement video_for_create_user_pack, Image image_for_create_user_pack, MediaPlayer music_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user)
        {
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            Question_for_user = TBck_for_user;
            video_for_user = video_for_create_user_pack;
            image_for_user = image_for_create_user_pack;
            music_for_user = music_for_create_user_pack;
            DirectoryInfo dirInfo = new DirectoryInfo(@"\PacksCreated\" + pack_name);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            //открытие папки которую создал пользователь 
            System.Diagnostics.Process.Start("explorer", @"\PacksCreated\" + pack_name);
            MessageBoxResult messageBoxResult = MessageBox.Show("Скидывайте свои видео(.mp4), картинки(.jpg),музыку(.mp3) в папку как закинете нажмите ОК ", " Закинули? ", MessageBoxButton.OKCancel);
            //нажатие на ОК в мессдж боксе   
            if (messageBoxResult == MessageBoxResult.OK)
            {
                FileInfo[] Video_Files = dirInfo.GetFiles("*.mp4");
                kol_vo_in_dir += Video_Files.Length;
                FileInfo[] Music_Files = dirInfo.GetFiles("*.mp3");
                kol_vo_in_dir += Music_Files.Length;
                FileInfo[] Image_Files = dirInfo.GetFiles("*.jpg");
                kol_vo_in_dir += Image_Files.Length;
                FileInfo[] All_Files = dirInfo.GetFiles();
                Files = All_Files;
                if (kol_vo_in_dir == 0)
                {
                    while (kol_vo_in_dir == 0)
                    {
                        System.Diagnostics.Process.Start("explorer", @"\PacksCreated\" + pack_name);
                        MessageBoxResult messageBoxResult1 = MessageBox.Show("Не ну будь ты человеком закинь хотябы 1 файл (.mp4) и потом нажми ОК", " Закинули? ", MessageBoxButton.OK);
                        if (messageBoxResult1 == MessageBoxResult.OK)
                        {
                            Video_Files = dirInfo.GetFiles("*.mp4");
                            kol_vo_in_dir += Video_Files.Length;
                            Music_Files = dirInfo.GetFiles("*.mp3");
                            kol_vo_in_dir += Music_Files.Length;
                            Image_Files = dirInfo.GetFiles("*.jpg");
                            kol_vo_in_dir += Image_Files.Length;
                            All_Files = dirInfo.GetFiles();
                            Files = All_Files;
                        }
                        else { is_create = true; kol_vo_in_dir = -1; }
                    }
                }
                if (kol_vo_in_dir > 0) 
                {
                    foreach (var file in Directory.EnumerateFiles(@"\PacksCreated\" + pack_name, "*", SearchOption.TopDirectoryOnly))
                    {
                        int found = 0;
                        string expansion;
                        expansion = file;

                        if (Directory.Exists(expansion))
                        {
                            Directory.Delete(expansion);
                        }
                        else
                        {
                            found = expansion.IndexOf(".");
                            expansion = expansion[found..];
                            if (expansion == ".mp4" || expansion == ".mp3" || expansion == ".jpg") { }
                            else
                            { File.Delete(file); }
                        }
                    }
                    //txt файл создание
                    File.Create(@"\PacksCreated\" + pack_name + @"\answersM.txt").Close();
                }  
            }
            else { is_create = true; kol_vo_in_dir = -1; }
            Create_Mixed_for_user();
        }
        /// <summary>
        /// Создание видео пака
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
                MessageBoxResult messageBoxResult = MessageBox.Show("Скидывайте свои видео(.mp4) в папку как закинете нажмите ОК ", " Закинули? ", MessageBoxButton.OKCancel);
            //нажатие на ОК в мессдж боксе   
            if (messageBoxResult == MessageBoxResult.OK)
            {
                //выцепляем только mp4 
                FileInfo[] GraphicFiles = dirInfo.GetFiles("*.mp4");
                Files = GraphicFiles;
                kol_vo_in_dir = GraphicFiles.Length;
                //проверка на то что ни одного файла не скинули
                if (kol_vo_in_dir == 0)
                {
                    while (kol_vo_in_dir == 0)
                    {

                        System.Diagnostics.Process.Start("explorer", @"\PacksCreated\" + pack_name);
                        MessageBoxResult messageBoxResult1 = MessageBox.Show("Не ну будь ты человеком закинь хотябы 1 файл (.mp4) и потом нажми ОК", " Закинули? ", MessageBoxButton.OKCancel);
                        if (messageBoxResult1 == MessageBoxResult.OK)
                        {
                            GraphicFiles = dirInfo.GetFiles("*.mp4");
                            Files = GraphicFiles;
                            kol_vo_in_dir = GraphicFiles.Length;
                        }
                        else
                        {
                            is_create = true;
                            kol_vo_in_dir = -1;
                        }
                    }
                }
                if (kol_vo_in_dir > 0)
                {
                    //проверка что скинули всё нужное и если ненужное то удаляем нахер всё
                    foreach (var file in Directory.EnumerateFiles(@"\PacksCreated\" + pack_name, "*", SearchOption.TopDirectoryOnly))
                    {
                        int found = 0;
                        string expansion;
                        expansion = file;

                        if (Directory.Exists(expansion))
                        {
                            Directory.Delete(expansion);
                        }
                        else
                        {
                            found = expansion.IndexOf(".");
                            expansion = expansion[found..];
                            if (expansion != ".mp4") { File.Delete(file); }
                        }
                    }
                    //txt файл создание
                    File.Create(@"\PacksCreated\" + pack_name + @"\answersV.txt").Close();
                }
            }
            else { is_create = true; kol_vo_in_dir = -1; }
            string Path_toVideo = Path.GetFullPath(Files[iter].FullName);
            Create_Video_for_user(Path_toVideo);
        }
        /// <summary>
        /// Создание картиночного пака
        /// </summary>
        /// <param name="type_of_game"></param>
        /// <param name="pack_name"></param>
        /// <param name="image_for_create_user_pack"></param>
        /// <param name="TB_For_Answer"></param>
        /// <param name="TBck_for_user"></param>
        public CreatePack(int type_of_game, string pack_name, Image image_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user)
        {
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            image_for_user = image_for_create_user_pack;
            Question_for_user = TBck_for_user;
            DirectoryInfo dirInfo = new DirectoryInfo(@"\PacksCreated\" + pack_name);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            System.Diagnostics.Process.Start("explorer", @"\PacksCreated\" + pack_name);
            MessageBoxResult messageBoxResult = MessageBox.Show("Скидывайте свои картинки(.jpg) в папку как закинете нажмите ok ", " Закинули? ", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                FileInfo[] GraphicFiles = dirInfo.GetFiles("*.jpg");
                Files = GraphicFiles;
                kol_vo_in_dir = GraphicFiles.Length;
                //проверка на то что ни одного файла не скинули
                if (kol_vo_in_dir == 0)
                {
                    while (kol_vo_in_dir == 0)
                    {
                        System.Diagnostics.Process.Start("explorer", @"\PacksCreated\" + pack_name);
                        MessageBoxResult messageBoxResult1 = MessageBox.Show("Не ну будь ты человеком закинь хотябы 1 файл (.jpg) и потом нажми ОК", " Закинули? ", MessageBoxButton.OKCancel);
                        if (messageBoxResult1 == MessageBoxResult.OK)
                        {
                            GraphicFiles = dirInfo.GetFiles("*.jpg");
                            Files = GraphicFiles;
                            kol_vo_in_dir = GraphicFiles.Length;
                        }
                        else { kol_vo_in_dir = -1; is_create = true; }
                    }
                }
                //проверка что скинули всё нужное и если ненужное то удаляем нахер всё
                foreach (var file in Directory.EnumerateFiles(@"\PacksCreated\" + pack_name, "*", SearchOption.TopDirectoryOnly))
                {
                    int found = 0;
                    string expansion;
                    expansion = file;
                    if (Directory.Exists(expansion))
                    {
                        Directory.Delete(expansion);
                    }
                    else
                    {
                        found = expansion.IndexOf(".");
                        expansion = expansion[found..];
                        if (expansion != ".jpg") { File.Delete(file); }
                    }
                }
                File.Create(@"\PacksCreated\" + pack_name + @"\answersI.txt").Close();
            }
            else
            {
                kol_vo_in_dir = -1;
                is_create = true;
            }
            Create_Image_for_user();
        }
        /// <summary>
        /// Создание музыкального пака
        /// </summary>
        /// <param name="type_of_game"></param>
        /// <param name="pack_name"></param>
        /// <param name="music_for_create_user_pack"></param>
        /// <param name="TB_For_Answer"></param>
        /// <param name="TBck_for_user"></param>
        public CreatePack(int type_of_game, string pack_name, MediaPlayer music_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user) 
        {
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            music_for_user = music_for_create_user_pack;
            Question_for_user = TBck_for_user;
            DirectoryInfo dirInfo = new DirectoryInfo(@"\PacksCreated\" + pack_name);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            System.Diagnostics.Process.Start("explorer", @"\PacksCreated\" + pack_name);
            MessageBoxResult messageBoxResult = MessageBox.Show("Скидывайте свою музыку (.mp3) в папку как закинете нажмите ok ", " Закинули? ", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                FileInfo[] GraphicFiles = dirInfo.GetFiles("*.mp3");
                Files = GraphicFiles;
                kol_vo_in_dir = GraphicFiles.Length;
                //проверка на то что ни одного файла не скинули
                if (kol_vo_in_dir == 0)
                {
                    while (kol_vo_in_dir == 0)
                    {
                        System.Diagnostics.Process.Start("explorer", @"\PacksCreated\" + pack_name);
                        MessageBoxResult messageBoxResult1 = MessageBox.Show("Не ну будь ты человеком закинь хотябы 1 файл (.mp3) и потом нажми ОК", " Закинули? ", MessageBoxButton.OKCancel);
                        if (messageBoxResult1 == MessageBoxResult.OK)
                        {
                            GraphicFiles = dirInfo.GetFiles("*.mp3");
                            Files = GraphicFiles;
                            kol_vo_in_dir = GraphicFiles.Length;
                        }
                        else { kol_vo_in_dir = -1; is_create = true; }
                    }
                }
                //проверка что скинули всё нужное и если ненужное то удаляем нахер всё
                foreach (var file in Directory.EnumerateFiles(@"\PacksCreated\" + pack_name, "*", SearchOption.TopDirectoryOnly))
                {
                    int found = 0;
                    string expansion;
                    expansion = file;
                    if (Directory.Exists(expansion))
                    {
                        Directory.Delete(expansion);
                    }
                    else
                    {
                        found = expansion.IndexOf(".");
                        expansion = expansion[found..];
                        if (expansion != ".mp3") { File.Delete(file); }
                    }
                }
                File.Create(@"\PacksCreated\" + pack_name + @"\answersMU.txt").Close();
            }
            else
            {
                kol_vo_in_dir = -1;
                is_create = true;
            }
            string Path_toMusic = Path.GetFullPath(Files[iter].FullName);
            Create_Music_for_user(Path_toMusic);
        }
        /// <summary>
    /// проверяет видео на ломанность
    /// </summary>
    /// <param name="video"></param>
    /// <summary>
    /// Создание Image пака
    /// </summary>
    /// <param name="type_of_game"></param>
    /// <param name="pack_name"></param>
    /// <param name="image_for_create_user_pack"></param>
    /// <param name="TB_For_Answer"></param>
    /// <param name="TBck_for_user"></param>
        public void Check_video(MediaElement video) 
        {

            if (this.type_OF_create_game == 3)
            {

                if ( IsPlaying(this.video_for_user))
                {
                    //Всё заебумбно ничего не делаем
                }
                else
                {
                    File.Delete(Files[iter].FullName);
                    iter++;
                    temp_kol_vo_in_dir = kol_vo_in_dir;
                    temp_kol_vo_in_dir--;
                    if (temp_kol_vo_in_dir >= 1 && iter < kol_vo_in_dir)
                    {
                        string Path_toVideo = Path.GetFullPath(Files[iter].FullName);
                        Create_Video_for_user(Path_toVideo);
                    }
                        if (temp_kol_vo_in_dir < 1)
                    {
                        is_create = true;
                        MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later! Пересоздай пак.");
                    }
                    if (iter == kol_vo_in_dir)
                    {
                        //значит удалили последний в списке повреждённый файл
                    }
                }
            }
            if (this.type_OF_create_game == 4)
            {
                if (IsPlaying(this.video_for_user))
                {
                    //Всё заебумбно ничего не делаем
                }
                else
                {
                    File.Delete(Files[iter].FullName);
                    iter++;
                    temp_kol_vo_in_dir = kol_vo_in_dir;
                    temp_kol_vo_in_dir--;
                    if (temp_kol_vo_in_dir >= 1 && iter < kol_vo_in_dir)
                    {
                        Create_Mixed_for_user();
                    }
                    if (temp_kol_vo_in_dir < 1)
                    {
                        is_create = true;
                        MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later! Пересоздай пак.");
                    }
                    if (iter == kol_vo_in_dir)
                    {
                        //значит удалили последний в списке повреждённый файл
                    }
                }
            }
        }
        /// <summary>
        /// Проверка музыки на воспроизведение
        /// </summary>
        /// <param name="music"></param>
        public void Check_music(MediaPlayer music) 
        {
            //this.music_for_user.Stop();
            if (this.type_OF_create_game == 1)
            {

                if (IsPlaying(this.music_for_user))
                {
                    //Всё заебумбно ничего не делаем
                }
                else
                {
                    File.Delete(Files[iter].FullName);
                    iter++;
                    temp_kol_vo_in_dir = kol_vo_in_dir;
                    temp_kol_vo_in_dir--;
                    if (temp_kol_vo_in_dir >= 1 && iter < kol_vo_in_dir)
                    {
                        string Path_toVideo = Path.GetFullPath(Files[iter].FullName);
                        Create_Music_for_user(Path_toVideo);
                    }
                    if (temp_kol_vo_in_dir < 1)
                    {
                        is_create = true;
                        MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later! Пересоздай пак.");
                    }
                    if (iter == kol_vo_in_dir)
                    {
                        //значит удалили последний в списке повреждённый файл
                    }
                }
            }
            if (this.type_OF_create_game == 4)
            {
                if (IsPlaying(this.music_for_user))
                {
                    //Всё заебумбно ничего не делаем
                }
                else
                {
                    File.Delete(Files[iter].FullName);
                    iter++;
                    temp_kol_vo_in_dir = kol_vo_in_dir;
                    temp_kol_vo_in_dir--;
                    if (temp_kol_vo_in_dir >= 1 && iter < kol_vo_in_dir)
                    {
                        Create_Mixed_for_user();
                    }
                    if (temp_kol_vo_in_dir < 1)
                    {
                        is_create = true;
                        MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later! Пересоздай пак.");
                    }
                    if (iter == kol_vo_in_dir)
                    {
                        //значит удалили последний в списке повреждённый файл
                    }
                }
            }

        }
        /// <summary>
        /// корректно ли запустилась музыка
        /// </summary>
        /// <param name="music"></param>
        /// <returns></returns>
        bool IsPlaying(MediaPlayer music_play)
        {
            var pos1 = music_play.Position;
            System.Threading.Thread.Sleep(1000);
            var pos2 = music_play.Position;
            return pos2 != pos1;
        }
        /// <summary>
        /// корректно ли запустилось видео
        /// </summary>
        /// <param name="video_play"></param>
        /// <returns></returns>
        bool IsPlaying(MediaElement video_play)
        {
            var pos1 = video_play.Position;
            System.Threading.Thread.Sleep(1000);
            var pos2 = video_play.Position;
            return pos2 != pos1;
        }
        /// <summary>
        /// Включает музыку на экране
        /// </summary>
        /// <param name="Path_toMusic"></param>
        public void Create_Music_for_user(string Path_toMusic) 
        {
            if (kol_vo_in_dir > 0)
            {
                Question_for_user.Text = "Какой вы хотите ответ для этой музыки?";
                Uri path_to_music = new Uri(Path_toMusic, UriKind.Absolute);
                this.music_for_user.Open(path_to_music);
                this.music_for_user.Play();
                Check_music(this.music_for_user);
            }
            else { }
        }
        /// <summary>
        /// Показывает видео на экране
        /// </summary>
        public void Create_Video_for_user(string Path_toVideo)
        {
            if (kol_vo_in_dir > 0)
            {
                Question_for_user.Text = "Какой вы хотите ответ для этого видео?";
                this.video_for_user.Source = new Uri(Path_toVideo, UriKind.Absolute);
                Check_video(this.video_for_user);
            }
            else { }
        }
        /// <summary>
        /// Показывает картинку на экране 
        /// </summary>
        public void Create_Image_for_user() 
        {
            if (kol_vo_in_dir > 0)
            {
                Question_for_user.Text = "Какой вы хотите ответ для этой картинки?";
                string Path_toImage = Path.GetFullPath(Files[iter].FullName);
                setImageSource(Path_toImage);
            }
        }
        /// <summary>
        /// создаёт поток для картинки потому что если её просто поместить в BitmapImage то картинка открывается и не может закрыться
        /// </summary>
        /// <param name="file"></param>
        void setImageSource(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                try
                {
                    image_for_user.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
                catch (System.IO.FileFormatException)
                {
                   // image_for_user.Source = null;
                    stream.Close();
                    File.Delete(file);
                    iter++;
                    temp_kol_vo_in_dir = kol_vo_in_dir;
                    temp_kol_vo_in_dir--;
                    if (temp_kol_vo_in_dir >= 1 && iter < kol_vo_in_dir)
                    {
                        string Path_toImage = Path.GetFullPath(Files[iter].FullName);
                        setImageSource(Path_toImage);
                    }
                    if (iter == kol_vo_in_dir)
                    {
                        //если мы удалили последний элемент всё на чилле тогда
                    }
                    if (temp_kol_vo_in_dir < 1)
                    {
                        is_create = true;
                        MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later! Пересоздай пак.");
                    }
                }
                catch (System.NotSupportedException)
                {
                    image_for_user.Source = null;
                    stream.Close();
                    File.Delete(file);
                    iter++;
                    temp_kol_vo_in_dir = kol_vo_in_dir;
                    temp_kol_vo_in_dir--;
                    if (temp_kol_vo_in_dir >= 1 && iter < kol_vo_in_dir)
                    {
                        string Path_toImage = Path.GetFullPath(Files[iter].FullName);
                        setImageSource(Path_toImage);
                    }
                    if (iter == kol_vo_in_dir)
                    {
                        //если мы удалили последний элемент всё на чилле тогда
                    }
                    if (temp_kol_vo_in_dir < 1)
                    {
                        is_create = true;
                        MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later! Пересоздай пак.");
                    }
                }
            }
        }
        /// <summary>
        /// воспроизводит элемент из mixed на экране
        /// </summary>
        public void Create_Mixed_for_user()
        {
            if (kol_vo_in_dir > 0)
            {
                string Path_toFile = Path.GetFullPath(Files[iter].FullName);
                int found = 0;
                string expansion;
                expansion = Path.GetFullPath(Files[iter].FullName);
                found = expansion.IndexOf(".");
                expansion = expansion[found..];
                if (expansion == ".mp4")
                {
                    is_image = false;
                    is_video = true;
                    is_music = false;
                    Question_for_user.Text = "Какой вы хотите ответ для этого видео?";
                    this.video_for_user.Source = new Uri(Path_toFile, UriKind.Absolute);
                    Check_video(this.video_for_user);
                }
                if (expansion == ".mp3")
                {
                    is_image = false;
                    is_video = false;
                    is_music = true;
                    Question_for_user.Text = "Какой вы хотите ответ для этой музыки?";
                    string path_tomusic = Files[iter].FullName;
                    Create_Music_for_user(path_tomusic);
                }
                if (expansion == ".jpg")
                {
                    is_image = true;
                    is_video = false;
                    is_music = false;
                    Question_for_user.Text = "Какой вы хотите ответ для этой картинки?";
                    setImageSource(Path_toFile);
                }
            }
        }
        /// <summary>
        /// Создание архива
        /// </summary>
        public void Create_zip_archieve() 
        {
            if (File.Exists(@"\PacksCreated\" + Namepack + ".zip"))
            {
                File.Delete(@"\PacksCreated\" + Namepack + ".zip");
                ZipFile zf = new ZipFile(@"\PacksCreated\" + Namepack + ".zip");
                zf.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zf.AddDirectory(@"\PacksCreated\" + Namepack);
                zf.Save();
                Directory.Delete(@"\PacksCreated\" + Namepack, true);
                MessageBox.Show("Пак успешно создан");
                is_create = true;
            }
            else
            {
                ZipFile zf = new ZipFile(@"\PacksCreated\" + Namepack + ".zip");
                zf.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zf.AddDirectory(@"\PacksCreated\" + Namepack);
                zf.Save();
                Directory.Delete(@"\PacksCreated\" + Namepack, true);
                MessageBox.Show("Пак успешно создан");
                is_create = true;
            }
        }
        /// <summary>
        /// Удаляет папку пака со всем содержимым при резком выходе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Delete_Naher(object sender, CancelEventArgs e)
        {
            if (Directory.Exists(@"\PacksCreated\" + Namepack))
            {
                Directory.Delete(@"\PacksCreated\" + Namepack, true);
            }
        }
        /// <summary>
        /// Само нажатие пользователя на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void User_Create_CLick(object sender, RoutedEventArgs e)
        {
            //    DirectoryInfo dirInfo = new DirectoryInfo(@"\PacksCreated\" + Namepack);
            if (is_create == true) { }
            else
            {
                if (type_OF_create_game == 1)
                {
                    string nameFile = Files[iter].Name;
                    using (StreamWriter w = File.AppendText(@"\PacksCreated\" + Namepack + @"\answersMU.txt"))
                    {
                        w.WriteLine(@"..\..\..\Packs\" + Namepack + @"\" + nameFile + "|" + Users_Answer.Text);
                    }
                    iter++;
                    if (iter < kol_vo_in_dir)
                    {
                        music_for_user.Stop();
                        music_for_user.Close();
                        string Path_toVideo = Path.GetFullPath(Files[iter].FullName);
                        Create_Music_for_user(Path_toVideo);
                    }
                    else
                    {
                        Create_zip_archieve();
                    }
                }
                if (type_OF_create_game == 3)
                {
                    string nameFile = Files[iter].Name;
                    using (StreamWriter w = File.AppendText(@"\PacksCreated\" + Namepack + @"\answersV.txt"))
                    {
                        w.WriteLine(@"..\..\..\Packs\" + Namepack + @"\" + nameFile + "|" + Users_Answer.Text);
                    }
                    iter++;
                    if (iter < kol_vo_in_dir)
                    {
                        string Path_toVideo = Path.GetFullPath(Files[iter].FullName);
                        Create_Video_for_user(Path_toVideo);
                    }
                    else
                    {
                        Create_zip_archieve();
                    }
                }
                if (type_OF_create_game == 2)
                {
                    string nameFile = Files[iter].Name;
                    using (StreamWriter w = File.AppendText(@"\PacksCreated\" + Namepack + @"\answersI.txt"))
                    {
                        w.WriteLine(@"..\..\..\Packs\" + Namepack + @"\" + nameFile + "|" + Users_Answer.Text);
                    }
                    iter++;
                    if (iter < kol_vo_in_dir)
                    {
                        Create_Image_for_user();
                    }
                    else
                    {
                        Create_zip_archieve();
                    }
                }
                if (type_OF_create_game == 4)
                {
                    string nameFile = Files[iter].Name;
                    using (StreamWriter w = File.AppendText(@"\PacksCreated\" + Namepack + @"\answersM.txt"))
                    {
                        w.WriteLine(@"..\..\..\Packs\" + Namepack + @"\" + nameFile + "|" + Users_Answer.Text);
                    }
                    iter++;
                    if (iter < kol_vo_in_dir)
                    {
                        music_for_user.Stop();
                        music_for_user.Close();
                        Create_Mixed_for_user();
                    }
                    else
                    {
                        Create_zip_archieve();
                    }
                }
            }
        }
    }
}

