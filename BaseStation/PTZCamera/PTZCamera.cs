using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Scarlet;
using Scarlet.Utilities;

namespace HuskyRobotics
{

    class PTZCamera
    {
        private string _base_url { get; set; }
        private string base64_Credentials { get; set; }
        private string _headers { get; set; }
        public HttpClient client { get; }

        public PTZCamera(string camera_address, string username, string password, HttpClient client)
        {
            base64_Credentials = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));
            this.client = client;
            this.client.DefaultRequestHeaders.Add("Cookie", "pt_speed=100; ipcam_profile=4; tour_index=-1");
            this.client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", base64_Credentials));
            _base_url = string.Format("http://{0}/", camera_address);
        }

        public PTZCamera(IPAddress camera_address, string username, string password, HttpClient client) : this(camera_address.ToString(),username,password,client){ }
        public PTZCamera(string camera_address, string username, string password) : this(camera_address,username,password, new HttpClient()){ }
        public PTZCamera(IPAddress camera_address, string username, string password) : this(camera_address.ToString(), username, password, new HttpClient()) { }

        public async void _send_data(string relative_url, Dictionary<string, string> values)
        {
            List<string> data = new List<string>();
            foreach(KeyValuePair<string,string> entry in values)
            {
                data.Add(entry.Key+"="+entry.Value);
            }
            string url = this._base_url + relative_url + "?" + string.Join("&",data);
            try { 
                string responseString = await client.GetStringAsync(url);
                Log.Output(Log.Severity.INFO, Log.Source.CAMERAS, _base_url+" Response: "+responseString);
            }
            catch
            {
                Log.Output(Log.Severity.ERROR, Log.Source.CAMERAS, "Could not connect to camera at ip: "+_base_url);
            }
        }

        public void set_speeds(int x_speed, int y_speed)
        {
            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                {"continuouspantiltmove", string.Format("{0},{1}",x_speed,y_speed) }
            };
            this._send_data("ptz.cgi",values);
        }
    }
}
