using System.Net.Http;

namespace LumTomofunCustomization.API_Helper
{
    public interface ILUMAPIConfig
    {
        HttpMethod RequestMethod { get; set; }

        string RequestUrl { get; set; }
        string AuthType { get; set; }
        string Token { get; set; }
        string AuthorizationToken { get; }
        string OrgName { get; set; }
    }
}
