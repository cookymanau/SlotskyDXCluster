using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace SlotskyDXCluster
{
    public static class classUtilities
    {


        public static List<classEntity> entitys = new List<classEntity>();




        /// <summary>
        /// Given a number in Hz translates it to a band   70000047 => 40M
        /// </summary>
        /// <param name="strSpotfreq"></param>
        /// <returns></returns>
        public static string Frequency2Band(string strSpotfreq)
        {
            // clean up strSpotFreq - different depending on DXCluster or FT8
            //   24905.1 from DXCluster
            //   24905100 from FT8

            int dubSpotfreq = 0;

            if (!strSpotfreq.Contains("."))
                int.TryParse(strSpotfreq, out dubSpotfreq);
            else if (strSpotfreq.Contains("."))
                int.TryParse(strSpotfreq.Substring(0, strSpotfreq.IndexOf(".")), out dubSpotfreq);
            //dubSpotfreq =   int.Parse(strSpotfreq.Substring(0, strSpotfreq.IndexOf(".")));


            string band = "";

            if (dubSpotfreq >= 1800 && dubSpotfreq < 2000)
                band = "160M";

            else if (dubSpotfreq >= 3500 && dubSpotfreq < 4000)
                band = "80M";
            else if (dubSpotfreq >= 5000 && dubSpotfreq < 6000)
                band = "60M";

            else if (dubSpotfreq >= 7000 && dubSpotfreq < 7300)
                band = "40M";

            else if (dubSpotfreq >= 10100 && dubSpotfreq < 10200)
                band = "30M";

            else if (dubSpotfreq >= 14000 && dubSpotfreq < 14350)
                band = "20M";

            else if (dubSpotfreq >= 18068 && dubSpotfreq < 18168)
                band = "17M";

            else if (dubSpotfreq >= 21000 && dubSpotfreq < 21450)
                band = "15M";

            else if (dubSpotfreq >= 24889 && dubSpotfreq < 24990)
                band = "12M";

            else if (dubSpotfreq >= 28000 && dubSpotfreq < 29700)
                band = "10M";
            else if (dubSpotfreq >= 50000 && dubSpotfreq < 54000)
                band = "6M";
            else if (dubSpotfreq >= 60000)
                band = "VHF";

            return band;
        }

        /// <summary>
        /// Send it a callsign returns a country  150 => Australia
        /// </summary>
        /// <param name="callsign"></param>
        /// <returns></returns>
        public static string call2entity(string callsign)
        {

            try
            {
                string[] poss = new string[10]; //20221004 made this bigger, was 114. I think the cluster string was real long.
                //Debug.WriteLine($"The call2Entity poss string is length = {poss.Length}");
                int cnt = 0;

                // string ovride = checkForOverride(callsign, "entity");
                //   if (ovride.Length > 0)
                //    {
                //        return ovride;
                //    }

                if (callsign.Length > 3)
                {
                    //foreach (classEntity entity in entitys.ToList()) // for each entry in DXCCIDs etc   changed this one as well.
                    foreach (classEntity entity in entitys) // for each entry in DXCCIDs etc   changed this one as well.
                    {
                        string callLeft = callsign.Substring(0, 4);
                        if (callLeft.Length > 0)
                        {
                            if (callLeft.StartsWith(entity.Prefix))
                            {
                                cnt++;
                                poss[cnt] = entity.entity;
                                //  Debug.WriteLine($"Callsign= {callsign}, Count = {cnt}  and Poss ={poss[cnt]} and entity =  {entity.entity} ");
                                //break;
                            }
                        }
                    }
                }
                //  Debug.WriteLine($"The Callsign is {callsign}  The Return will be {poss[cnt]}  and cnt = {cnt}");
                if (cnt == 0)  //attempt to use the Overrides.csv file for unknown callsigns
                {
                    //foreach (var overridecall in lstOverrides)
                    //{
                    //    if (overridecall.DX == callsign)
                    //        return overridecall.country;
                    //}
                    return "Unk";
                }
                else
                    return poss[cnt];

            }
            catch (Exception ex)
            {
                //  classUtilities.writeToLog($"ERROR:Error in classUtilities.call2entity - Called from frmFT8Decodes UDP loop line 248 {callsign} {ex}");
                return " ";
            }
        }


        //public static string entityFileName = $@"C:\Ham\DXLab\DXKeeper\Databases\DXCC.mdb";
        public static string entityFileName = $@"D:\Users\Cooky\Documents\Slotsky\System\DXCC.mdb";

            /// reads data from mdb  with dxccids - its the database from DXkeeper  spotcollector
            /// </summary>
            public static void geDXCCdata()
            {
                entitys.Clear();

                string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={entityFileName};";//;Extended Properties=\"Excel 12.0 Macro;HDR=Yes;\"";

                string strSQL = "SELECT a.CountryCode,a.DXCCName,b.DXCCPrefix,b.Location,b.Prefix,a.CountryCode FROM DXCC a inner join PREFIX b on a.DXCCPrefix = b.DXCCPrefix ORDER BY b.DXCCPrefix,b.Prefix DESC;";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(strSQL, connection);
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            entitys.Add(new classEntity
                            {
                                ccode = reader["CountryCode"].ToString(),
                                entity = reader["DXCCName"].ToString(),
                                DXCCPrefix = reader["DXCCPrefix"].ToString(),
                                Location = reader["Location"].ToString(),
                                Prefix = reader["Prefix"].ToString(),
                                DXCCID = reader["CountryCode"].ToString()
                            });
                        }
                    }
                }
            }//end of function to associate an entity name with a country code



        /// <summary>
        /// Given an entity eg Australia, will return 150 (its dxccid)
        /// </summary>
        /// <param name="dxccid"></param>
        /// <returns></returns>
        public static string Entity2DXCCID(string entity)
        {

            //entitys = F1.getEntitys;

            for (int k = 0; k < entitys.Count; k++)
            {
                if (entitys[k].entity == entity)
                    return entitys[k].DXCCID;
            }
            return "UNK";
        }



        public static string freq2mode(string Spotfreq)
        {
            // Define the frequency ranges and their corresponding modes
            var frequencyRanges = new List<(int, int, string)>
{
    (1800, 1840, "CW"),
    (1840, 1850, "DIG"),
    (1850, 1899, "SSB"),
    (3500, 3568, "CW"),
    (3568, 3580, "DIG"), //FT4
    (3580, 3700, "SSB"),
    (7000, 7047, "CW"),
    (7047, 7051, "DIG"), //DXpeditions and FT4
    (7051, 7056, "CW"),
    (7056, 7058, "DIG"),//FT8
    (7058, 7074, "CW"),
    (7074, 7078, "DIG"),//FT8
    (7078, 7300, "SSB"),
    (10100, 10131, "CW"),//30m BAND NO ssb
    (10131, 10150, "DIG"),//1031 is where F/H lives for DXpeditions
    (14000, 14070, "CW"),
    (14070, 14100, "DIG"),
    (14100, 14101, "CW"),//beacon
    (14101, 14230, "SSB"),
    (14230, 14240, "DIG"),// SSTV
    (14240, 14350, "SSB"),
    (18068, 18090, "CW"),
    (18090, 18109, "DIG"),
    (18109, 18111, "CW"),//Beacon at 110
    (18111, 18168, "SSB"),
    (21000, 21070, "CW"),
    (21070, 21144, "DIG"),//ft4 
    (21144, 21149, "SSB"),// DONT KNOW REALLY, MIGHT BE dig
    (21149, 21151, "CW"), //beacon
    (21151, 21450, "SSB"),
    (24890, 24910, "CW"),//12M band  CW
    (24910, 24930, "DIG"),
    (24930, 24990, "SSB"),
    (28000, 28070, "CW"),//10M band
    (28070, 28200, "DIG"),//10M band
    (28200, 28201, "CW"),//10M band
    (28201, 28900, "SSB"),//10M band
    (28900, 29700, "FM"),//10M band
    (50000, 50312, "CW"),//10M band
    (50312, 50320, "DIG"),//10M band


    // Add more ranges and modes as needed...
};
            double spotfreq = 0;
            double.TryParse(Spotfreq, out spotfreq);

            // Iterate over the frequency ranges and find the corresponding mode
            string spotbandmode = "Unknown";
            foreach (var freqRange in frequencyRanges)
            {
                int start = freqRange.Item1;
                int end = freqRange.Item2;
                string mode = freqRange.Item3;

                if (spotfreq >= start && spotfreq < end)
                {
                    spotbandmode = mode;
                    return mode;
                    // break;  // Exit the loop since we found the mode
                }
            }
            return "UNK";
        }













    }
}
