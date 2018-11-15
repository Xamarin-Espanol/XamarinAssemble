using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinAssemble.Models;

namespace XamarinAssemble.ViewModels
{
    public class SpeakersViewModel : ViewModelBase
    {
        public SpeakersViewModel()
        {
            Title = "Speakers";
        }
    }
}
