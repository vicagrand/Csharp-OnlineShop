using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;


        static PaypalConfiguration()
        {
            var confiq = getconfig();
            clientId = "ARuZdiH8vfyEzjcO0Uh-Hh2fckGrtQVr3CgExtR1SW6HCp_NkpOEuwOcsAdCZf-0IGh5pplbcbyjLty2";
            clientSecret = "EO-O4IbOoU48wQSjOPnZSpS0GGyCncc0mOuywgJb1gkfEK4ier7FOQFf_MbT6fP-k_6JHLsUnOjZCHMS";
        }

        private static Dictionary<string,string> getconfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, getconfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            APIContext apicontext = new APIContext(GetAccessToken());
            apicontext.Config = getconfig();
            return apicontext;
        }
    }
}