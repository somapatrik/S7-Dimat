using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7_Dimat.Class;

namespace S7_Dimat
{
    public partial class NewPLC : Form
    {
        public NewPLC()
        {
            InitializeComponent();
        }

        private void NewPLC_Load(object sender, EventArgs e)
        {
            LoadTypes();
        }

        private void LoadTypes()
        {
            combo_typ.Items.Clear();
            DBLite db = new DBLite("select ID, Name from PLC_type");
            DataTable dt = db.ExecTable();
            combo_typ.DataSource = dt;
            combo_typ.ValueMember = "ID";
            combo_typ.DisplayMember = "Name";
      //      combo_typ.SelectedIndex = 0;
        }


    }
}
