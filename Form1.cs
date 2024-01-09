using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;


namespace SlotskyDXCluster
{
    public partial class frmMain : Form
    {

        bool DEBUG = true;
        Stream stream;

        //string connectionString = "Data Source=BOYA-PC;Integrated Security = True; Initial Catalog = SLOTSKY";
        string connectionString = "Data Source=BOYA-PC; Initial Catalog = SLOTSKY;TrustServerCertificate = true; User Id = ianacook; Password=ianacook";
        string start1Site = "";
        string start2Site = "";


        public frmMain()
        {
            InitializeComponent();
            InitializeListener();  //for socket comms
            classUtilities.geDXCCdata();
        }

        private async void btnStart1_Click(object sender, EventArgs e)
        {
            //Task.Run(() => task1());
            task1();
            btnStart1.BackColor = Color.LightCoral;
        }

        //private void task1()
        private async void task1()
        {

            string[] start1 = comboBoxStart1.Text.Split(',');
            start1Site = start1[2];
            lblStart1.Text = start1Site;

            // EstablishConnection(start1[0], int.Parse(start1[1]));
            await EstablishConnectionAsync(start1[0], int.Parse(start1[1]));

            //String response = SendMessage("VK6DW\r\n");
            String response = await SendMessageAsync("VK6DW\r\n");
            //Thread.Sleep(1000);
            await Task.Delay(1000);



            while (true)
            {

                // Task.Run(() => ReadMessage());
                //ReadMessage();
                await ReadMessageAsync();
                // Debug.WriteLine(ReadMessage());
                // Thread.Sleep(200);
            }
        }


        private void btnStart2_Click(object sender, EventArgs e)
        {
        }


        public async Task EstablishConnectionAsync(string ip_address, int port_number = 23)
        {
            try
            {
                TcpClient client = new TcpClient(ip_address, port_number);
                Task.Run(() => MonitorConnection(client));

                if (DEBUG) Debug.WriteLine("[Communication] : [EstablishConnection] : Success connecting to : {0}, port: {1}", ip_address, port_number);
                stream = client.GetStream();
            }
            catch
            {
                Console.WriteLine("[Communication] : [EstablishConnection] : Failed while connecting to : {0}, port: {1}", ip_address, port_number);
                System.Environment.Exit(1);
            }
        }


        private void MonitorConnection(TcpClient client)
        {
            while (client.Connected)
            {
                // You can implement additional logic to check the connection status
                // For simplicity, we are using a simple delay here
                Task.Delay(5000).Wait(); // Adjust the delay as needed

                // Update the connection status on the UI thread
                UpdateConnectionStatus("Connected");
                readData2DatabaseAsync("CLUSTER");
            }

            // Update the connection status when disconnected
            UpdateConnectionStatus("Disconnected");
        }

        private void UpdateConnectionStatus(string status)
        {
            // Update the connection status on the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action(() => lblConnectedStatus.Text = status));
            }
            else
            {
                lblConnectedStatus.Text = status;
            }
        }

        // Add additional methods and event handlers as needed
















        public async Task<string> SendMessageAsync(string command)
        {
            // Send command
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(command + "\r\n");
            await stream.WriteAsync(data, 0, data.Length);
            if (DEBUG) Console.Write("Sent : {0}", command);

            await Task.Delay(2000);

            // Receive response
            string response = await ReadMessageAsync();
            if (DEBUG) Debug.WriteLine("Received : {0}", response);

            return response;
        }


        public async Task<string> ReadMessageAsync()
        {
            try
            {

                string cleanItem = "";

                // Receive response
                Byte[] responseData = new byte[256];
                Int32 numberOfBytesRead = await stream.ReadAsync(responseData, 0, responseData.Length);
                string response = System.Text.Encoding.ASCII.GetString(responseData, 0, numberOfBytesRead);


                // Clean up the string
                string[] spts = response.Split('\n');

                for (int i = 0; i < spts.Length - 1; i++)
                {
                    cleanItem = string.Join(" ", spts[i].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
                }

                cleanItem = cleanItem.TrimEnd('\a');

                //Task.Run(() => Send2Database(cleanItem));
                await Send2DatabaseAsync(cleanItem);

                //Debug.WriteLine($"{response}             {cleanItem} ");
                // Debug.WriteLine($"        {cleanItem} ");
                return cleanItem;
            }
            catch (Exception)
            {

                Debug.WriteLine($"Error in In ReadMessageAsync() ");
                return "";
            }
            finally
            {
            }

        }



        private async Task Send2DatabaseAsync(string cleanItem)
        {
            try
            {
                string rn = DateTime.UtcNow.ToString("HHmm");
                rn = rn + "Z";

                if (chkShowMessages.Checked == true)
                {
                    richTextBox1.AppendText($"{cleanItem}\r\n");
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                }

                // Debug.WriteLine(cleanItem);

                if (cleanItem.StartsWith("DX de"))
                {

                    if (!cleanItem.Contains("DXSpider") && !cleanItem.Contains("CCcluster") && !cleanItem.Contains("UNSET") && !cleanItem.Contains("This") && !cleanItem.Contains("Nodes") && !cleanItem.Contains("http") && !cleanItem.Contains("BCN") && !cleanItem.Contains("BEACON"))
                    {

                        string[] sd1 = cleanItem.Split(' '); //initial split to fix a format problem with the raw data
                        if (sd1[2].Length > 11)// need to fix the data where the DE and Frequency are run together.
                        {
                            cleanItem = cleanItem.Replace(":1", ": 1");
                            cleanItem = cleanItem.Replace(":2", ": 2");
                            cleanItem = cleanItem.Replace("'", "");
                        }
                        string comment = IsolateData(cleanItem);

                        string[] sd = cleanItem.Split(' ');
                        // Debug.WriteLine($"--->{cleanItem} \t\t\tComment = {comment} \t\t sd[2]={sd[2]} and  sd[4] = {sd[4]}  and sd[3]={sd[3]}");

                        // somewhere here we need to find our comment after the 5th space?

                        //WE NEED TO TAKE CARE OF THE MODE.  sd[5] is guessed at by the cluster.  It seems to be the first word of the comment - or that might be me
                        if (sd[5] != "CW" && sd[5] != "SSB" && sd[5] != "LSB" && sd[5] != "USB")
                        {
                            sd[5] = classUtilities.freq2mode(sd[3]);
                            // sd[5] = "DIG";  // this is an experiment to see if this classUtilities.freq2mode(sd[3]) s the problem

                        }
                        //else
                        //    sd[5] = sd[5];


                        if (sd.Length == 9)
                        {
                            string entity = classUtilities.call2entity(sd[4]);
                            string dxcc = classUtilities.Entity2DXCCID(entity);
                            int dxccid = int.Parse(dxcc);
                            //   Debug.WriteLine($"sd.Length == 9  sd[3] {sd[3]}  =====> {cleanItem} ");

                            writeData2DatabaseAsync("CLUSTER", sd[2], sd[4], classUtilities.Frequency2Band(sd[3]), double.Parse(sd[3]), sd[5], dxccid, entity, sd[8], comment, start1Site);
                        }

                        else if (sd.Length == 10)
                        {
                            //  Debug.WriteLine($"sd.Length == 10  sd[3] {sd[3]}  sd[4] {sd[4]}  ----> {cleanItem}   ");
                            string entity = classUtilities.call2entity(sd[4]);
                            string dxcc = classUtilities.Entity2DXCCID(entity);
                            int dxccid = int.Parse(dxcc);

                            writeData2DatabaseAsync("CLUSTER", sd[2], sd[4], classUtilities.Frequency2Band(sd[3]), double.Parse(sd[3]), sd[5], dxccid, entity, sd[9], comment, start1Site);
                        }

                        else if (sd.Length == 11)
                        {
                            // Debug.WriteLine("sd.Length ==11");
                            string entity = classUtilities.call2entity(sd[4]);
                            string dxcc = classUtilities.Entity2DXCCID(entity);
                            int dxccid = int.Parse(dxcc);

                            //writeData2DatabaseAsync("CLUSTER", sd[2], sd[4], classUtilities.Frequency2Band(sd[3]), double.Parse(sd[3]), sd[5], dxccid, entity, sd[10], sd[6], start1Site);
                            writeData2DatabaseAsync("CLUSTER", sd[2], sd[4], classUtilities.Frequency2Band(sd[3]), double.Parse(sd[3]), sd[5], dxccid, entity, sd[10], comment, start1Site);
                        }

                        else if (sd.Length == 12)
                        {
                            // Debug.WriteLine("sd.Length == 12");
                            string entity = classUtilities.call2entity(sd[4]);
                            string dxcc = classUtilities.Entity2DXCCID(entity);
                            int dxccid = int.Parse(dxcc);

                            writeData2DatabaseAsync("CLUSTER", sd[2], sd[4], classUtilities.Frequency2Band(sd[3]), double.Parse(sd[3]), sd[5], dxccid, entity, sd[11], comment, start1Site);
                        }
                    }
                }
                //else if (cleanItem.StartsWith("WWV"))
                else if (cleanItem.Substring(0, 3) == "WWV")
                {
                    richTextBoxMessages.AppendText($"{rn} WWV: {cleanItem}\r\n");
                    richTextBoxMessages.SelectionStart = richTextBoxMessages.Text.Length;
                    richTextBoxMessages.ScrollToCaret();
                    Debug.WriteLine(cleanItem);

                    writeData2DatabaseAsync("CLUSTER", "WWV", "WX", "NIL", 0, "CW", 291, "United States", rn, cleanItem, "FtCollins");
                }
                else if (cleanItem.Substring(0, 3) == "WCY")
                {
                    richTextBoxMessages.AppendText($"{rn} WCY: {cleanItem}\r\n");
                    richTextBoxMessages.SelectionStart = richTextBoxMessages.Text.Length;
                    richTextBoxMessages.ScrollToCaret();
                    Debug.WriteLine(cleanItem);

                    writeData2DatabaseAsync("CLUSTER", "WCY", "WX", "NIL", 0, "CW", 230, "Germany", rn, cleanItem, "DK0WCY");
                }
                //else if (cleanItem.StartsWith("To ALL"))
                else if (cleanItem.Substring(0, 2) == "To")
                {

                    richTextBoxMessages.AppendText($"{rn} To: {cleanItem}\r\n");
                    richTextBoxMessages.SelectionStart = richTextBoxMessages.Text.Length;
                    richTextBoxMessages.ScrollToCaret();
                    Debug.WriteLine(cleanItem);

                    //  writeData2DatabaseAsync("CLUSTER", "WWV", "WWV", "NIL", 0, "CW", 291, "United States", "sd11", cleanItem, "Start1Site");
                }
                else
                {
                    //richTextBoxMessages.AppendText($"{rn} ?? {cleanItem}\r\n");
                    //richTextBoxMessages.SelectionStart = richTextBoxMessages.Text.Length;
                    //richTextBoxMessages.ScrollToCaret();

                    // Debug.WriteLine(cleanItem);

                }


            }
            catch (Exception e)
            {

                Debug.WriteLine($"Send2DatabaseAsync(string cleanItem) {cleanItem}  \r\n{e}");
            }
        }


        private string IsolateData(string input)
        {
            try
            {

                // Split the string by spaces
                string[] words = input.Split(' ');

                // Check if there are at least six words in the array
                if (words.Length >= 6)
                {
                    // Join the words starting from the sixth word up to the second-to-last word
                    string isolatedData = string.Join(" ", words, 5, words.Length - 6 + 1);
                    string isoData = isolatedData.Substring(0, isolatedData.Length - 5);
                    isoData = isoData.Replace("'", " ");

                    //return isolatedData;
                    return isoData;
                }
                else
                {
                    // Return an empty string or handle the case where there are not enough words
                    return string.Empty;
                }
            }
            catch (Exception)
            {

                Debug.WriteLine("In IsolateData ");
                return "";
            }

        }













        //private async Task writeData2Database(string table, string DE, string DX, string BAND, double FREQUENCY, string MODE, int DXCCID, string ENTITY, string ZULUTIME, string COMMENT, string SOURCE)
        private async Task writeData2DatabaseAsync(string table, string DE, string DX, string BAND, double FREQUENCY, string MODE, int DXCCID, string ENTITY, string ZULUTIME, string COMMENT, string SOURCE)
        {

            try
            {
                SqlConnection conn;
                SqlDataReader rdr = null;

                conn = new SqlConnection(connectionString);  //connectionString is a global ATM
                SqlCommand insertReadings = new SqlCommand();

                //Debug.WriteLine($"insert into {table} (DE,DX,BAND,FREQUENCY,MODE,DXCCID,ENTITY,ZULUTIME,COMMENT,SOURCE) values ('{DE}','{DX}','{BAND}',{FREQUENCY},'{MODE}',DXCCID,'{ENTITY}','{ZULUTIME}','{COMMENT}','{SOURCE}');");

                SqlCommand writeToTable = new SqlCommand($"insert into {table} (DE,DX,BAND,FREQUENCY,MODE,DXCCID,ENTITY,ZULUTIME,COMMENT,SOURCE) values ('{DE}','{DX}','{BAND}',{FREQUENCY},'{MODE}',{DXCCID},'{ENTITY}','{ZULUTIME}','{COMMENT}','{SOURCE}');", conn);
                conn.Open();
                writeToTable.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ERROR in write data to writeData2DatabaseAsync  DE:{DE} DX:{DX} {BAND} {FREQUENCY} {MODE} {DXCCID} {ENTITY} {ZULUTIME} {COMMENT} {SOURCE} \r\n{e}");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            readData2DatabaseAsync("CLUSTER");
        }

        private async Task readData2DatabaseAsync(string table)
        {
            //string connectionString = "Data Source=your_server;Initial Catalog=your_database;User ID=your_username;Password=your_password";

            // Example: Read data from a table
            string query = "SELECT count(*) as COUNT FROM CLUSTER";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Access columns using reader["ColumnName"] or reader[index]
                            int count = (int)reader["COUNT"];
                            //string name = (string)reader["Name"];

                            // Debug.WriteLine($"Count: {count}");//, Name: {name}");

                            if (InvokeRequired)
                            {
                                Invoke(new Action(() => lblCurrentCount.Text = count.ToString()));
                            }

                            lblCurrentCount.Text = count.ToString();
                        }
                    }
                }
                connection.CloseAsync();
            }


        }

        private void comboBoxStart1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Location = Properties.Settings.Default.frmLocation;
            comboBoxStart1.Text = Properties.Settings.Default.propLastConnection;
            GetTelnetClusers();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.frmLocation = this.Location;
            Properties.Settings.Default.propLastConnection = comboBoxStart1.Text;
            Properties.Settings.Default.Save();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            SqlConnection conn;
            SqlDataReader rdr = null;

            string recs = "";
            if (radioOlder2days.Checked == true)
                recs = "2";
            else if (radioOlder4days.Checked == true)
                recs = "4";
            else if (radioOlder6days.Checked == true)
                recs = "6";
            else if (radioOlder10days.Checked == true)
                recs = "10";



            string query = $@"
delete from CLUSTER where cast(LDATE as DATE) < cast(getdate() - {recs}  as DATE) 


";

            conn = new SqlConnection(connectionString);  //connectionString is a global ATM
            SqlCommand insertReadings = new SqlCommand();

            //Debug.WriteLine($"insert into {table} (DE,DX,BAND,FREQUENCY,MODE,DXCCID,ENTITY,ZULUTIME,COMMENT,SOURCE) values ('{DE}','{DX}','{BAND}',{FREQUENCY},'{MODE}',DXCCID,'{ENTITY}','{ZULUTIME}','{COMMENT}','{SOURCE}');");



            SqlCommand writeToTable = new SqlCommand(query, conn);
            conn.Open();
            writeToTable.ExecuteNonQuery();
            conn.Close();
        }

        private void btnMaintainClusterFile_Click(object sender, EventArgs e)
        {
            frmMaintainTelnetCluster mt = new frmMaintainTelnetCluster();
            mt.Show();


        }


        private void GetTelnetClusers()
        {

            bool bFileExists = false;
            string fileToLoad = $@"D:\Users\Cooky\Documents\Slotsky\System\telnetClusters.xml";

            bFileExists = File.Exists(fileToLoad);

            if (bFileExists)
            {


                //string fileToLoad = Properties.Settings.Default.propClusterSearchOverridesFile;
                //string[] row = new string[] { "", "", "", "", "" };
                string cmboBoxDropdownItem = "";

                if (fileToLoad.Length > 0)
                {
                    List<classTelnetCluster> ddnotes;
                    XmlSerializer serialser = new XmlSerializer(typeof(List<classTelnetCluster>));
                    using (StreamReader reader = new StreamReader(fileToLoad))
                    {
                        ddnotes = (List<classTelnetCluster>)serialser.Deserialize(reader);
                    }

                    //OK - now we have the data..put it back into the the comboBox
                    foreach (classTelnetCluster or in ddnotes)
                    {
                        //row = new string[] { or.address, or.port, or.name, or.type, or.comment };

                        cmboBoxDropdownItem = $"{or.address},{or.port},{or.name},{or.type}";
                        comboBoxStart1.Items.Add(cmboBoxDropdownItem);

                    }
                }
            }
            else
                comboBoxStart1.Items.Add("dxc.sm7iun.se,7300,SM7UIN,CCCluster");
        }

        private void chkShowMessages_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void btnSendToCluster_Click(object sender, EventArgs e)
        {
            SendMessageAsync(cmboSendToCluster.Text);
        }




        //----------------------  2023-12-31 14:44    This is Socket comms  ---------------------v-------------------i------
        private TcpListener tcpListener;
        private Thread listenerThread;

        private void InitializeListener()
        {
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12346);
            listenerThread = new Thread(new ThreadStart(ListenForClients));
            listenerThread.Start();
        }

        private void ListenForClients()
        {
            tcpListener.Start();

            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object clientObj)
        {
            TcpClient tcpClient = (TcpClient)clientObj;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                    break;

                string receivedText = Encoding.UTF8.GetString(message, 0, bytesRead);
                UpdateComboBox(receivedText);
            }

            tcpClient.Close();
        }

        private void UpdateComboBox(string text)
        {
            if (cmboSendToCluster.InvokeRequired)
            {
                cmboSendToCluster.Invoke(new Action<string>(UpdateComboBox), text);
            }
            else
            {
                // cmboSendToCluster.Items.Add(text);
                cmboSendToCluster.Text = $"{text}";
            }
        }











    }//end
}