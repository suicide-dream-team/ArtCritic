using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;


namespace ArtCritic_Desctop
{
    class Music_question : TextQuestion
    {
        MediaPlayer music;
        public Music_question(string text, string[] answers, Uri uriQestion) : base(text, answers)
        {            
            music = new MediaPlayer();
            music.Open(uriQestion);
            music.Play();
        }
        public void Play()
        {
            music.Play();
        }
        public void Pause()
        {
            music.Pause();
        }
        public void Stop()
        {
            music.Stop();
        }
    }
}
