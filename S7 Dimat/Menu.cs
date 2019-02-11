using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using S7_Dimat.Class;
using Zakázky.Class;
using System.Data.SQLite;

namespace S7_Dimat
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            LoadLocalDB();
        }

        private void LoadLocalDB()
        {
            string currentpath = Path.Combine(Global.ExeDirectory, Global.LocalDBName);

            if (File.Exists(currentpath))
            {
                TestDB();
            } else
            {
                File.WriteAllBytes(Path.Combine(Global.ExeDirectory, Global.LocalDBName), S7_Dimat.Properties.Resources.data);  
            }
        }

        private void TestDB()
        {
            try { 
                DBLite db = new DBLite("select 1");
                using (SQLiteDataReader dr = db.ExecReader())
                {
                    if (dr.Read())
                    {
                        return;
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba čtení z DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
