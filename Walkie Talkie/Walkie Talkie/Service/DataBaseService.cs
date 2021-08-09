using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Walkie_Talkie.Models;

namespace Walkie_Talkie.Service
{
    public class DataBaseService
    {
        static SQLiteAsyncConnection _dataBase;

        static bool _initialized;

        public bool Initialized => _initialized;

        public event EventHandler Initializing;

        public DataBaseService()
        {
            if (_dataBase == null)
            {
                _initialized = false;
                InitializeAsync();
            }
        }

        async void InitializeAsync()
        {
            _dataBase = new SQLiteAsyncConnection(Constants.CONNECTION_STRING);
            await _dataBase.CreateTableAsync<Network>();

            string createQuery = @"CREATE TABLE IF NOT EXISTS Client(
                                    ip_address TEXT NOT NULL,
                                    port TEXT NOT NULL,
                                    ip_endpoint TEXT NOT NULL,
                                    name TEXT NOT NULL,
                                    bssid TEXT NOT NULL,
                                    FOREIGN KEY(bssid) REFERENCES Network(bssid),
                                    PRIMARY KEY (ip_address, bssid)
                                    );";

            await _dataBase.ExecuteAsync(createQuery);

            string triggerQuery = @"CREATE TRIGGER IF NOT EXISTS network_update 
                                    AFTER UPDATE ON Network 
                                    WHEN old.last_ip <> new.last_ip
                                    BEGIN
                                        DELETE FROM Client WHERE bssid = new.bssid ; 
                                    END;";

            await _dataBase.ExecuteAsync(triggerQuery);

            Initializing?.Invoke(this, EventArgs.Empty);
            _initialized = true;

        }


        public async Task<int> UpdateNetworkAsync(Network network)
        {
            return await _dataBase.UpdateAsync(network);
        }

        public async Task<List<Network>> GetNetworksAsync()
        {
            return await _dataBase.Table<Network>().ToListAsync();
        }

        public async Task<int> AddNetworkAsync(Network network)
        {
            try
            {
                return await _dataBase.InsertAsync(network);
            }
            catch { return 0; }
        }

        public async Task<int> DeleteNetworkClients(string bssid)
        {
            string deleteQuery = $"DELETE FROM Client WHERE bssid = '{bssid}'";
            return await _dataBase.ExecuteAsync(deleteQuery);
        }

        public async Task<List<ClientInfo>> GetClientsAsync()
        {
            return await _dataBase.Table<ClientInfo>().ToListAsync();
        }

        public async Task<int> AddClientAsync(ClientInfo client)
        {
            try
            {
                return await _dataBase.InsertAsync(client);
            }
            catch { return 0; }
        }

        public async Task<int> DeleteClientAsync(ClientInfo client)
        {
            string deleteQuery = $"DELETE FROM Client WHERE ip_address = '{client.IPAddress}' AND bssid = '{client.BSSID}' ";
            return await _dataBase.ExecuteAsync(deleteQuery, new object[0]);
        }
    }
}
