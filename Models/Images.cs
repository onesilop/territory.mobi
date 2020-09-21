using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace territory.mobi.Models
{
    public partial class Images
    {
        public Guid ImgId { get; set; }
        public string ImgText { get; set; }
        public string ImgPath { get; set; }
        public byte[] ImgImage { get; set; }
        public Guid? MapId { get; set; }
        public DateTime? Updatedatetime { get; set; }

        public virtual Map Map { get; set; }


        public string Cat
        {
            get
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.thecatapi.com/v1/images/search");
                request.Headers["x-api-key"] = "9ba33624-dfc4-4cef-b52b-7f9c25830fa4";
                request.ContentType = "application/json";
                request.Method = "GET";
                string resp = "";
                try
                {
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                        resp = reader.ReadToEnd();
                        resp = resp.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
                        var x = JObject.Parse(resp);
                        resp = x["url"].ToString();
                    }
                }
                catch
                {
                    resp = "https://cdn2.thecatapi.com/images/3d8.png";
                }
                return resp;
            }
        }
    }
}
