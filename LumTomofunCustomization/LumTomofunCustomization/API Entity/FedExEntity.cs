using System;
using System.Collections.Generic;

namespace LumTomofunCustomization.API_Entity
{
    public class FedExEntity
    {
        public class Access
        {
            //public string token_type { get; set; }
            //public string access_token { get; set; }
            //public int expires_in { get; set; }
            //public int consented_on { get; set; }
            //public string scope { get; set; }
            //public string refresh_token { get; set; }
            //public int refresh_token_expires_in { get; set; }
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

        public class Root
        {
            public string requestIdentifier { get; set; }
            public DateTime transactionDate { get; set; }
            public List<Inventory> inventory { get; set; }
        }

        public class Inventory
        {
            public string sku { get; set; }
            public string availableCount { get; set; }
            public string blockedCount { get; set; }
            public string inboundInventoryCount { get; set; }
            public string inboundReturnCount { get; set; }
            public string allocatedCount { get; set; }
            public string backorderedCount { get; set; }
        }

        public class AccessToken
        {
            public string client_id { get; set; }
            public string client_secret { get; set; }
        }
    }
}
