using System.Net.Http;

namespace LumTomofunCustomization.API_Helper
{
    public class LUMAPIConfig : ILUMAPIConfig
    {
        /// <summary> API Method </summary>
        public HttpMethod RequestMethod { get; set; }

        /// <summary> Target API EndPoint </summary>
        public string RequestUrl { get; set; }

        /// <summary> Authorization Type </summary>
        public string AuthType { get; set; }

        /// <summary> Authorization secret key </summary>
        public string Token { get; set; }

        /// <summary> Combine Authorization Token </summary>
        public string AuthorizationToken
        {
            get => $"{this.AuthType} {this.Token}";
        }

        /// <summary> FedEx API -> Partner or Organization name. No spaces are allowed. </summary>
        public string OrgName { get; set; }
    }
}
