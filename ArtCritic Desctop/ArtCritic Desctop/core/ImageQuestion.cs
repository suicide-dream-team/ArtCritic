using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace ArtCritic_Desctop
{
    class ImageQuestion //: TextQuestion
    {
        public BitmapImage Picture { get; set; }


        public string Answer_for_Image { get; set; }

        



        public ImageQuestion(string text, string[] answers)// : base(text, answers)
        {


        }


    }

}
