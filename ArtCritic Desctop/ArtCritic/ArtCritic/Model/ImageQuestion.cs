using Xamarin.Forms;

namespace ArtCritic
{
    public class Image_Question
    {
        //Изображение для View
        public Image Picture { get; set; }

        //Ответ на картинку
        public string Answer { get; set; }

        public Image_Question(string PathImageSource, string AnswerSource)
        {
            this.Picture = new Image();
            this.Picture.Source = ImageSource.FromResource(PathImageSource);
            this.Answer = AnswerSource;
        }
    }
}