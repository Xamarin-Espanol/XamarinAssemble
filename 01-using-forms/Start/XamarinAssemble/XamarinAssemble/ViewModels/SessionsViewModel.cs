using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinAssemble.Models;

namespace XamarinAssemble.ViewModels
{
    public class SessionsViewModel : ViewModelBase
    {
        public ObservableCollection<Session> Sessions { get; set; }

        public ICommand GetSessionsCommand { get; set; }
        
        public Task Initialization { get; private set; }

        public SessionsViewModel()
        {
            Sessions = new ObservableCollection<Session>();
            Title = "Sessions";
            Initialization = GetSessions();
            GetSessionsCommand = RefreshCommand = new Command(async () => await GetSessions());
        }

        async Task GetSessions()
        {
            if (IsBusy)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                using (var client = new HttpClient())
                {
                    //grab json from server
                    var json = await client.GetStringAsync("https://xamarinassemblebaires.azurewebsites.net/tables/speakers?ZUMO-API-VERSION=2.0.0");

                    //Deserialize json
                    var items = JsonConvert.DeserializeObject<List<Session>>(json);

                    //Load sessions into list
                    Sessions.Clear();

                    foreach (var item in items)
                    {
                        Sessions.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
        }
    }
}
