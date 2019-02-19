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

        public EditTableControl()
        {
            InitializeComponent();
        }

        public EditTableControl(int ID)
        {
            InitializeComponent();

            _id = ID;
            LoadPlc();

            CreateTable();

        }

        private void CreateTable()
        {
            BuildRow();
            dataGridView1.Rows.Add(10);
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
                    _ip = dr.GetString(dr.GetOrdinal("Desc"));
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
    }
}
