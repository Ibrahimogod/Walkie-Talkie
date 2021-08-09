using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Walkie_Talkie.Models
{
    [Table("Network")]
    public class Network
    {
        [Column("bssid")]
        [PrimaryKey]
        public string BSSID {  get; set; }
        
        [Column("ssid")]
        public string SSID { get; set; }

        [Column("last_ip")]
        public string LastIP { get; set; }

        List<ClientInfo> _clients = new List<ClientInfo>();
    }
}