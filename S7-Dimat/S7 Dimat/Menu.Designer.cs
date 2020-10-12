namespace S7_Dimat
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btn_NewPlc = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_about = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.txt_version = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.pLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.novéToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.context_plclist = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.změnitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smazatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.context_new = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.novéToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.context_plclist.SuspendLayout();
            this.context_new.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_NewPlc,
            this.btn_about});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1098, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btn_NewPlc
            // 
            this.btn_NewPlc.Image = ((System.Drawing.Image)(resources.GetObject("btn_NewPlc.Image")));
            this.btn_NewPlc.Name = "btn_NewPlc";
            this.btn_NewPlc.Size = new System.Drawing.Size(28, 20);
            this.btn_NewPlc.ToolTipText = "Přidat PLC";
            this.btn_NewPlc.Click += new System.EventHandler(this.btn_NewPlc_Click);
            // 
            // btn_about
            // 
            this.btn_about.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_about.Image = ((System.Drawing.Image)(resources.GetObject("btn_about.Image")));
            this.btn_about.Name = "btn_about";
            this.btn_about.Size = new System.Drawing.Size(28, 20);
            this.btn_about.Click += new System.EventHandler(this.btn_about_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txt_version});
            this.statusStrip1.Location = new System.Drawing.Point(0, 714);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip1.Size = new System.Drawing.Size(1098, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // txt_version
            // 
            this.txt_version.Name = "txt_version";
            this.txt_version.Size = new System.Drawing.Size(45, 17);
            this.txt_version.Text = "version";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1MinSize = 150;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.splitContainer1.Size = new System.Drawing.Size(1098, 690);
            this.splitContainer1.SplitterDistance = 180;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(178, 688);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // pLCToolStripMenuItem
            // 
            this.pLCToolStripMenuItem.Name = "pLCToolStripMenuItem";
            this.pLCToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.pLCToolStripMenuItem.Text = "PLC";
            // 
            // novéToolStripMenuItem
            // 
            this.novéToolStripMenuItem.Name = "novéToolStripMenuItem";
            this.novéToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.novéToolStripMenuItem.Text = "Přidat/Odebrat...";
            // 
            // context_plclist
            // 
            this.context_plclist.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.změnitToolStripMenuItem,
            this.smazatToolStripMenuItem});
            this.context_plclist.Name = "contextMenuStrip1";
            this.context_plclist.Size = new System.Drawing.Size(117, 48);
            // 
            // změnitToolStripMenuItem
            // 
            this.změnitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("změnitToolStripMenuItem.Image")));
            this.změnitToolStripMenuItem.Name = "změnitToolStripMenuItem";
            this.změnitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.změnitToolStripMenuItem.Text = "Edit";
            this.změnitToolStripMenuItem.Click += new System.EventHandler(this.změnitToolStripMenuItem_Click);
            // 
            // smazatToolStripMenuItem
            // 
            this.smazatToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("smazatToolStripMenuItem.Image")));
            this.smazatToolStripMenuItem.Name = "smazatToolStripMenuItem";
            this.smazatToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.smazatToolStripMenuItem.Text = "Delete...";
            this.smazatToolStripMenuItem.Click += new System.EventHandler(this.smazatToolStripMenuItem_Click);
            // 
            // context_new
            // 
            this.context_new.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.novéToolStripMenuItem1});
            this.context_new.Name = "context_new";
            this.context_new.Size = new System.Drawing.Size(130, 26);
            // 
            // novéToolStripMenuItem1
            // 
            this.novéToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("novéToolStripMenuItem1.Image")));
            this.novéToolStripMenuItem1.Name = "novéToolStripMenuItem1";
            this.novéToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
            this.novéToolStripMenuItem1.Text = "Add PLC...";
            this.novéToolStripMenuItem1.Click += new System.EventHandler(this.novéToolStripMenuItem1_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 736);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "S7 Dimat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Menu_FormClosing);
            this.Load += new System.EventHandler(this.Menu_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.context_plclist.ResumeLayout(false);
            this.context_new.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem pLCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem novéToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip context_plclist;
        private System.Windows.Forms.ToolStripMenuItem smazatToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip context_new;
        private System.Windows.Forms.ToolStripMenuItem novéToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem změnitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btn_NewPlc;
        private System.Windows.Forms.ToolStripStatusLabel txt_version;
        private System.Windows.Forms.ToolStripMenuItem btn_about;
    }
}