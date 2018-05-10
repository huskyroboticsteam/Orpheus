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
    /// <summary>
    /// This class represents the IP camera and allows for connection via IP and control using HTTP GET requests.
    /// </summary>
    class PTZCamera
    {
        private string _base_url { get; set; } //the ip address for the camera
        private string base64_Credentials { get; set; } // The username and password for the camera
        private string _headers { get; set; } //The HTTP headers for the camera
        public HttpClient client { get; set; } // The Httpclient used to connet to and communicate with ip devices using HTTP requests. This should be initialized elsewhere and share between all HTTP based devices.

        /// <summary>
        /// This is the standard constructor used to initialize the camera.
        /// </summary>
        /// <param name="camera_address">A string representing the ip address. This should only be te ip not the full url.</param>
        /// <param name="username">The username used for authorization credentials on the camera</param>
        /// <param name="password">The password used for authorization credentials on the camera</param>
        /// <param name="client">The HTTP client used for HTTP based communications. This should be initialized elsewhere and share between all HTTP based devices.</param>
        public PTZCamera(string camera_address, string username, string password, HttpClient client)
        {
            base64_Credentials = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));
            this.client = client;
            this.client.DefaultRequestHeaders.Add("Cookie", "pt_speed=100; ipcam_profile=4; tour_index=-1");
            this.client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", base64_Credentials));
            _base_url = string.Format("http://{0}/", camera_address);
        }

        /// <summary>
        /// This is the IPAddress based constructor used to initialize the camera.
        /// </summary>
        /// <param name="camera_address">An IPAddress object.</param>
        /// <param name="username">The username used for authorization credentials on the camera</param>
        /// <param name="password">The password used for authorization credentials on the camera</param>
        /// <param name="client">The HTTP client used for HTTP based communications. This should be initialized elsewhere and share between all http based devices.</param>
        public PTZCamera(IPAddress camera_address, string username, string password, HttpClient client) : this(camera_address.ToString(),username,password,client){ }

        /// <summary>
        /// This is the string based constructor without an HTTP client.
        /// This constructor can be used to initialize a new Httpclient for the camera.
        /// </summary>
        /// <param name="camera_address">A string representing the ip address. This should only be te ip not the full url.</param>
        /// <param name="username">The username used for authorization credentials on the camera</param>
        /// <param name="password">The password used for authorization credentials on the camera</param>
        public PTZCamera(string camera_address, string username, string password) : this(camera_address,username,password, new HttpClient()){ }

        /// <summary>
        /// This is the IPAddress based constructor without an HTTP client.
        /// This constructor can be used to initialize a new Httpclient for the camera.
        /// </summary>
        /// <param name="camera_address">An IPAddress object.</param>
        /// <param name="username">The username used for authorization credentials on the camera</param>
        /// <param name="password">The password used for authorization credentials on the camera</param>
        public PTZCamera(IPAddress camera_address, string username, string password) : this(camera_address.ToString(), username, password, new HttpClient()) { }

        /// <summary>
        /// This is the method used to send data to the camera
        /// </summary>
        /// <param name="relative_url">A string representin</param>
        /// <param name="values">A dictionary of string keys and string values that will be sent as parameters via a HTTP GET request</param>
        private async void _send_data(string relative_url, Dictionary<string, string> values)
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

        /// <summary>
        /// This is a method used to set the panning speeds of the camera
        /// </summary>
        /// <param name="x_speed">Horizontal panning speed</param>
        /// <param name="y_speed">Vertical panning speed</param>
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
