using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MC.Buiness;
using MC.DB;

namespace MasterClassified
{
    public partial class frmNavigate : DockContent
    {

        private frmAddCaiPiao frmAddCaiPiao;
        public log4net.ILog ProcessLogger;
        public log4net.ILog ExceptionLogger;

        public frmNavigate()
        {
            InitializeComponent();
         //   MessageBox.Show("维护中....", "状态", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            InitialSystemInfo();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {


            if (frmAddCaiPiao == null)
            {
                frmAddCaiPiao = new frmAddCaiPiao("");
                frmAddCaiPiao.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
            }
            if (frmAddCaiPiao == null)
            {
                frmAddCaiPiao = new frmAddCaiPiao("");
            }
            frmAddCaiPiao.ShowDialog();
        }
        void FrmOMS_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is frmAddCaiPiao)
            {
                InitialSystemInfo();
                frmAddCaiPiao = null;
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text == null)
            {
                MessageBox.Show("请选择彩票！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clsAllnew BusinessHelp = new clsAllnew();
            BusinessHelp.delete_CaiPiaoZhongLei(this.listBox1.Text);
            MessageBox.Show("删除{0}成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            InitialSystemInfo();
        }
        private void InitialSystemInfo()
        {
            #region 初始化配置
            ProcessLogger = log4net.LogManager.GetLogger("ProcessLogger");
            ExceptionLogger = log4net.LogManager.GetLogger("SystemExceptionLogger");
            ProcessLogger.Fatal("System Start " + DateTime.Now.ToString());
            #endregion
            this.listBox1.DisplayMember = "Name";
            clsAllnew BusinessHelp = new clsAllnew();
            List<CaipiaoZhongLeiDATA> Result = BusinessHelp.Read_CaiPiaoZhongLei();
            List<CaipiaoZhongLeiDATA> filtered = Result.FindAll(s => s.Name != null);
            CaipiaoZhongLeiDATA indexfiltered = Result.Find(s => s.MoRenXuanzhong == "YES");
            this.listBox1.DataSource = filtered;

            this.listBox1.SelectedItems.Clear();


            int index = 0;
            if (indexfiltered != null)
            {
                foreach (CaipiaoZhongLeiDATA iteminde in Result)
                {

                    if (iteminde.Name == indexfiltered.Name)
                    {
                        break;
                      
                    }
                    index++;
                }
            }

            if (filtered.Count!=0)
            this.listBox1.SelectedIndex = index;
            // listBox1.SetSelected(0, true); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text == null)
            {
                MessageBox.Show("请选择彩票！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (frmAddCaiPiao == null)
            {
                frmAddCaiPiao = new frmAddCaiPiao(this.listBox1.Text);
                frmAddCaiPiao.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
            }
            if (frmAddCaiPiao == null)
            {
                frmAddCaiPiao = new frmAddCaiPiao(this.listBox1.Text);
            }
            frmAddCaiPiao.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text == null)
            {
                MessageBox.Show("请选择彩票！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clsAllnew BusinessHelp = new clsAllnew();
            BusinessHelp.MoRenUpdate_CaiPiaoZhongLei(this.listBox1.Text);
            this.Close();


        }


    }
}
