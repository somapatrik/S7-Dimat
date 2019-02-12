using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using S7_Dimat.Class;

namespace S7_Dimat
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
            dataGridView1.KeyDown += DataGridView1_KeyDown;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            BuildRow();
            dataGridView1.Rows.Add(15);
            Plc plc = new Plc("", Plc.S7Type.S7300);
        }


        private void DataGridView1_KeyDown(object sender, KeyEventArgs e)
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
                    dataGridView1.Rows.Insert(act);
                    dataGridView1.CurrentCell = dataGridView1.Rows[act].Cells[dataGridView1.CurrentCell.ColumnIndex];
                }
            }
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) { 

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
                        } else if (format.IsByte || format.IsWord)
                        {
                            cmbtype.Items.Add("BIN");
                            cmbtype.Items.Add("DEC");
                            cmbtype.Items.Add("HEX");
                        } else if (format.IsWord)
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
                } else
                {
                    style.BackColor = Color.White;
                }

                inputcell.Style = style;
            }
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
    }
}
