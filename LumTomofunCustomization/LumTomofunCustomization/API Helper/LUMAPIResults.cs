using System.Net;
using System.Net.Http;

namespace LumTomofunCustomization.API_Helper
{
    public class LUMAPIResults
    {
        public HttpStatusCode StatusCode { get; set; }
        public HttpContent Content { get; set; }
        public string ContentResult { get; set; }
    }
}
