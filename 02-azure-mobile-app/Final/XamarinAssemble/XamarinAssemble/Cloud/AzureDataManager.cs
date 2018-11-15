using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using XamarinAssemble.Models;

namespace XamarinAssemble.Cloud
{
    public class AzureDataManager : IDataManager
    {
        private static AzureDataManager defaultInstance = new AzureDataManager();
        private MobileServiceClient client;

        public static AzureDataManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return tablaSesion is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Sessions>; }
        }

        private IMobileServiceSyncTable<Sessions> tablaSesion;
        private IMobileServiceSyncTable<Speakers> tablaSpeaker;

        private AzureDataManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.client = new MobileServiceClient(
               Constants.ApplicationURL);

            var store = new MobileServiceSQLiteStore("localstore.db");

            if (!client.SyncContext.IsInitialized)
            {
                store.DefineTable<Sessions>();
                store.DefineTable<Speakers>();
                client.SyncContext.InitializeAsync(store);
            }

            tablaSesion = client.GetSyncTable<Sessions>();
            tablaSpeaker = client.GetSyncTable<Speakers>();
        }

        public async Task SyncAsync(string type)
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                switch (type)
                {
                    case "Session":
                        await this.tablaSesion.PullAsync($"allSession", this.tablaSesion.CreateQuery());
                        break;
                    case "Speaker":
                        await this.tablaSpeaker.PullAsync($"allSpeaker", this.tablaSpeaker.CreateQuery());
                        break;
                    case "All":
                        await this.tablaSesion.PullAsync($"allSession", this.tablaSesion.CreateQuery());
                        await this.tablaSpeaker.PullAsync($"allSpeaker", this.tablaSpeaker.CreateQuery());
                        break;
                }
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }

        public async Task<IEnumerable<Sessions>> GetSessionsAsync()
        {
            try
            {
                await this.SyncAsync("Session");

                return await this.tablaSesion.ToEnumerableAsync();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Speakers>> GetSpeakersAsync()
        {
            try
            {
                await this.SyncAsync("Speaker");

                return await this.tablaSpeaker.ToEnumerableAsync();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }

            return null;
        }
    }
}
