namespace ArtCritic
{
    public interface IMusicPlayer
    {
        void Open(string assetFileName);
        void Play();
        void Pause();
        void Stop();
    }
}
