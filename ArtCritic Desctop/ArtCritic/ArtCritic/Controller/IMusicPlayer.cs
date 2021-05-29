﻿using System;
using System.Collections.Generic;
using System.Text;

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