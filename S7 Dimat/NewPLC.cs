using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7_Dimat.Class;
using static S7_Dimat.Class.Plc;

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
            combo_typ.SelectedIndex = 0;
        }

        private void combo_typ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo_typ.SelectedIndex >= 0)
            {
                DBLite db = new DBLite("select Def_Rack, Def_Slot from PLC_type where ID like @id");
                db.AddParameter("id", combo_typ.SelectedValue, DbType.String);
                using (SQLiteDataReader dr = db.ExecReader())
                {
                    if (dr.Read()) { 
                        rack.Value = dr.GetValue(0).Equals(DBNull.Value) ? 0 : dr.GetInt32(0);
                        slot.Value = dr.GetValue(1).Equals(DBNull.Value) ? 0 : dr.GetInt32(1);
                    }
                }
            }
        }



        private void btn_test_Click(object sender, EventArgs e)
        {
           if (Test())
            {
                MessageBox.Show("Test ok", "Test připojení", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
        }

        private Boolean Test()
        {
            Boolean res = false;
            Ping pinger = null;
            try
            {
                if (!string.IsNullOrEmpty(txt_ip.Text))
                {
                    pinger = new Ping();
                    PingReply reply = pinger.Send(txt_ip.Text);
                    if (reply.Status == IPStatus.Success)
                    {
                        string msg = "PLC je dosažitelné";

                        S7Type type = S7Type.S7300;

                        switch (combo_typ.SelectedValue)
                        {
                            case "s7-300":
                                type = S7Type.S7300;
                                break;
                            case "s7-400":
                                type = S7Type.S7400;
                                break;
                            case "s7-1200":
                                type = S7Type.S71200;
                                break;
                            case "s7-1500":
                                type = S7Type.S71500;
                                break;
                        }

                        Plc plc = null;

                        try
                        {
                            plc = new Plc(txt_ip.Text, type);
                            plc.Connect();
                            if (plc.Connected)
                            {
                                msg += "\r\nDo PLC se lze připojit";
                                res = true;
                            }
                            else
                            {
                                msg += "\r\nDo PLC se nelze připojit";
                                MessageBox.Show(msg, "Chyba PLC komunikace", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                res = false;
                            }
                        }
                        catch (Exception ex0)
                        {
                            msg += "\r\nChyba připojení do PLC \r\n " + ex0.Message;
                            MessageBox.Show(msg, "Chyba PLC komunikace", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            res = false;
                        }
                        finally
                        {
                            plc.Disconnect();
                            plc = null;
                        }

                    }
                    else
                    {
                        MessageBox.Show("PLC nelze dosáhnout", "Chyba připojení PLC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        res = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba připojení PLC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                res = false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return res;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_ip.Text) && !string.IsNullOrEmpty(txt_name.Text))
            {
                if (Test())
                {
                    DBLite db = new DBLite("insert into PLC values (null, @name, ifnull(@desc,''), @ip, @r, @s, @type)");
                    db.AddParameter("name", txt_name.Text, DbType.String);
                    db.AddParameter("desc", txt_desc.Text, DbType.String);
                    db.AddParameter("ip", txt_ip.Text, DbType.String);
                    db.AddParameter("r", rack.Value, DbType.Int32);
                    db.AddParameter("s", slot.Value, DbType.Int32);
                    db.AddParameter("type", combo_typ.SelectedValue.ToString(), DbType.String);
                    db.Exec();
                }
            }
        }
    }
}
