namespace SlotskyDXCluster
{
    public class classEntity
    {
        public string ccode { get; set; }  //this CountryCode or DXCCID  eg 150
        public string entity { get; set; } //this is for Location or entity name - eg Austrlai

        public string DXCCPrefix { get; set; }  //from the Prefix table  eg VK
        public string Location { get; set; }
        public string Prefix { get; set; } // from the Prefix table eg VK6, AX6 etc
        public string DXCCID { get; set; }


        public string getEntityByDxccid()
        {
            return this.entity;
        }

        public string getPrefixByDxccid()
        {
            return this.DXCCPrefix;
        }

    }
}
