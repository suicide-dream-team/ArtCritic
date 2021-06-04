using Xamarin.Forms;

namespace ArtCritic
{
    public class ImageQuestion : TextQuestion
    {
        //Изображение для View
        public Image Picture { get; set; }

        public ImageQuestion(string pathToImageSource, string answerSource) : base("", answerSource)
        {
            this.Picture = new Image();
            this.Picture.Source = ImageSource.FromResource(pathToImageSource);
        }
    }
}