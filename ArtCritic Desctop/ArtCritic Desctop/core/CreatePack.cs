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

        public BitmapImage Picture { get; set; }

        public string Namepack;
        public int type_OF_create_game = 4;
        public int iter = 0;
        public int kol_vo_in_dir = 0;
        public bool is_create=false;
        public FileInfo[] ImageFiles;
        public TextBox Users_Answer;
        public TextBlock Question_for_user;
        public Image img_for_user;
  



        //выбирается путь//создаётся папка в которую пользователь в зависимости от выбора кидает картинки/видео/музыка //внутри папки создаётся файл с путями+ответами и переименовываются файлы чтобы было всё ОкAy// вся папка архивируется архив сохраняется



        public CreatePack(int type_of_game, string pack_name, Image image_for_create_user_pack, TextBox TB_For_Answer, TextBlock TBck_for_user)
        {
            //0 text game only questions and answers
            //1 music game

            //2 image game+
            //3 game_with_video+
            Users_Answer = TB_For_Answer;
            Namepack = pack_name;
            type_OF_create_game = type_of_game;
            img_for_user = image_for_create_user_pack;
            Question_for_user = TBck_for_user;
            if (type_of_game == 2)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(@"\..\..\..\PacksCreated\" + pack_name);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                // чтобы пользователь сам писал ответ для картинки // окно вывода+textblock+image
                System.Diagnostics.Process.Start("explorer", @"\..\..\..\PacksCreated\" + pack_name);

                MessageBoxResult messageBoxResult = MessageBox.Show("Скидывайте свои картинки(.jpg) в папку как закинете нажмите ok ", " Закинули? ", MessageBoxButton.OK);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    File.Create(@"\..\..\..\PacksCreated\" + pack_name + @"\answersI.txt");
                    FileInfo[] GraphicFiles = dirInfo.GetFiles("*.jpg");
                    ImageFiles = GraphicFiles;
                    kol_vo_in_dir = GraphicFiles.Length;
                }

                Create_Image_for_user();



            }
        }
        public void User_Create_CLick(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"\..\..\..\PacksCreated\" + Namepack);







            if (type_OF_create_game == 2)
            {

               

                   

                    string nameFile = ImageFiles[iter].Name;
                    
                    using (StreamWriter w = File.AppendText(@"\..\..\..\PacksCreated\" + Namepack + @"\answersI.txt"))
                    {
                        w.WriteLine(@"\..\..\..\Packs\" + Namepack + @"\" + nameFile + "|" + Users_Answer.Text);
                    }
                    iter++;
                if (iter < kol_vo_in_dir)
                {
                    Create_Image_for_user();
                }
                else
                {

                    //создание архива
                    ZipFile zf = new ZipFile(@"\..\..\..\PacksCreated\" + Namepack + ".zip");
                    zf.AddDirectory(@"\..\..\..\PacksCreated\" + Namepack);
                    zf.Save();

                    img_for_user.Source = null;

                    Directory.Delete(@"\..\..\..\PacksCreated\" + Namepack, true);

                    MessageBox.Show("Пак успешно создан");
                    is_create = true;
                }
            }


        }


        public void Create_Image_for_user() {

          Question_for_user.Text ="Какой вы хотите ответ для этой картинки?";
            this.Picture = new BitmapImage();
            this.Picture.BeginInit();
            string AbsolutePathBitmapImageSource = Path.GetFullPath(ImageFiles[iter].FullName);
            this.Picture.UriSource = new Uri(AbsolutePathBitmapImageSource, UriKind.RelativeOrAbsolute);
            this.Picture.EndInit();
            img_for_user.Source = Picture;

         

        }





    }
}

