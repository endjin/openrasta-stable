namespace OpenRasta.Hosting.AspNet.Tests.Integration
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    using NUnit.Framework;

    using OpenRasta.Hosting.AspNet.AspNetHttpListener;
    using OpenRasta.Testing;
    using OpenRasta.Web;

    #endregion

    public class aspnet_server_context : context
    {
        protected readonly HttpListenerController http;

        protected HttpWebResponse theResponse;

        protected string theResponseAsString;

        private static readonly Random Random = new Random();

        private int port;

        public aspnet_server_context()
        {
            this.SelectPort();

            this.http = new HttpListenerController(new[] { "http://+:" + this.port + "/" }, "/", FileCopySetup.TempFolder.FullName);
            this.http.Start();
        }

        protected override void SetUp()
        {
            base.SetUp();
            this.theResponseAsString = null;
            this.theResponse = null;
        }

        [TestFixtureTearDown]
        public void tear()
        {
            if (this.http != null)
            {
                this.http.Stop();
            }
        }

        public void GivenARequest(string verb, string uri)
        {
            this.GivenARequest(verb, uri, null, null);
        }

        public void GivenATextRequest(string verb, string uri, string content, string textEncoding)
        {
            this.GivenATextRequest(verb, uri, content, textEncoding, "text/plain");
        }

        public void GivenATextRequest(string verb, string uri, string content, string textEncoding, string contentType)
        {
            this.GivenARequest(
                verb,
                uri,
                Encoding.GetEncoding(textEncoding).GetBytes(content),
                new MediaType(contentType) { CharSet = textEncoding });
        }

        public void GivenAUrlFormEncodedRequest(string verb, string uri, string content, string textEncoding)
        {
            this.GivenARequest(
                verb,
                uri,
                Encoding.GetEncoding(textEncoding).GetBytes(content),
                new MediaType("application/x-www-form-urlencoded") { CharSet = textEncoding });
        }

        public void GivenARequest(string verb, string uri, byte[] content, MediaType contentType)
        {
            var destinationUri = new Uri("http://127.0.0.1:" + this.port + uri);

            WebRequest request = WebRequest.Create(destinationUri);
            request.Timeout = int.MaxValue;
            request.Method = verb;
            request.ContentLength = content != null ? content.Length : 0;

            if (request.ContentLength > 0)
            {
                request.ContentType = contentType.ToString();
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(content, 0, content.Length);
                }
            }

            try
            {
                this.theResponse = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException exception)
            {
                this.theResponse = exception.Response as HttpWebResponse;
            }
        }

        public void GivenTheResponseIsInEncoding(Encoding encoding)
        {
            var data = new byte[this.theResponse.ContentLength];

            int payload = this.theResponse.GetResponseStream().Read(data, 0, data.Length);

            this.theResponseAsString = encoding.GetString(data, 0, payload);
        }

        public void ConfigureServer(Action t)
        {
            this.http.Host.ExecuteConfig(t);
        }

        private void SelectPort()
        {
            this.port = Random.Next(40000, 40500);
        }
    }
}