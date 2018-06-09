//using Microsoft.WindowsAzure.MobileServices;
//using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
//using Microsoft.WindowsAzure.MobileServices.Sync;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Text;
//using System.Threading.Tasks;
//using XamarinAssemble.Models;

//namespace XamarinAssemble.Cloud
//{
//    public class AzureDataManager : IDataManager
//    {
//        private static AzureDataManager defaultInstance = new AzureDataManager();
//        private MobileServiceClient client;
//        private IMobileServiceSyncTable<Session> tablaSesion;
//        private IMobileServiceSyncTable<Speaker> tablaSpeaker;

//        private AzureDataManager()
//        {
//            Initialize();
//        }

//        public static AzureDataManager DefaultManager
//        {
//            get
//            {
//                return defaultInstance;
//            }
//            private set
//            {
//                defaultInstance = value;
//            }
//        }

//        public MobileServiceClient CurrentClient
//        {
//            get { return client; }
//        }

//        public bool IsOfflineEnabled
//        {
//            get { return tablaSesion is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Session>; }
//        }

//        #region Generic Azure Sync Table Helper Methods

//        private void Initialize()
//        {
//            this.client = new MobileServiceClient(
//               Constants.ApplicationURL);

//            var store = new MobileServiceSQLiteStore("localstore.db");

//            if (!client.SyncContext.IsInitialized)
//            {
//                store.DefineTable<Session>();
//                store.DefineTable<Speaker>();
//                client.SyncContext.InitializeAsync(store);
//            }

//            tablaSesion = client.GetSyncTable<Session>();
//            tablaSpeaker = client.GetSyncTable<Speaker>();
//        }

//        public async Task<IEnumerable<T>> GetItemsAsync<T>() where T : ModelBase
//        {
//            try
//            {
//                return await this.client.GetSyncTable<T>().ToEnumerableAsync();
//            }
//            catch (MobileServiceInvalidOperationException msioe)
//            {
//                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine(@"Sync error: {0}", e.Message);
//            }
//            return null;
//        }

//        public async Task SyncAsync<T>() where T : ModelBase
//        {
//            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

//            try
//            {
//                await this.client.SyncContext.PushAsync();

//                switch (typeof(T).Name)
//                {
//                    case "Session":
//                        await this.tablaSesion.PullAsync($"allSession", this.tablaSesion.CreateQuery());
//                        break;
//                    case "Speaker":
//                        await this.tablaSpeaker.PullAsync($"allSpeaker", this.tablaSpeaker.CreateQuery());
//                        break;
//                }
//            }
//            catch (MobileServicePushFailedException exc)
//            {
//                if (exc.PushResult != null)
//                {
//                    syncErrors = exc.PushResult.Errors;
//                }
//            }

//            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
//            // server conflicts and others via the IMobileServiceSyncHandler.
//            if (syncErrors != null)
//            {
//                foreach (var error in syncErrors)
//                {
//                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
//                    {
//                        //Update failed, reverting to server's copy.
//                        await error.CancelAndUpdateItemAsync(error.Result);
//                    }
//                    else
//                    {
//                        // Discard local change.
//                        await error.CancelAndDiscardItemAsync();
//                    }

//                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
//                }
//            }
//        }

//        public async Task SaveItemAsync<T>(T item) where T : ModelBase
//        {
//            if (item.Id == null)
//            {
//                await this.client.GetSyncTable<T>().InsertAsync(item);
//            }
//            else
//            {
//                await this.client.GetSyncTable<T>().UpdateAsync(item);
//            }
//        }

//        #endregion

//        #region IDataManager Methods
//        public async Task<IEnumerable<Session>> GetSessionsAsync()
//        {
//            try
//            {
//                await this.SyncAsync<Session>();

//                return await this.tablaSesion.ToEnumerableAsync();
//            }
//            catch (MobileServiceInvalidOperationException msioe)
//            {
//                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine(@"Sync error: {0}", e.Message);
//            }
//            return null;
//        }

//        public async Task<IEnumerable<Speaker>> GetSpeakersAsync()
//        {
//            try
//            {
//                return await this.tablaSpeaker.ToEnumerableAsync();
//            }
//            catch (MobileServiceInvalidOperationException msioe)
//            {
//                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine(@"Sync error: {0}", e.Message);
//            }
//            return null;
//        }

//        //TODO: Implment SaveSpeakerAsync method here

//        #endregion
//    }
//}
