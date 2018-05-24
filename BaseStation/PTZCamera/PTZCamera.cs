using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Scarlet;
using Scarlet.Utilities;

namespace HuskyRobotics.BaseStation
{
    /// <summary>
    /// This class represents the IP camera and allows for connection via IP and control using HTTP GET requests.
    /// </summary>
    class PTZCamera
    {
        private int lastXSpeed { get; set; }
        private int lastYSpeed { get; set; }
        private string BaseURL { get; set; } //the ip address for the camera
        private string Base64Credentials { get; set; } // The username and password for the camera
        private string Headers { get; set; } //The HTTP headers for the camera
        public HttpClient client { get; } // The Httpclient used to connet to and communicate with ip devices using HTTP requests. This should be initialized elsewhere and share between all HTTP based devices.

        /// <summary>
        /// This is the standard constructor used to initialize the camera.
        /// </summary>
        /// <param name="cameraAddress">A string representing the ip address. This should only be te ip not the full url.</param>
        /// <param name="username">The username used for authorization credentials on the camera</param>
        /// <param name="password">The password used for authorization credentials on the camera</param>
        /// <param name="client">The HTTP client used for HTTP based communications. This should be initialized elsewhere and share between all HTTP based devices.</param>
        public PTZCamera(string cameraAddress, string username, string password, HttpClient client)
        {
            Base64Credentials = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));
            this.client = client;
            this.client.DefaultRequestHeaders.Add("Cookie", "pt_speed=100; ipcam_profile=4; tour_index=-1");
            this.client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Base64Credentials));
            BaseURL = string.Format("http://{0}/", cameraAddress);
            lastXSpeed = 0;
            lastYSpeed = 0;
        }

        /// <summary>
        /// This is the IPAddress based constructor used to initialize the camera.
        /// </summary>
        /// <param name="cameraAddress">An IPAddress object.</param>
        /// <param name="username">The username used for authorization credentials on the camera</param>
        /// <param name="password">The password used for authorization credentials on the camera</param>
        /// <param name="client">The HTTP client used for HTTP based communications. This should be initialized elsewhere and share between all http based devices.</param>
        public PTZCamera(IPAddress cameraAddress, string username, string password, HttpClient client) : this(cameraAddress.ToString(),username,password,client){ }

        /// <summary>
        /// This is the string based constructor without an HTTP client.
        /// This constructor can be used to initialize a new Httpclient for the camera.
        /// </summary>
        /// <param name="cameraAddress">A string representing the ip address. This should only be te ip not the full url.</param>
        /// <param name="username">The username used for authorization credentials on the camera</param>
        /// <param name="password">The password used for authorization credentials on the camera</param>
        public PTZCamera(string cameraAddress, string username, string password) : this(cameraAddress,username,password, new HttpClient()){ }

        /// <summary>
        /// This is the IPAddress based constructor without an HTTP client.
        /// This constructor can be used to initialize a new Httpclient for the camera.
        /// </summary>
        /// <param name="cameraAddress">An IPAddress object.</param>
        /// <param name="username">The username used for authorization credentials on the camera</param>
        /// <param name="password">The password used for authorization credentials on the camera</param>
        public PTZCamera(IPAddress cameraAddress, string username, string password) : this(cameraAddress.ToString(), username, password, new HttpClient()) { }

        /// <summary>
        /// This is the method used to send data to the camera
        /// </summary>
        /// <param name="realativeURL">A string representin</param>
        /// <param name="values">A dictionary of string keys and string values that will be sent as parameters via a HTTP GET request</param>
        private async void SendData(string realativeURL, Dictionary<string, string> values)
        {
            List<string> data = new List<string>();
            foreach(KeyValuePair<string,string> entry in values)
            {
                data.Add(entry.Key+"="+entry.Value);
            }
            string url = this.BaseURL + realativeURL + "?" + string.Join("&",data);
            try { 
                HttpResponseMessage responseString = await client.GetAsync(url);
                Log.Output(Log.Severity.INFO, Log.Source.CAMERAS, BaseURL+" SpeedChanged: "+responseString.IsSuccessStatusCode);
            }
            catch
            {
                Log.Output(Log.Severity.ERROR, Log.Source.CAMERAS, "Could not connect to camera at ip: "+BaseURL);
            }
        }

        /// <summary>
        /// This is a method used to set the panning speeds of the camera
        /// </summary>
        /// <param name="xSpeed">Horizontal panning speed</param>
        /// <param name="ySpeed">Vertical panning speed</param>
        public void SetSpeeds(int xSpeed, int ySpeed)
        {
            if ((xSpeed != lastXSpeed || ySpeed != lastYSpeed)) {
                lastXSpeed = xSpeed;
                lastYSpeed = ySpeed;
                Dictionary<string, string> values = new Dictionary<string, string>()
                {
                    {"continuouspantiltmove", string.Format("{0},{1}",xSpeed,ySpeed) }
                };
                this.SendData("ptz.cgi",values);
            }
        }
    }
}
