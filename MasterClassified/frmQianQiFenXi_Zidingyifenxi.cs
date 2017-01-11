using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MC.Buiness;
using MC.DB;

namespace MasterClassified
{
    public partial class frmQianQiFenXi_Zidingyifenxi : Form
    {
        public log4net.ILog ProcessLogger;
        public log4net.ILog ExceptionLogger;

     public   List<int> newi = new List<int>();

        public frmQianQiFenXi_Zidingyifenxi()
        {
            InitializeComponent();
            InitialSystemInfo();

        }
        private void InitialSystemInfo()
        {
            #region 初始化配置
            ProcessLogger = log4net.LogManager.GetLogger("ProcessLogger");
            ExceptionLogger = log4net.LogManager.GetLogger("SystemExceptionLogger");
            ProcessLogger.Fatal("System Start " + DateTime.Now.ToString());
            #endregion

            clsAllnew BusinessHelp = new clsAllnew();

            List<CaipiaoZhongLeiDATA> CaipiaozhongleiResult = BusinessHelp.Read_CaiPiaoZhongLei_Moren("YES");

            if (CaipiaozhongleiResult.Count == 0)
            {
                MessageBox.Show("彩票默认运行类型没有选中,请到【彩票类型界面】选中彩票类型，点击确认", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }
            //this.label2.Text = CaipiaozhongleiResult[0].Name;
            ////this.label4.Text = CaipiaozhongleiResult[0].Name;
            //this.label6.Text = CaipiaozhongleiResult[0].JiBenHaoMaS + "-" + CaipiaozhongleiResult[0].JiBenHaoMaT;
            string len = CaipiaozhongleiResult[0].Xuan;
            for (int i = 0; i < Convert.ToInt32(len); i++)
            {
                int con = i + 1;

                clbStatus.Items.Add("第 " + con + " 位");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbStatus.Items.Count; i++)
            {
                clbStatus.SetItemChecked(i, false);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbStatus.Items.Count; i++)
            {
                clbStatus.SetItemChecked(i, true);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            newi = new List<int>();


            if (clbStatus.CheckedItems.Count > 0)
            {
                foreach (string status in this.clbStatus.CheckedItems)
                {
                    newi.Add(Convert.ToInt32(status.Replace("第 ", "").Replace(" 位", "")));
                  
                }
            }
            this.Close();

        }
    }
}
