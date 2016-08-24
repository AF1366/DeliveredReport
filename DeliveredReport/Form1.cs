using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeliveredReport
{
    public partial class Form1 : Form
    {
        #region Global
        int index;
        int count = 0;
        string command;
        string ConnectionAddress;
        string q_count1Ta10;
        string q_count1Ta10Total;

        clsDAL clsdal = new clsDAL();
        clsHelper hlp = new clsHelper();
        DataTable dt2 = new DataTable();
        DataTable dtcheck = new DataTable();
        DataTable dtfinal = new DataTable();        
        Dictionary<int, string> dicDBName = new Dictionary<int, string>();
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTestServer_Click(object sender, EventArgs e)
        {
            clsVal.Password = ""; clsVal.UserID = ""; clsVal.ServerName = "";
            connection(cmbServer,txtUser,txtPass);
            hlp.ComboDBEvent(1, dicDBName, cmbDBName, cmbTBName);
            connection(cmbServer2, txtUser2, txtPass2);
            hlp.ComboDBEvent(1, dicDBName, cmbDBName2, cmbTBName2);
            //cmbTBName.SelectedIndex = 7;
            //cmbFilter.SelectedIndex = 5;
            //cmbType.SelectedIndex = 1;

        }

        public void connection(ComboBox server,TextBox user,TextBox pass)
        {
            if (user.Text=="" & pass.Text=="")
            {
                ConnectionAddress = "Data Source=farahani-pc;Initial Catalog=master;Integrated Security=True";
            }
            else
            {
                ConnectionAddress = "Data Source=" + server.Text + ";Initial Catalog=master;User ID=" + user.Text + ";Password=" + pass.Text;
            }            
            try
            {
                clsdal.sqlcon(ConnectionAddress).Open();
                MessageBox.Show("اتصال با موفقیت", "پیغام");
                clsVal.ConnectionAddress = ConnectionAddress;
                clsVal.ServerName = cmbServer.Text;
                clsVal.Password = txtPass.Text;
                clsVal.UserID = txtUser.Text;
                clsdal.ComboBoxSource(cmbDBName, clsdal.sqlgetdbname(ConnectionAddress));
                clsdal.ComboBoxSource(cmbDBName2, clsdal.sqlgetdbname(ConnectionAddress));
                //dtget = clsdal.sqlgetdbname(ConnectionAddress);
                //DGV2.DataSource = clsdal.sqlgetdbname(ConnectionAddress);
                btnDis.Visible = true;
            }
            catch (Exception ex)
            {
                btnDis.Visible = false;
                MessageBox.Show("اتصال ناموفق", "!خطا");
                clsVal.ServerName = "";
            }
            finally
            {
                clsdal.sqlcon(ConnectionAddress).Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            //lbltest.Text = "";
            count = 0;
        }

        private void cmbDBName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            hlp.ComboDBEvent(1, dicDBName, cmbDBName, cmbTBName);
        }

        private void cmbTBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //hlp.LoadColumnNew(cmbDBName.Text, cmbTBName, clbField1);
            //hlp.LoadColumn(cmbDBName.Text, cmbTBName, cmbField2);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            //cmbFilter.SelectedIndex = 1;
            //cmbServer.SelectedIndex = 0;
            //txtfrom.Visible = false;txtTo.Visible = false;
            //lblFrom.Visible = false;lblTo.Visible = false;lblType.Visible = false;
            
            cmbServer2.SelectedIndex = 0;
            btnDis.Visible = false;
            DataTable dtyear = new DataTable();
            for (int i = 1300; i < 1400; i++)
            {
                cmbYear1.Items.Insert(0, i.ToString());
                cmbYear2.Items.Insert(0, i.ToString());
            }
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Query;            
        }

        public DataTable AllSreach(string db,string tb)
        {
            string Query;
            Query = "Select * from [" + db + "].dbo.[" + tb +"]";
            return clsdal.sqlDataAdapter(Query,db);
        }
        private void btnDis_Click(object sender, EventArgs e)
        {
            DataTable dtAll = new DataTable();
            dtAll = AllSreach(cmbDBName.Text, cmbTBName.Text);
            DGV2.DataSource = dtAll;
            lblAllPerson.Text = dtAll.AsEnumerable().GroupBy(r => r.Field<int>("StudentID")).Count().ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn2_Click(object sender, EventArgs e)
        {
            command = "";
            DataTable dt = new DataTable();
            clsVal.DBName = cmbDBName.Text;
            clsVal.TBName = cmbTBName.Text;
            //txtTest.Text = clsdal.OneTaTenDarand("1385");
            dt.Columns.Add("ردیف");
            dt.Columns.Add("سال");
            dt.Columns.Add("تعداد افراد کل");
            dt.Columns.Add("سریع");
            dt.Columns.Add("نرمال");
            dt.Columns.Add("کند");
            dt.Columns.Add("نا معتبر");
            //for (int i = Convert.ToInt32(cmbYear1.Text); i < Convert.ToInt32(cmbYear2.Text); i++)
            //{
            //    DataRow dr=dt.NewRow();
            //    dr["ردیف"] = "fdgdfg";
            //    dr["سال"] = "fdgdfg";
            //    dr["تعداد افراد کل"] = "fdgdfg";
            //    dr["سریع"] = "fdgdfg";
            //    dr["نرمال"] = "fdgdfg";
            //    dr["کند"] = "fdgdfg";
            //    dr["نا معتبر"] = "fdgdfg";

            //    dt.Rows.Add(dr);
            //}

            DGVtp2.DataSource = dt;
        }

        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbServer.SelectedIndex==0)
            {
                txtPass.Text = ""; txtPass.Visible = false; label9.Visible = false;
                txtUser.Text = ""; txtUser.Visible = false; label10.Visible = false;
            }
            else if(cmbServer.SelectedIndex==1)
            {
                txtPass.Text = "Ark.NSS-110";
                txtUser.Text = "Karimi";
                //txtPass.Visible = true; label9.Visible = true;
                //txtUser.Visible = true; label10.Visible = true;
            }
            if (cmbServer2.SelectedIndex==0 | cmbServer2.SelectedIndex==1)
            {
                txtPass2.Text = ""; txtPass2.Visible = false; label20.Visible = false;
                txtUser2.Text = ""; txtUser2.Visible = false; label19.Visible = false;
            }
        }

        private void btnCopyDB_Click(object sender, EventArgs e)
        {
            clsVal.DBName = cmbDBName.Text;
            clsVal.TBName = cmbTBName.Text;
            DataTable dtcpDB = new DataTable();
            //if (chkcopyDB.CheckState==CheckState.Checked)
            //{            
            //    dtcpDB = clsdal.sqlDataAdapter("select * from ["+cmbDBName.Text+".dbo."+cmbTBName.Text+"]",cmbDBName.Text);            
            //}
            DGV2.DataSource = dtcpDB;
        }
    }
}
