using System.Collections.Generic;

namespace LumTomofunCustomization.API_Entity
{
    public class GoogleSheetsEntity
    {
        public class RequestRoot
        {
            public string SheetName { get; set; }
        }

        public class DataRoot
        {
            public List<List<object>> Data { get; set; }
        }
    }
}
