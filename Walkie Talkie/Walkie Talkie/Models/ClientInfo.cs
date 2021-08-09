using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Walkie_Talkie.Models
{
    [Table("Client")]
    public class ClientInfo
    {
        [Column("ip_address")]
        public string IPAddress { get; set; }

        [Column("port")]
        public int Port { get; set; }

        [Column("ip_endpoint")]
        public string IPEndPoint { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("bssid")]
        [Indexed]
        public string BSSID { get; set; }


        public static ClientInfo Create(string ipAddress, int port, string Ipendpoint, string name, string bssid)
        {
            return new ClientInfo()
            {
                IPAddress = ipAddress,
                Port = port,
                IPEndPoint = Ipendpoint,
                Name = name,
                BSSID = bssid,
            };
        }

        public static ClientInfo Create(string dataformQR)
        {
            string name = dataformQR.Substring(0, dataformQR.IndexOf(':'));
            string ipendpoint = dataformQR.Substring(dataformQR.IndexOf(':') + 1);
            string ipaddress = ipendpoint.Substring(0, dataformQR.LastIndexOf(':'));
            int port = int.Parse(ipendpoint.Substring(ipendpoint.LastIndexOf(':') + 1));

            return new ClientInfo()
            {
                Name =  name,
                IPEndPoint = ipendpoint ,
                IPAddress = ipaddress,
                Port = port,
            };
        }

    }
}
