using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MC.Common;
using MC.Buiness;
using MC.DB;

namespace MasterClassified
{
    public partial class frmImport_Data : Form
    {    // 后台执行控件
        private BackgroundWorker bgWorker;
        // 消息显示窗体
        private frmMessageShow frmMessageShow;
        // 后台操作是否正常完成
        private bool blnBackGroundWorkIsOK = false;
        //后加的后台属性显
        private bool backGroundRunResult;
        public log4net.ILog ProcessLogger { get; set; }
        public log4net.ILog ExceptionLogger { get; set; }
        List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();
        string MCpath;

        public frmImport_Data()
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
        }
        private void InitialBackGroundWorker()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged +=
                new ProgressChangedEventHandler(bgWorker_ProgressChanged);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                blnBackGroundWorkIsOK = false;
            }
            else if (e.Cancelled)
            {
                blnBackGroundWorkIsOK = true;
            }
            else
            {
                blnBackGroundWorkIsOK = true;
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (frmMessageShow != null && frmMessageShow.Visible == true)
            {
                //设置显示的消息
                frmMessageShow.setMessage(e.UserState.ToString());
                //设置显示的按钮文字
                if (e.ProgressPercentage == clsConstant.Thread_Progress_OK)
                {
                    frmMessageShow.setStatus(clsConstant.Dialog_Status_Enable);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MCpath = "";


            OpenFileDialog tbox = new OpenFileDialog();
            tbox.Multiselect = false;
            //  tbox.Filter = "Excel Files(*.xls,*.xlsx,*.xlsm,*.xlsb)|*.xls;*.xlsx;*.xlsm;*.xlsb";
            tbox.Filter = "Excel Files(*.xls,*.xlsx,*.xlsm,*.xlsb,*.txt)|*.xls;*.xlsx;*.xlsm;*.xlsb;*.txt";
            if (tbox.ShowDialog() == DialogResult.OK)
            {
                MCpath = tbox.FileName;
            }
            if (MCpath == null || MCpath == "")
                return;
            this.textBox1.Text = MCpath.Trim();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            {
                if (MessageBox.Show(" 将导入新数据导入系统，是否继续 ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                }
                else
                    return;
                try
                {
                    InitialBackGroundWorker();
                    bgWorker.DoWork += new DoWorkEventHandler(inputdatacaipiao);

                    bgWorker.RunWorkerAsync();
                    // 启动消息显示画面
                    frmMessageShow = new frmMessageShow(clsShowMessage.MSG_001,
                                                        clsShowMessage.MSG_007,
                                                        clsConstant.Dialog_Status_Disable);
                    frmMessageShow.ShowDialog();
                    // 数据读取成功后在画面显示
                    if (blnBackGroundWorkIsOK)
                    {

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
        private void inputdatacaipiao(object sender, DoWorkEventArgs e)
        {
            //导入程序集
            DateTime oldDate = DateTime.Now;
            Result = new List<inputCaipiaoDATA>();
            clsAllnew BusinessHelp = new clsAllnew();
            ProcessLogger.Fatal("1005--input kiajiang data" + DateTime.Now.ToString());
            Result = BusinessHelp.InputclaimReport(ref this.bgWorker, MCpath);
            ProcessLogger.Fatal("1006-- Input finish" + DateTime.Now.ToString());
            DateTime FinishTime = DateTime.Now;  //   
            TimeSpan s = DateTime.Now - oldDate;
            string timei = s.Minutes.ToString() + ":" + s.Seconds.ToString();
            string Showtime = clsShowMessage.MSG_029 + timei.ToString();
            bgWorker.ReportProgress(clsConstant.Thread_Progress_OK, clsShowMessage.MSG_009 + "\r\n" + Showtime);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
