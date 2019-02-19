using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7_Dimat.Class;
using System.Data.SQLite;
using System.Threading;

namespace S7_Dimat
{
    public partial class EditTableControl : UserControl
    {

        private int _id;
        private string _name;
        private string _ip;
        private int _rack;
        private int _slot;

        private Plc _plc;
        private Plc.S7Type _type;

        private Boolean run;

        private Thread mythread;

        public EditTableControl()
        {
            InitializeComponent();
        }

        public EditTableControl(int ID)
        {
            InitializeComponent();
            // Načte z DB
            _id = ID;
            LoadPlc();
            // Vytvoří datagrid
            CreateTable();
            // Vlákno pro čtení
            mythread = new Thread(new ThreadStart(ThreadWork));
            // PLC globálně
            _plc = new Plc(_ip, _rack, _slot);
            _plc.Type = _type;

        }

        // Připojit
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!run)
            {              
                if (_plc.Connect())
                {
                    run = true;
                    textToolStripMenuItem.Enabled = false;
                     mythread.Start();
                    //_plc.test();
                } else
                {
                    MessageBox.Show("Nelze se připojit k PLC", "Chyba připojení PLC");
                }
            }
        }

        private void odpojitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run = false;
            //if (mythread.IsAlive)
            //{
            //    mythread.Abort();
            //}
            
            textToolStripMenuItem.Enabled = true;
        }

        // Vlákno čtení
        private void ThreadWork()
        {
            while (run)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["address"].Value != null)
                    { 
                        // Přečti řádek
                        string addr = row.Cells["address"].Value.ToString();
                        string format = row.Cells["format"].Value.ToString();
                        int rowindex = row.Index;
                        // Hodnota z PLC
                        //string resvalue =System.Text.Encoding.UTF8.GetString(_plc.GetValue(addr));
                        string resvalue = _plc.GetValue(addr);
                        // Update datagridview
                        SendResult dResult = new SendResult(ShowResult);
                        this.Invoke(dResult, rowindex, resvalue);
                    }
                }
                Thread.Sleep(100);
            }
        }

        private delegate void SendResult(int index, string result);
        private void ShowResult(int index, string result)
        {
            dataGridView1.Rows[index].Cells["result"].Value = result;
        }

        private void CreateTable()
        {
            BuildRow();
            dataGridView1.Rows.Add(9);
        }

        private void BuildRow()
        {

            DataTable dt = new DataTable();

            DataGridViewComboBoxColumn cmb_ResulType = new DataGridViewComboBoxColumn();
            cmb_ResulType.HeaderText = "Výstupní formát";
            cmb_ResulType.Name = "format";

            DataGridViewTextBoxColumn Addr = new DataGridViewTextBoxColumn();
            Addr.HeaderText = "Adresa";
            Addr.Name = "address";

            DataGridViewTextBoxColumn Result = new DataGridViewTextBoxColumn();
            Result.HeaderText = "Výsledek";
            Result.Name = "result";
            Result.ReadOnly = true;
            Result.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns.Add(Addr);
            dataGridView1.Columns.Add(cmb_ResulType);
            dataGridView1.Columns.Add(Result);

        }

        private void EditTableControl_Load(object sender, EventArgs e)
        {
            LoadGUI();
        }

        private void LoadGUI()
        {
            toolStripTextBox1.Text = _name;
            toolStripTextBox2.Text = _ip;
        }

        private void LoadPlc()
        {
            DBLite db = new DBLite("select Name, Desc, IP, Rack, Slot, Type from PLC where ID=@id");
            db.AddParameter("id", _id, DbType.Int32);
            using (SQLiteDataReader dr = db.ExecReader())
            {
                if (dr.Read())
                {
                    _name = dr.GetString(dr.GetOrdinal("Name"));
                    _ip = dr.GetString(dr.GetOrdinal("IP"));
                    _rack = dr.GetInt32(dr.GetOrdinal("Rack"));
                    _slot = dr.GetInt32(dr.GetOrdinal("Slot"));

                    switch (dr.GetString(dr.GetOrdinal("Type")))
                    {
                        case "s7-1500":
                            _type = Plc.S7Type.S71500;
                            break;
                        case "s7-1200":
                            _type = Plc.S7Type.S71200;
                            break;
                        case "s7-400":
                            _type = Plc.S7Type.S7400;
                            break;
                        case "s7-300":
                            _type = Plc.S7Type.S7300;
                            break;
                    }
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {

                DataGridViewCell inputcell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string input = inputcell.Value == null ? "" : inputcell.Value.ToString();
                DataGridViewCellStyle style = new DataGridViewCellStyle();

                DataGridViewComboBoxCell cmbtype = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells[1];
                cmbtype.Items.Clear();

                if (!string.IsNullOrEmpty(input))
                {
                    inputcell.Value = input.ToUpper();
                    InputFormatter format = new InputFormatter(input);
                    if (format.IsValid)
                    {
                        if (format.IsBit)
                        {
                            cmbtype.Items.Add("BIN");
                            cmbtype.Items.Add("BOOL");
                            cmbtype.Items.Add("DEC");
                        }
                        else if (format.IsByte || format.IsWord)
                        {
                            cmbtype.Items.Add("BIN");
                            cmbtype.Items.Add("DEC");
                            cmbtype.Items.Add("HEX");
                        }
                        else if (format.IsWord)
                        {
                            cmbtype.Items.Add("BIN");
                            cmbtype.Items.Add("DEC");
                            cmbtype.Items.Add("HEX");
                            cmbtype.Items.Add("FLOAT");
                        }

                        if (cmbtype.Value == null)
                        {
                            cmbtype.Value = "BIN";
                        }
                    }
                    else
                    {
                        style.BackColor = Color.LightCoral;
                    }
                }
                else
                {
                    style.BackColor = Color.White;
                }

                inputcell.Style = style;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                int act = dataGridView1.CurrentRow.Index;
                int last = dataGridView1.RowCount - 1;

                if (act == last)
                {
                    dataGridView1.Rows.Add();
                }
                else if (dataGridView1.Rows[act + 1].Cells[0].Value != null)
                {
                    dataGridView1.Rows.Insert(act+1);
                    dataGridView1.CurrentCell = dataGridView1.Rows[act].Cells[dataGridView1.CurrentCell.ColumnIndex];
                }
            }
        }


    }
}
