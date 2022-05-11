using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMLocalization.Helper
{
    public class TWCustomsDutyResponse
    {
        public string msg { get; set; }
        public string status { get; set; }
        public string guest { get; set; }
        public List<TWCustomsDutyData> data { get; set; }
    }
    public class TWCustomsDutyData
    {
        public string UP_DATE { get; set; }
        public string CRRN_CD { get; set; }
        public string UP_PERSON { get; set; }
        public Decimal IN_RATE { get; set; }
        public Decimal EX_RATE { get; set; }
        public string YEAR { get; set; }
        public string MON { get; set; }
        public string TEN_DAY { get; set; }
        public int ORDER_FLAGN { get; set; }
    }
}
