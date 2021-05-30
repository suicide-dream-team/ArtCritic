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
//using Ionic.Zip;//зип прикол
using ArtCritic_Desctop.core.db;
using Microsoft.Win32;

namespace ArtCritic_Desctop.core
{
    class CreatePack
    {

        /// Имя файла в паке

        public string nameFile;

        /// Пак для бд

        Pack p = new Pack();

        /// Путь к нашей рабочей папке

        string path = Directory.GetCurrentDirectory();

        /// Имя пака 

        public string Namepack;

        /// тип созданной игры 5 потому что ещё не выбран тип

        public int type_OF_create_game = 5;
        //Type of games:
        //0 text game only questions and answers-
        //1 music game+
        //2 image game+
        //3 game_with_video+
        //4 mixed+

        /// итератор для прохода под массиву файлов

        public int iter = 0;

        /// количество файлов в папке кроме текстового
 
        public int kol_vo_in_dir = 0;

        /// Создался пак или нет(чтобы закрыть окно)

        public bool is_create = false;

        /// главный массив для файлов хранит все кроме текстового файла

        public FileInfo[] Files;

        /// Текстбокс для Юзера чтобы он вводил имя/ответ на картинку

        public TextBox Users_Answer;

        /// Текстблок для вывода вопроса для юзера(Какое имя пака/ответ для этой картинки)

        public TextBlock Question_for_user;

        /// Картинка для юзера

        public Image image_for_user;

        /// медиафайл для юзера

        public MediaElement video_for_user;

        /// Музыка для юзера

        public MediaPlayer music_for_user;

        /// Для показывания нужной формы в Mixed

        public bool is_image = false;

        /// Для показывания нужной формы в Mixed

        public bool is_video = false;

        /// Для показывания нужной формы в Mixed

        public bool is_music = false;

        /// Для проверки имени

        public bool norm_name = true;

        /// для проверок на битые файлы

        int temp_kol_vo_in_dir = 0;
        //создаётся папка в которую пользователь в зависимости от выбора кидает картинки/видео/музыку
        //внутри папки создаётся файл с путями+ответами 
        //вся папка архивируется архив сохраняется папка удаляется

        /// Создание mixed пака

        public CreatePack(int type_of_game, string pack_name, MediaElement video_for_create_user_pack, Image image_for_create_user_pack, MediaPlayer music_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user)
        {
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            Question_for_user = TBck_for_user;
            video_for_user = video_for_create_user_pack;
            image_for_user = image_for_create_user_pack;
            music_for_user = music_for_create_user_pack;
            p.Name = pack_name;
            DirectoryInfo dirInfo = new DirectoryInfo(path + @"\Packs\" + pack_name);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            p.Path = "dirInfo";
            p.Type = Question.QuestionType.Mixed;

            //открытие папки которую создал пользователь 
            System.Diagnostics.Process.Start("explorer", path + @"\Packs\" + pack_name);
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
                        System.Diagnostics.Process.Start("explorer", path + @"\Packs\" + pack_name);
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
                    foreach (var file in Directory.EnumerateFiles(path + @"\Packs\" + pack_name, " * ", SearchOption.TopDirectoryOnly))
                    {
                        string expansion;
                        expansion = file;

                        if (Directory.Exists(expansion))
                        {
                            Directory.Delete(expansion);
                        }
                        else
                        {
                            expansion = expansion.Substring(expansion.Length - 4);
                            if (expansion == ".mp4" || expansion == ".mp3" || expansion == ".jpg") { }
                            else
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    //txt файл создание
                    //File.Create(path + @"\Packs\" + pack_name + @"\answersM.txt").Close();
                }
            }
            else { is_create = true; kol_vo_in_dir = -1; }
            Create_Mixed_for_user();
        }

        /// Создание видео пака
        public CreatePack(int type_of_game, string pack_name, MediaElement video_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user)
        {
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            Question_for_user = TBck_for_user;
            video_for_user = video_for_create_user_pack;
            p.Name = pack_name;
            DirectoryInfo dirInfo = new DirectoryInfo(path + @"\Packs\" + pack_name);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            //открытие папки которую создал пользователь 
            System.Diagnostics.Process.Start("explorer", path + @"\Packs\" + pack_name);
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

                        System.Diagnostics.Process.Start("explorer", path + @"\Packs\" + pack_name);
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
                    foreach (var file in Directory.EnumerateFiles(path + @"\Packs\" + pack_name, " * ", SearchOption.TopDirectoryOnly))
                    {
                        string expansion;
                        expansion = file;

                        if (Directory.Exists(expansion))
                        {
                            Directory.Delete(expansion);
                        }
                        else
                        {
                            expansion = expansion.Substring(expansion.Length - 4);
                            if (expansion != ".mp4") { File.Delete(file); }
                        }
                    }
                    //txt файл создание
                    //File.Create(path + @"\Packs\" + pack_name + @"\answersV.txt").Close();
                }
            }
            else { is_create = true; kol_vo_in_dir = -1; }
            string Path_toVideo = Path.GetFullPath(Files[iter].FullName);
            Create_Video_for_user(Path_toVideo);
        }

        /// Создание картиночного пака
        public CreatePack(int type_of_game, string pack_name, Image image_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user)
        {
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            image_for_user = image_for_create_user_pack;
            Question_for_user = TBck_for_user;
            p.Name = pack_name;
            DirectoryInfo dirInfo = new DirectoryInfo(path + @"\Packs\" + pack_name);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            System.Diagnostics.Process.Start("explorer", path + @"\Packs\" + pack_name);
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
                        System.Diagnostics.Process.Start("explorer", path + @"\Packs\" + pack_name);
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
                foreach (var file in Directory.EnumerateFiles(path + @"\Packs\" + pack_name, " * ", SearchOption.TopDirectoryOnly))
                {
                    string expansion;
                    expansion = file;
                    if (File.Exists(expansion))
                    {
                        expansion = expansion.Substring(expansion.Length - 4);
                        if (expansion != ".jpg") { File.Delete(file); }
                    }
                }
                //File.Create(path + @"\Packs\" + pack_name + @"\answersI.txt").Close();
            }
            else
            {
                kol_vo_in_dir = -1;
                is_create = true;
            }
            Create_Image_for_user();
        }

        /// Создание музыкального пака
        public CreatePack(int type_of_game, string pack_name, MediaPlayer music_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user)
        {
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            music_for_user = music_for_create_user_pack;
            Question_for_user = TBck_for_user;
            p.Name = pack_name;
            DirectoryInfo dirInfo = new DirectoryInfo(path + @"\Packs\" + pack_name);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            System.Diagnostics.Process.Start("explorer", path + @"\Packs\" + pack_name);
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
                        System.Diagnostics.Process.Start("explorer", path + @"\Packs\" + pack_name);
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
                foreach (var file in Directory.EnumerateFiles(path + @"\Packs\" + pack_name, " * ", SearchOption.TopDirectoryOnly))
                {
                    string expansion;
                    expansion = file;
                    if (Directory.Exists(expansion))
                    {
                        Directory.Delete(expansion);
                    }
                    else
                    {
                        expansion = expansion.Substring(expansion.Length - 4);
                        if (expansion != ".mp3") { File.Delete(file); }
                    }
                }
                //File.Create(path + @"\Packs\" + pack_name + @"\answersMU.txt").Close();
            }
            else
            {
                kol_vo_in_dir = -1;
                is_create = true;
            }
            string Path_toMusic = Path.GetFullPath(Files[iter].FullName);
            Create_Music_for_user(Path_toMusic);
        }

        /// проверяет видео на ломанность
        public void Check_video(MediaElement video)
        {

            if (this.type_OF_create_game == 3)
            {

                if (IsPlaying(this.video_for_user))
                {
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
                    Question q = new Question(p, 4, "", Users_Answer.Text, nameFile);
                    q = QuestionDao.Add(q);
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

        /// Проверка музыки на воспроизведение

        public void Check_music(MediaPlayer music)
        {
            //this.music_for_user.Stop();
            if (this.type_OF_create_game == 1)
            {

                if (IsPlaying(this.music_for_user))
                {
                    Question q = new Question(p, 3, "", Users_Answer.Text, nameFile);
                    q = QuestionDao.Add(q);
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
                    Question q = new Question(p, 3, "", Users_Answer.Text, nameFile);
                    q = QuestionDao.Add(q);
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

        /// корректно ли запустилась музыка

        bool IsPlaying(MediaPlayer music_play)
        {
            var pos1 = music_play.Position;
            System.Threading.Thread.Sleep(1000);
            var pos2 = music_play.Position;
            return pos2 != pos1;
        }

        /// корректно ли запустилось видео

        bool IsPlaying(MediaElement video_play)
        {
            var pos1 = video_play.Position;
            System.Threading.Thread.Sleep(1000);
            var pos2 = video_play.Position;
            return pos2 != pos1;
        }

        /// Включает музыку на экране

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

        /// Показывает видео на экране

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

        /// Показывает картинку на экране 

        public void Create_Image_for_user()
        {
            if (kol_vo_in_dir > 0)
            {
                Question_for_user.Text = "Какой вы хотите ответ для этой картинки?";
                string Path_toImage = Path.GetFullPath(Files[iter].FullName);
                setImageSource(Path_toImage);
            }
        }

        /// создаёт поток для картинки потому что если её просто поместить в BitmapImage то картинка открывается и не может закрыться

        void setImageSource(string file)
        {
            if (type_OF_create_game == 2)
            {
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        image_for_user.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                    catch (System.IO.FileFormatException)
                    {
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
                            MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later before upload the files! Пересоздай пак.");
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
                            MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later before upload the files! Пересоздай пак.");
                        }
                    }
                }
            }
            if (type_OF_create_game == 4)
            {
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        image_for_user.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                    catch (System.IO.FileFormatException)
                    {
                        stream.Close();
                        File.Delete(file);
                        iter++;
                        temp_kol_vo_in_dir = kol_vo_in_dir;
                        temp_kol_vo_in_dir--;
                        if (temp_kol_vo_in_dir >= 1 && iter < kol_vo_in_dir)
                        {
                            Create_Mixed_for_user();
                        }
                        if (iter == kol_vo_in_dir)
                        {
                            //если мы удалили последний элемент всё на чилле тогда
                        }
                        if (temp_kol_vo_in_dir < 1)
                        {
                            is_create = true;
                            MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later before upload the files! Пересоздай пак.");
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
                            Create_Mixed_for_user();
                        }
                        if (iter == kol_vo_in_dir)
                        {
                            //если мы удалили последний элемент всё на чилле тогда
                        }
                        if (temp_kol_vo_in_dir < 1)
                        {
                            is_create = true;
                            MessageBox.Show("я удалил все повреждённые файлы, и папка кончилась. Check later before upload the files! Пересоздай пак.");
                        }
                    }
                }
                Question q = new Question(p, 2, "", Users_Answer.Text, nameFile);
                q = QuestionDao.Add(q);
            }

        }
        /// воспроизводит элемент из mixed на экране
        public void Create_Mixed_for_user()
        {
            if (kol_vo_in_dir > 0)
            {
                string Path_toFile = Path.GetFullPath(Files[iter].FullName);
                string expansion;
                expansion = Path.GetFullPath(Files[iter].FullName);
                expansion = expansion.Substring(expansion.Length - 4);
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
        /// Создание архива
        /*   public void Create_zip_archieve()
           {
               if (File.Exists(path + @"\Packs\" + Namepack + ".zip"))
               {
                   File.Delete(path + @"\Packs\" + Namepack + ".zip");
                   ZipFile zf = new ZipFile(path + @"\Packs\" + Namepack + ".zip");
                   zf.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                   zf.AddDirectory(path + @"\Packs\" + Namepack);
                   zf.Save();
                   Directory.Delete(path + @"\Packs\" + Namepack, true);
                   MessageBox.Show("Пак успешно создан");
                   is_create = true;
               }
               else
               {
                   ZipFile zf = new ZipFile(path + @"\Packs\" + Namepack + ".zip");
                   zf.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                   zf.AddDirectory(path + @"\Packs\" + Namepack);
                   zf.Save();
                   Directory.Delete(path + @"\Packs\" + Namepack, true);
                   MessageBox.Show("Пак успешно создан");
                   is_create = true;
               }
           }
        */

        /// Удаляет папку пака со всем содержимым при резком выходе
        /* public void Delete_Naher(object sender, CancelEventArgs e)
         {
             if (Directory.Exists(path + @"\Packs\" + Namepack))
             {
                 Directory.Delete(path + @"\Packs\" + Namepack, true);
             }
         }
        */
        /// Само нажатие пользователя на кнопку
        public void User_Create_CLick(object sender, RoutedEventArgs e)
        {
            if (is_create == true) { }
            else
            {
                if (type_OF_create_game == 1)
                {
                    nameFile = Files[iter].Name;
                   // using (StreamWriter w = File.AppendText(path + @"\Packs\" + Namepack + @"\answersMU.txt"))
                    //{
                      //  w.WriteLine(@"\Packs\" + Namepack + @"\" + nameFile + " | " + Users_Answer.Text);
                        Question q = new Question(p, 3, "", Users_Answer.Text, nameFile);
                        q = QuestionDao.Add(q);
                    //}
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
                        p = PackDao.Add(p);
                        MessageBox.Show("Пак успешно создан");
                        is_create = true;
                        //Create_zip_archieve();
                    }
                }
                if (type_OF_create_game == 3)
                {
                    nameFile = Files[iter].Name;
                    //   using (StreamWriter w = File.AppendText(path + @"\Packs\" + Namepack + @"\answersV.txt"))
                    // {
                    //   w.WriteLine(@"\Packs\" + Namepack + @"\" + nameFile + " | " + Users_Answer.Text);
                    // }
                    Question q = new Question(p, 4, "", Users_Answer.Text, nameFile);
                    q = QuestionDao.Add(q);
                    iter++;
                    if (iter < kol_vo_in_dir)
                    {
                        string Path_toVideo = Path.GetFullPath(Files[iter].FullName);
                        Create_Video_for_user(Path_toVideo);
                    }
                    else
                    {
                        //Create_zip_archieve();
                        p = PackDao.Add(p);
                        is_create = true;
                    }
                }
                if (type_OF_create_game == 2)
                {
                    nameFile = Files[iter].Name;
                    // using (StreamWriter w = File.AppendText(path + @"\Packs\" + Namepack + @"\answersI.txt"))
                    //{
                    //   w.WriteLine(@"\Packs\" + Namepack + @"\" + nameFile + " | " + Users_Answer.Text);
                    //}
                    Question q = new Question(p, 2, "", Users_Answer.Text, nameFile);
                    q = QuestionDao.Add(q);
                    iter++;
                    if (iter < kol_vo_in_dir)
                    {
                        Create_Image_for_user();
                    }
                    else
                    {
                        p = PackDao.Add(p);
                        is_create = true;
                        //Create_zip_archieve();
                    }
                }
                if (type_OF_create_game == 4)
                {
                    nameFile = Files[iter].Name;
                    //  using (StreamWriter w = File.AppendText(path + @"\Packs\" + Namepack + @"\answersM.txt"))
                    // {
                    //    w.WriteLine(@"\Packs\" + Namepack + @"\" + nameFile + " | " + Users_Answer.Text);
                    //}
                    iter++;
                    if (iter < kol_vo_in_dir)
                    {
                        music_for_user.Stop();
                        music_for_user.Close();
                        video_for_user.Close();
                        Create_Mixed_for_user();
                    }
                    else
                    {
                        p = PackDao.Add(p);
                        is_create = true;
                        //Create_zip_archieve();
                    }
                }
            }
        }
    }
}


