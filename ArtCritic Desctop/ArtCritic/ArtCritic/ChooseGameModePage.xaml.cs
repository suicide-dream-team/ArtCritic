﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArtCritic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseGameModePage : ContentPage
    {
        public ChooseGameModePage()
        {
            InitializeComponent();
        }
        async private void Video_question_Click(object sender, EventArgs e)
        {
            await DisplayAlert("Не работает", "Пока в разработке", "OK");
        }


        async private void Music_question_Click(object sender, EventArgs e)
        {
            await DisplayAlert("Не работает", "Пока в разработке", "OK");
        }

        async private void Mixed_questions_Click(object sender, EventArgs e)
        {
            await DisplayAlert("Не работает", "Пока в разработке", "OK");
        }

        async private void Image_question_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImageQuestionPage());
        }
    }
}