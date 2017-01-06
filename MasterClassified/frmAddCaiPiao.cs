using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MC.DB;
using MC.Buiness;

namespace MasterClassified
{
    public partial class frmAddCaiPiao : Form
    {
        List<CaipiaoZhongLeiDATA> ClaimReport_Server;
        string checkname = "";
        public log4net.ILog ProcessLogger;
        public log4net.ILog ExceptionLogger;
        public frmAddCaiPiao(string name)
        {
            InitializeComponent();
            checkname = name;
            if (name != "")
            {
                InitialSystemInfo(name);
            }
        }
        private void InitialSystemInfo(string name)
        {
            #region 初始化配置
            ProcessLogger = log4net.LogManager.GetLogger("ProcessLogger");
            ExceptionLogger = log4net.LogManager.GetLogger("SystemExceptionLogger");
            ProcessLogger.Fatal("System Start " + DateTime.Now.ToString());

            #endregion
            clsAllnew BusinessHelp = new clsAllnew();
            List<CaipiaoZhongLeiDATA> Result = BusinessHelp.Find_CaipiaoZhongLei_(name);
            foreach (CaipiaoZhongLeiDATA item in Result)
            {
                textBox1.Text = item.Name;

                this.comboBox1.Text = item.JiBenHaoMaS;

                this.comboBox2.Text = item.JiBenHaoMaT;

                if (item.Check_TeBieHao == "YES")
                    checkBox1.Checked = true;

                else
                    checkBox1.Checked = false;

                this.comboBox3.Text = item.Xuan;

                this.comboBox5.Text = item.TeBieHaoS;

                this.comboBox4.Text = item.TeBieHaoT;

            }



        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkname == "")
            {
                ClaimReport_Server = new List<CaipiaoZhongLeiDATA>();

                CaipiaoZhongLeiDATA item = new CaipiaoZhongLeiDATA();

                item.Name = textBox1.Text.Trim();
                item.JiBenHaoMaS = this.comboBox1.Text.Trim();
                item.JiBenHaoMaT = this.comboBox2.Text.Trim();
                if (checkBox1.Checked == true)
                    item.Check_TeBieHao = "YES";
                else
                    item.Check_TeBieHao = "NO";
                item.Xuan = this.comboBox3.Text.Trim();
                item.TeBieHaoS = this.comboBox5.Text.Trim();
                item.TeBieHaoT = this.comboBox4.Text.Trim();
                ClaimReport_Server.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Save_CaiPiaoZhongLei(ClaimReport_Server);

                MessageBox.Show("创建成功！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }
            else
            {

                ClaimReport_Server = new List<CaipiaoZhongLeiDATA>();

                CaipiaoZhongLeiDATA item = new CaipiaoZhongLeiDATA();

                item.Name = textBox1.Text.Trim();
                item.JiBenHaoMaS = this.comboBox1.Text.Trim();
                item.JiBenHaoMaT = this.comboBox2.Text.Trim();
                if (checkBox1.Checked == true)
                    item.Check_TeBieHao = "YES";
                else
                    item.Check_TeBieHao = "NO";
                item.Xuan = this.comboBox3.Text.Trim();
                item.TeBieHaoS = this.comboBox5.Text.Trim();
                item.TeBieHaoT = this.comboBox4.Text.Trim();
                ClaimReport_Server.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Update_CaiPiaoZhongLei(checkname,ClaimReport_Server);

                MessageBox.Show("修改成功！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();


            }
        }
    }
}
