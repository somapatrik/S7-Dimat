using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace S7_Dimat
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {

            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            label1.Text = Application.ProductName + "\r\n" +
                            "v " + Application.ProductVersion + "\r\n" +
                            versionInfo.LegalCopyright + "\r\n\r\n" +
                            "Využívá otevřený software:" + "\r\n" +
                            "SQLite, Sharp7, Icons8";
        }
    }
}
