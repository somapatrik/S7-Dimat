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
using System.Data.SQLite;

namespace S7_Dimat
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            LoadLocalDB();
            LoadTree();
        }

        private void LoadTree()
        {
            treeView1.Nodes.Clear();
            DBLite db = new DBLite("select PLC.ID as ID, PLC.Name as Name, PLC_type.Name as Type from PLC left join PLC_Type on PLC.Type=PLC_type.ID");
            using (SQLiteDataReader dr = db.ExecReader())
            {
                while (dr.Read()) { 
                TreeNode node = new TreeNode(dr.GetString(dr.GetOrdinal("Name")) + " [" + dr.GetString(dr.GetOrdinal("Type")) + "]");
                node.Tag = dr.GetInt32(dr.GetOrdinal("ID"));
                treeView1.Nodes.Add(node);
                }
            }
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

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                
                Point p = new Point(e.X, e.Y);
                TreeNode node = treeView1.GetNodeAt(p);
                if (node != null)
                {
                    treeView1.SelectedNode = node;
                    context_plclist.Show(treeView1, p);
                }
                else
                {
                    context_new.Show(MousePosition);
                }
            }
        }

        private void smazatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Opravdu chcete smazat toto PLC?", "Fakt smazat?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int id = Convert.ToInt32(treeView1.SelectedNode.Tag);

            if (id >= 0)
            {
                // Pak dodělat signály
                DBLite db = new DBLite("delete from PLC where ID=@id");
                db.AddParameter("id", id, DbType.Int32);
                db.Exec();
            }

            LoadTree();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                TreeNode node = treeView1.SelectedNode;
                int id = Convert.ToInt32(node.Tag);

                UserControl control = new EditTableControl(id);
                control.Dock = DockStyle.Fill;

                // Close old control with respect
                if (splitContainer1.Panel2.Controls.Count > 0)
                {
                    EditTableControl old = (EditTableControl)splitContainer1.Panel2.Controls["EditTableControl"];
                    old.EasyClose();
                }

                splitContainer1.Panel2.Controls.Clear();

                splitContainer1.Panel2.Controls.Add(control);
            }
        }

        private void novéToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NewPLC add = new NewPLC();
            if (add.ShowDialog() == DialogResult.OK)
            {
                LoadTree();
            }
        }
    }
}
