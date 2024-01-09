using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SlotskyDXCluster
{
    public partial class frmMaintainTelnetCluster : Form
    {
        public frmMaintainTelnetCluster()
        {
            InitializeComponent();
            MakeThedgvOV();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string fileToLoad = Properties.Settings.Default.propClusterSearchOverridesFile;
            string fileToLoad = $@"D:\Users\Cooky\Documents\Slotsky\System\telnetClusters.xml";
            string[] row = new string[] { "", "", "", "", " " };


            if (fileToLoad.Length > 0)
            {
                List<classTelnetCluster> ddnotes;
                XmlSerializer serialser = new XmlSerializer(typeof(List<classTelnetCluster>));
                using (StreamReader reader = new StreamReader(fileToLoad))
                {
                    ddnotes = (List<classTelnetCluster>)serialser.Deserialize(reader);
                }

                //OK - now we have the data..put it back into the form
                foreach (classTelnetCluster or in ddnotes)
                {
                    row = new string[] { or.address, or.port, or.name, or.type, or.comment };
                    dgvOV.Rows.Add(row);
                }
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<classTelnetCluster> clusters = new List<classTelnetCluster>();   //this is our list of T
            foreach (DataGridViewRow row in dgvOV.Rows)   //put the datagridview into the list of T
            {
                //if (row.Cells["dgvOVcomment"].Value.ToString().Length == 0)
                //    row.Cells["dgvOVcomment"].Value = "Description";

                if (row.Cells["dgvOVaddress"].Value != null)
                {
                    clusters.Add(new classTelnetCluster()
                    {
                        address = row.Cells["dgvOVaddress"].Value.ToString()
                        ,
                        port = row.Cells["dgvOVport"].Value.ToString()
                        ,
                        name = row.Cells["dgvOVname"].Value.ToString()
                        ,
                        type = row.Cells["dgvOVtype"].Value.ToString()
                        ,

                        comment = row.Cells["dgvOVcomment"].Value.ToString()
                    });
                }
            }

            //now save the list of T to the xml file
            //            string fname = $@"{Properties.Settings.Default.SlotskyFolder}clusterSearchOverrides.xml";
            //string fname = Properties.Settings.Default.propClusterSearchOverridesFile;
            string fname = $@"D:\Users\Cooky\Documents\Slotsky\System\telnetClusters.xml";
            XmlSerializer serialiser = new XmlSerializer(typeof(List<classTelnetCluster>));
            using (StreamWriter writer = new StreamWriter(fname))   //FNAME IS A STRING THAT IS THE FILNAME TO SAVE TO EG C:\DXNOTES.XML
            {
                serialiser.Serialize(writer, clusters);
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void helpTextToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MakeThedgvOV()
        {

            //create the datagridview stuff in here to dgvDXP
            dgvOV.Rows.Clear();
            dgvOV.Columns.Clear();

            dgvOV.Columns.Add("dgvOVaddress", "Address");
            dgvOV.Columns.Add("dgvOVport", "Port");
            dgvOV.Columns.Add("dgvOVname", "Name");
            dgvOV.Columns.Add("dgvOVtype", "Type");
            dgvOV.Columns.Add("dgvOVcomment", "Comment");

            dgvOV.Columns["dgvOVaddress"].Width = 140;
            dgvOV.Columns["dgvOVport"].Width = 65;
            dgvOV.Columns["dgvOVname"].Width = 100;
            dgvOV.Columns["dgvOVtype"].Width = 100;
            dgvOV.Columns["dgvOVcomment"].Width = 320;

            dgvOV.RowHeadersVisible = true;
            dgvOV.Dock = System.Windows.Forms.DockStyle.Fill;


            // dgvOV.Rows.Add(10);
            dgvOV.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            var lastCol = dgvOV.Columns["dgvOVcomment"];// this is the last column in the grid
            lastCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;




        }

    }
}
