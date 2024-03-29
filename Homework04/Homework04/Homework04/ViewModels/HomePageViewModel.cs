﻿using Homework04.Models;
using Homework04.Services;
using Homework04.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Homework04.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private bool notInternet { get; set; }
        public string InputCountry { get; set; }
        public CountryNews CountryNews { get; set; }
        public INewsApi ServiceApi { get; set; }

        public ICommand CommandGetNews { get; set; }
        public ICommand CommandGetInfo { get; set; }
        public HomePageViewModel()
        {
            notInternet = true;
            ServiceApi = new ApiService();
            CommandGetNews = new Command(async () =>
            {
                await GetArticles(InputCountry);
                if(notInternet == false)
                    await App.Current.MainPage.Navigation.PushAsync(new NewsDetailsPage(CountryNews.Articles));


            });

            CommandGetInfo = new Command(async () =>
            {
                string msj = $"\n" +
                $"US, BR, CA, CU, \n" +
                $"FR, IT, JP, PT, RU \n";

                await App.Current.MainPage.DisplayAlert("Introduce Country Code Like: ", msj, "Ok");
            });
        }
        async Task GetArticles(string country)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                CountryNews = await ServiceApi.GetNews(country);
                notInternet = false;
                //ImageLink = $"http://openweathermap.org/img/wn/{CityWeather.Weather[0].Icon}@2x.png";

            }
            else
            {
                notInternet = true;
                await App.Current.MainPage.DisplayAlert("Error", "Don't have internet connection, Try again...", "Ok");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
    }
}
