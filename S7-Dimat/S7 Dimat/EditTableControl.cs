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
using System.Net.NetworkInformation;

namespace S7_Dimat
{
    public partial class EditTableControl : UserControl
    {
        // PLC
        private int _id;
        private string _name;
        private string _ip;
        private int _rack;
        private int _slot;
        // PLC client
        private Plc _plc;
        private Plc.S7Type _type;
        // Threads
        private Thread mythread;
        private Thread pingthread;
        // List of 'global' variables
        private List<int> usedrows = new List<int>();
        private List<string> usedseries = new List<string>();
        // Scanning PLC
        private Boolean irun;

        private Boolean run
        {
            set
            {
                irun = value;
                //toolStripStatusLabel1.Text = value ? "" : "";
                statusStrip1.BackColor = value ? Color.LawnGreen : Color.Orange;
            }
            get
            {
                return irun;
            }
        }

        public EditTableControl()
        {
            InitializeComponent();
        }

        public EditTableControl(int ID)
        {
            InitializeComponent();

            // ID inside DB
            _id = ID;
            LoadPlc();
            // Creates DGV
            CreateTable();
            // PLC global object
            _plc = new Plc(_ip, _rack, _slot);
            _plc.Type = _type;
        }

        private void EditTableControl_Load(object sender, EventArgs e)
        {
            LoadGUI();
            LoadSignals();
        }

        private void LoadSignals()
        {
            int plcid = GetPlcID();

            DBLite db = new DBLite("select Addr, Desc, repre from PLC_Signal where PLC=@id");
            db.AddParameter("id", plcid, DbType.Int32);
            using (SQLiteDataReader dr = db.ExecReader())
            {
                while (dr.Read())
                {
                    dataGridView1.Rows.Add();
                    int newrow = dataGridView1.Rows.Count - 2;

                    string addr = "";
                    string desc = "";
                    string repre = "";

                    if (!dr["Addr"].Equals(DBNull.Value))
                    {
                        addr = dr.GetString(dr.GetOrdinal("Addr"));
                    }


                    if (!dr["Desc"].Equals(DBNull.Value))
                    {
                        desc = dr.GetString(dr.GetOrdinal("Desc"));
                    }


                    if (!dr["repre"].Equals(DBNull.Value))
                    {
                        repre = dr.GetString(dr.GetOrdinal("repre"));
                    }

                    dataGridView1.Rows[newrow].Cells["address"].Value = string.IsNullOrEmpty(addr) ? null : addr;
                    dataGridView1.Rows[newrow].Cells["desc"].Value = desc;

                    if (!string.IsNullOrEmpty(addr))
                    {
                        dataGridView1_CellEndEdit(dataGridView1, new DataGridViewCellEventArgs(dataGridView1.Rows[newrow].Cells["address"].ColumnIndex, dataGridView1.Rows[newrow].Index));
                    }

                    dataGridView1.Rows[newrow].Cells["format"].Value = repre.ToUpper();
                    
                }
                dataGridView1.Update();
            }
        }

        private void LoadGUI()
        {
            toolStripTextBox1.Text = _name;
            toolStripTextBox2.Text = _ip;
            toolStripStatusLabel1.Text = "";

            LoadPlotter();
        }

        private void LoadPlotter()
        {
            chart1.BackColor = Color.Black;
            chart1.ForeColor = Color.White;
            chart1.ChartAreas[0].BackColor = Color.Black;
            chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            chart1.ChartAreas[0].AxisX.LineColor = Color.White;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";

            chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            chart1.ChartAreas[0].AxisY.LineColor = Color.White;

            chart1.ChartAreas[0].AxisX.Interval = 3;
            chart1.ChartAreas[0].AxisX.IntervalOffset = 1;
            
            chart1.ChartAreas[0].AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;

        }

        // Connect to PLC
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mythread != null)
            {
                if (run)
                {
                    run = false;
                }

                if (mythread.IsAlive)
                {
                    Thread.Sleep(100);
                }
            }

            if (!run)
            {
                mythread = new Thread(new ThreadStart(ThreadWork));
                pingthread = new Thread(new ThreadStart(PingThread));

                if (_plc.Connect())
                {
                    run = true;
                    textToolStripMenuItem.Enabled = false;
                    uložitToolStripMenuItem.Enabled = false;
                    PrepareChart();
                    mythread.Start();
                    pingthread.Start();
                } else
                {
                    MessageBox.Show("Nelze se připojit k PLC", "Chyba připojení PLC");
                }
            }
        }

        private void PrepareChart()
        {
            // Delete series
            chart1.Series.Clear();
            chart1.Legends.Clear();
            //Delete global variables
            usedseries.Clear();
        }

        private void odpojitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run = false;
            textToolStripMenuItem.Enabled = true;
            uložitToolStripMenuItem.Enabled = true;
        }

        private void PingThread()
        {
            while (run)
            {
                Ping ping = new Ping();
                try
                {
                    PingReply reply = ping.Send(this._ip,1000);
                    if (reply.Status != IPStatus.Success)
                    {
                        run = false;
                        return;
                    }
                } catch (Exception ex)
                {
                    run = false;
                    return;
                }
                Thread.Sleep(3000);
            }
        }

        // Background reading
        private void ThreadWork()
        {
            while (run)
            {

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["address"].Value != null && row.Cells["IsValid"].Value.ToString() == "1")
                    {
                       
                        // Read row
                        string idrow = row.Cells["idrow"].Value.ToString();
                        string addr = row.Cells["address"].Value.ToString();
                        string format = row.Cells["format"].Value.ToString();

                        string GraphName = addr;
                        // Get name for the graph
                        if (row.Cells["desc"].Value != null)
                        {
                            if (!string.IsNullOrEmpty(row.Cells["desc"].Value.ToString()))
                            {
                                GraphName += " (" + row.Cells["desc"].Value.ToString() + ")";
                            }
                        }

                        // Raw byte from PLC
                        byte[] resbyte = _plc.GetValue(addr);
                        // Formated user output
                        string resvalue = "";
                        // Read in format
                        switch (format.ToUpper())
                        {
                            case "BOOL":
                                resvalue = _plc.GetBooltS(resbyte);
                                break;
                            case "DEC":
                                resvalue = _plc.GetDecS(resbyte);
                                break;
                            case "BIN":
                                resvalue = _plc.GetBinS(resbyte);
                                break;
                            case "FLOAT":
                                resvalue = _plc.GetFloatS(resbyte);
                                break;
                            default:
                                resvalue = "Chyba formátu";
                                break;
                        }
                        
                        // Update datagridview
                        SendResult dResult = new SendResult(ShowResult);
                        this.Invoke(dResult, idrow, resvalue);

                        // Insert into chart
                        PlotResult dPlot = new PlotResult(InsertPlotPoint);
                        this.Invoke(dPlot, GraphName, resvalue, format.ToUpper());
                    }
                }
                Thread.Sleep(100);
                // Update chart
                UpdateChart dupdate = new UpdateChart(UpdateChartPoints);
                this.Invoke(dupdate);
            }
        }

        private delegate void SendResult(string index, string result);
        private void ShowResult(string index, string result)
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["idrow"].Value.ToString() == index)
                {
                    row.Cells["result"].Value = result;
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    if (result.ToUpper() == "TRUE")
                    {
                        style.BackColor = Color.LawnGreen;
                        row.Cells["result"].Style = style;
                    }
                    else
                    {
                        style.BackColor = Color.White;
                        row.Cells["result"].Style = style;
                    }
                    break;
                }
            }
        }

        private delegate void PlotResult(string name, string value, string format);
        private void InsertPlotPoint(string name, string value, string format)
        {
            // If not needed do not use
            if (splitContainer1.Panel2Collapsed)
            {
                return;
            }

            // Must exists
            if (chart1.Series.IndexOf(name) == -1)
            {
                usedseries.Add(name);
                // Series
                chart1.Series.Add(name);
                chart1.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
                chart1.Series[name].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chart1.Series[name].BorderWidth = 5;
                //Legend
                chart1.Legends.Add(name);
                chart1.Legends[name].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            }

            DateTime act = DateTime.Now;

            //Get value from string
            switch (format)
            {
                case "BOOL":
                    int boolval = value.ToUpper() == "TRUE" ? 1 : 0;
                    chart1.Series[name].Points.AddXY(act, boolval);
                    break;
                case "DEC":
                    int intval = Int32.Parse(value);
                    chart1.Series[name].Points.AddXY(act, intval);
                    break;
                case "FLOAT":
                    float fvalue = float.Parse(value);
                    chart1.Series[name].Points.AddXY(act, fvalue);
                    break;
            }

            DateTime MinDate = act.AddSeconds(-30);
            if (chart1.ChartAreas[0].AxisX.Minimum < MinDate.ToOADate())
            {
                chart1.ChartAreas[0].AxisX.Minimum = MinDate.ToOADate();
            }

            // Recalculate range
            chart1.ChartAreas[0].RecalculateAxesScale();

        }

        private delegate void UpdateChart();
        private void UpdateChartPoints()
        {
            
            chart1.Update();
        }

        private void CreateTable()
        {
            BuildRow();
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

            DataGridViewTextBoxColumn label = new DataGridViewTextBoxColumn();
            label.HeaderText = "Popisek";
            label.Name = "desc";

            DataGridViewTextBoxColumn Result = new DataGridViewTextBoxColumn();
            Result.HeaderText = "Výsledek";
            Result.Name = "result";
            Result.ReadOnly = true;
            Result.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewTextBoxColumn valid = new DataGridViewTextBoxColumn();
            valid.Name = "IsValid";
            valid.Visible = false;

            DataGridViewTextBoxColumn idrow = new DataGridViewTextBoxColumn();
            idrow.Name = "idrow";
            idrow.Visible = false;

            dataGridView1.Columns.Add(idrow);
            dataGridView1.Columns.Add(valid);
            dataGridView1.Columns.Add(Addr);
            dataGridView1.Columns.Add(label);
            dataGridView1.Columns.Add(cmb_ResulType);
            dataGridView1.Columns.Add(Result);
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

            if (e.ColumnIndex == dataGridView1.Columns["address"].Index)
            {
                dataGridView1.Rows[e.RowIndex].Cells["IsValid"].Value = "0";

                DataGridViewCell inputcell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string input = inputcell.Value == null ? "" : inputcell.Value.ToString();
                DataGridViewCellStyle style = new DataGridViewCellStyle();

                DataGridViewComboBoxCell cmbtype = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells["format"];

                if (!string.IsNullOrEmpty(input))
                {
                    inputcell.Value = input.ToUpper();
                    InputFormatter format = new InputFormatter(input);
                    if (format.IsValid)
                    {

                        dataGridView1.Rows[e.RowIndex].Cells["IsValid"].Value = "1";

                        string defvalue = "DEC";
                        cmbtype.Value = null;
                        cmbtype.Items.Clear();

                        if (format.IsBit)
                        {
                            //cmbtype.Items.Add("BIN");
                            cmbtype.Items.Add("BOOL");
                            //cmbtype.Items.Add("DEC");
                            defvalue = "BOOL";
                        }
                        else if (format.IsByte || format.IsWord)
                        {
                            cmbtype.Items.Add("BIN");
                            cmbtype.Items.Add("DEC");
                           // cmbtype.Items.Add("HEX");
                        }
                        else if (format.IsDouble)
                        {
                            cmbtype.Items.Add("BIN");
                            cmbtype.Items.Add("DEC");
                           /// cmbtype.Items.Add("HEX");
                           cmbtype.Items.Add("FLOAT");
                        }

                        if (cmbtype.Value == null)
                        {
                            cmbtype.Value = defvalue;
                        }
                    }
                    else
                    {
                        cmbtype.Value = null;
                        cmbtype.Items.Clear();
                        style.BackColor = Color.LightCoral;
                    }
                }
                else
                {
                    cmbtype.Value = null;
                    cmbtype.Items.Clear();
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
                else if (dataGridView1.Rows[act + 1].Cells["address"].Value != null)
                {
                    dataGridView1.Rows.Insert(act+1);
                    dataGridView1.CurrentCell = dataGridView1.Rows[act].Cells[dataGridView1.CurrentCell.ColumnIndex];
                }
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int index = 1;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["idrow"].Value == null)
                {
                    while (usedrows.Contains(index))
                    {
                        index++;
                    }
                    dataGridView1.Rows[row.Index].Cells["idrow"].Value = index;
                    usedrows.Add(index);
                }
            }
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            usedrows.Remove(Convert.ToInt32(e.Row.Cells["idrow"].Value));
        }

        public void EasyClose()
        {
            if (mythread != null)
            {
                run = false;
                if (mythread.IsAlive)
                {
                    Thread.Sleep(100);
                    if (mythread.IsAlive)
                    {
                        mythread.Abort();
                        if (_plc != null)
                        {
                            if (_plc.Connected)
                            {
                                _plc.Disconnect();
                            }
                        }
                    }
                }
            }
        }

        private void uložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int plcid = GetPlcID();
            DeleteSignals(plcid);
            SaveSignals(plcid);
        }

        private void SaveSignals(int PlcID)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows) {

                string desc = "";
                string repre = "";
                string addr = "";

                if (row.Cells["desc"].Value != null)
                {
                    desc = row.Cells["desc"].Value.ToString();
                }

                if (row.Cells["address"].Value != null)
                {
                    addr = row.Cells["address"].Value.ToString();
                }

                if (row.Cells["format"].Value != null)
                {
                    repre = row.Cells["format"].Value.ToString();
                }
                

                DBLite db = new DBLite("insert into PLC_Signal values (@id, @addr, @desc, @repre)");
                db.AddParameter("id", this._id, DbType.Int32);
                db.AddParameter("addr", addr, DbType.String);
                db.AddParameter("desc", desc, DbType.String);
                db.AddParameter("repre", repre.ToLower(), DbType.String);
                db.Exec();
                db = null;
            }
        }

        private int GetPlcID()
        {
            int id = 0;
            DBLite db = new DBLite("select ID from PLC where Name like @name and IP like @ip");
            db.AddParameter("name", this._name, DbType.String);
            db.AddParameter("ip", this._ip, DbType.String);
            using (SQLiteDataReader dr = db.ExecReader())
            {
                if (dr.Read())
                {
                    id = dr.GetInt32(0);
                }
            }

            return id;
        }

        private void DeleteSignals(int PlcID)
        {
            DBLite db = new DBLite("delete from PLC_Signal where PLC=@id");
            db.AddParameter("id", PlcID, DbType.Int32);
            db.Exec();
        }

        private void grafToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = splitContainer1.Panel2Collapsed ? false : true;
        }
    }
}
