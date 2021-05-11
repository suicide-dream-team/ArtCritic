using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtCritic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicQuestionPage : ContentPage
    {
        private int score = 0;
        private int iter = 0;
        private List<MusicQuestion> music_Questions = new List<MusicQuestion>();
        IMusicPlayer musicPlayer;
        public MusicQuestionPage()
        {
            InitializeComponent();
            Creat_Music();
            musicPlayer = DependencyService.Get<IMusicPlayer>();
            musicPlayer.Open(music_Questions[iter].questionFilename);
            musicPlayer.Play();
        }

        void Creat_Music()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ArtCritic.Data.Music.answers.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var e = reader.ReadLine();
                    var args = e.Split('|');
                    string[] ans = new string[1];
                    ans[0] = args[1];
                    music_Questions.Add(new MusicQuestion("Угадайте название песни", ans, args[0]));
                }
            }
        }
        async private void Music_accept_Click(object sender, EventArgs e)
        {
            musicPlayer.Stop();
            if (music_Questions[iter].Check_Answer(Music_answer.Text))
                score++;
                music_score.Text = score.ToString();
            Music_answer.Text = "";
            ++iter;
            if (iter == 4)
            {
                await DisplayAlert("Молодец!", "твой результат: " + score + "/" + music_Questions.Count, "OK");
                iter = 0;
            }
            else
            {
                musicPlayer.Open(music_Questions[iter].questionFilename);
                musicPlayer.Play();
            }
        }

        private void Music_replay_music_Click(object sender, EventArgs e)
        {
            musicPlayer.Open(music_Questions[iter].questionFilename);
            musicPlayer.Play();
        }

        protected override void OnDisappearing()
        {
            musicPlayer.Stop();
        }
    }
}