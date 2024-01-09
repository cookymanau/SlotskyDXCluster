using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotskyDXCluster
{
    [Serializable]
    public class classTelnetCluster
    {
        public string address { get; set; }
        public string port { get; set; }

        public string name { get; set; }

        public string type { get; set; } //either CCCluster or Spider.  Spider should just pass them through

        public string comment { get; set; }

    }
}
