using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

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

        public void _send_data(string relative_url, Dictionary<string, string> values)
        {
            List<string> data = new List<string>();
            foreach(KeyValuePair<string,string> entry in values)
            {

            }
        }
    }
}
