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
using System.Reflection;
using System.IO;
using MC.Common;
using System.Threading;

namespace MasterClassified
{
    public partial class frmTracing_Analysis : DockContent
    {
        private Thread GetDataforOutlookThread;
        public log4net.ILog ProcessLogger { get; set; }
        public log4net.ILog ExceptionLogger { get; set; }
        private frmSetConfig frmSetConfig;
        private frmUDF frmUDF;
        private List<int> UDF;
        List<inputCaipiaoDATA> ClaimReport_Server;
        // 后台执行控件
        private BackgroundWorker bgWorker;
        // 消息显示窗体
        private frmMessageShow frmMessageShow;
        // 后台操作是否正常完成
        private bool blnBackGroundWorkIsOK = false;
        //后加的后台属性显
        private bool backGroundRunResult;
        private frmImport_MCleixing_Data frmImport_MCleixing_Data;
        private frmQianQiFenXi_Zidingyifenxi frmQianQiFenXi_Zidingyifenxi;
        List<int> newi;
        int qianqiqishu = 0;

        private SortableBindingList<inputCaipiaoDATA> sortablePendingOrderList;
        public frmTracing_Analysis()
        {
            InitializeComponent();

            InitialSystemInfo();

            //for (int j = 11; j < dataGridView1.ColumnCount; j++)
            //{
            //    // dataGridView1.Columns[j].Width = 30;

            //    //将每一列都调整为自动适应模式
            //    dgViewFiles.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
            //    //记录整个DataGridView的宽度
            //    width += dgViewFiles.Columns[i].Width;

            //}

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


            ClaimReport_Server = new List<inputCaipiaoDATA>();
            ClaimReport_Server = BusinessHelp.ReadclaimreportfromServerBy_Xuan(CaipiaozhongleiResult[0].Name);
            ClaimReport_Server.Sort(new Comp());

            this.toolStripComboBox1.ComboBox.DisplayMember = "QiHao";
            this.toolStripComboBox1.ComboBox.ValueMember = "QiHao";
            this.toolStripComboBox1.ComboBox.DataSource = ClaimReport_Server;

            this.toolStripComboBox2.ComboBox.DisplayMember = "QiHao";
            this.toolStripComboBox2.ComboBox.ValueMember = "QiHao";
            this.toolStripComboBox2.ComboBox.DataSource = ClaimReport_Server;

            if (ClaimReport_Server.Count != 0)
            {
                this.toolStripComboBox1.SelectedIndex = 0;
                this.toolStripComboBox2.SelectedIndex = ClaimReport_Server.Count - 1;
                this.toolStripComboBox3.SelectedIndex = 2;
                this.toolStripComboBox4.SelectedIndex = 2;
            }

            this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;


            toolStripLabel7.Text = "系统正在读取数据和内部计算，需要一段时间，请稍后....";
            //GetDataforOutlookThread = new Thread(NewMethodtab1);
            //GetDataforOutlookThread.Start();
            NewMethodtab1();

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



        private void AutoSizeColumn(DataGridView dgViewFiles)
        {
            int width = 0;
            //使列自使用宽度
            //对于DataGridView的每一个列都调整
            for (int i = 0; i < dgViewFiles.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                dgViewFiles.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += dgViewFiles.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，
            //如果是小于原来设定的宽度，将模式改为填充。
            if (width > dgViewFiles.Size.Width)
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            //冻结某列 从左开始 0，1，2
            dgViewFiles.Columns[1].Frozen = true;
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsAllnew BusinessHelp = new clsAllnew();
                //ClaimReport_Server = new List<inputCaipiaoDATA>();

                int s = this.tabControl1.SelectedIndex;
                if (s == 0)
                {
                    toolStripLabel7.Text = "系统正在读取数据和内部计算，需要一段时间，请稍后....";
                    //GetDataforOutlookThread = new Thread(NewMethodtab1);
                    //GetDataforOutlookThread.Start();
                    NewMethodtab1();

                }
                else if (s == 2)
                {
                    toolStripComboBox4.Items.Clear();
                    for (int i = 1; i <= 2000; i++)
                    {
                        toolStripComboBox4.Items.Add(i);

                    }
                    toolStripComboBox4.SelectedIndex = 4;

                    qianqiqishu = Convert.ToInt32(toolStripComboBox4.Text);

                    toolStripLabel7.Text = "系统正在读取数据和内部计算，需要一段时间，请稍后....";

                    GetDataforOutlookThread = new Thread(tab2);
                    GetDataforOutlookThread.Start();
                    // tab2(BusinessHelp);



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return;

                throw;
            }

        }

        private void tab2()
        {
            clsAllnew BusinessHelp = new clsAllnew();
            List<string> qianmingcheng = new List<string>();
            //ClaimReport_Server = BusinessHelp.ReadclaimreportfromServer();
            ClaimReport_Server.Sort(new CompsSmall());
            int indexing = 0;
            foreach (inputCaipiaoDATA item in ClaimReport_Server)
            {
                item.qianAll = "";
                item.qianMingcheng = "";
                item.TongAll = "";
                indexing = 0;
                string text = "";

                foreach (inputCaipiaoDATA temp in ClaimReport_Server)
                {
                    if (Convert.ToInt32(item.QiHao) > Convert.ToInt32(temp.QiHao) && indexing < Convert.ToInt32(qianqiqishu))
                    {
                        indexing++;
                        int xiangtongindex = 0;
                        string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.KaiJianHaoMa, " ");
                        string[] temp1 = System.Text.RegularExpressions.Regex.Split(temp.KaiJianHaoMa, " ");
                        #region 匹配相同次数
                        for (int i = 0; i < temp3.Length; i++)
                        {
                            for (int j1 = 0; j1 < temp1.Length; j1++)
                            {
                                if (temp3[i] == temp1[j1])
                                    xiangtongindex++;
                            }
                        }

                        #endregion
                        //item.qianAll = item.qianAll + "\r\n前" + indexing + " " + xiangtongindex.ToString();
                        text = text + " " + xiangtongindex.ToString();
                        item.qianAll = item.qianAll + " " + xiangtongindex.ToString();
                        item.qianMingcheng = item.qianMingcheng + "\r\n前" + indexing;
                        //  qianmingcheng = item.qianMingcheng + "\r\n前" + indexing; ;
                        int isrun = 0;
                        for (int m = 0; m < qianmingcheng.Count; m++)
                        {
                            if (qianmingcheng[m] == "前" + indexing)
                                isrun++;
                        }
                        if (isrun == 0)
                            qianmingcheng.Add("前" + indexing);
                    }
                    else if (indexing > Convert.ToInt32(qianqiqishu))
                    {
                        break;

                    }
                }
                string[] temptong = System.Text.RegularExpressions.Regex.Split(text, " ");

                for (int j = 0; j < 15; j++)
                {
                    int xiangtongindex = 0;

                    for (int i = 1; i < temptong.Length; i++)
                    {
                        if (j.ToString() == temptong[i])
                        {
                            xiangtongindex++;
                        }

                    }
                    item.TongAll = item.TongAll + "\r\n同" + j + " " + xiangtongindex.ToString();

                }

            }
            var qtyTable = new DataTable();
            //foreach (var igrouping in ClaimReport_Server)
            //{
            //    // 生成 ioTable, use c{j}  instead of igrouping.Key, datagridview required
            //    //qtyTable.Columns.Add(igrouping._id, System.Type.GetType("System.String"));

            //    // qtyTable.Columns.Add(igrouping._id, System.Type.GetType("System.Int32"));
            //}
            int l = 0;
            //添加 抬头名称，如果 选中了前几期的combox 
            indexing = 1;
            qianmingcheng = new List<string>();
            for (int i = 1; i <= qianqiqishu; i++)
            {
                qianmingcheng.Add("前" + indexing);
                indexing++;
            }

            qtyTable.Columns.Add("期号", System.Type.GetType("System.String"));
            qtyTable.Columns.Add("开奖号码", System.Type.GetType("System.String"));

            for (int m = 0; m < qianmingcheng.Count; m++)
            {
                qtyTable.Columns.Add(qianmingcheng[m], System.Type.GetType("System.String"));

            }
            //  qtyTable.Rows.Add(qtyTable.NewRow());
            foreach (var k in ClaimReport_Server)
            {
                qtyTable.Rows.Add(qtyTable.NewRow());
            }

            int jk = 0;
            int cindex = 0;

            foreach (var item in ClaimReport_Server)
            {
                cindex = 0;

                if (item.qianAll != null)
                {
                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(item.qianAll, " ");
                    for (int i = 0; i < temp1.Length; i++)
                    {
                        cindex++;

                        if (i == 0 || i >= temp1.Length)
                            continue;

                        qtyTable.Rows[jk][cindex] = temp1[i];
                    }
                }
                qtyTable.Rows[jk][0] = item.QiHao;
                qtyTable.Rows[jk][1] = item.KaiJianHaoMa;
                // qtyTable.Rows[1][4] = item.QiHao;
                jk++;
            }

            //   sortablePendingOrderList = new SortableBindingList<inputCaipiaoDATA>(qtyTable);
            //qtyTable.Sort(new Comp());
            //  this.bindingSource1.DataSource = null;
            this.bindingSource1.DataSource = qtyTable;
            bindingSource1.Sort = "期号  ASC";

            // this.dataGridView1.DataSource = this.bindingSource1;

            dataGridView2.DataSource = qtyTable;

            string width = "";

            for (int j = 2; j < dataGridView2.ColumnCount; j++)
            {

                dataGridView2.Columns[j].Width = 30;
            }
            toolStripLabel7.Text = "结束";
        }
        private void ZidingYi_tab2()
        {
            {
                clsAllnew BusinessHelp = new clsAllnew();
                List<string> qianmingcheng = new List<string>();

                //ClaimReport_Server = BusinessHelp.ReadclaimreportfromServer();


                ClaimReport_Server.Sort(new CompsSmall());
                int indexing = 0;
                foreach (inputCaipiaoDATA item in ClaimReport_Server)
                {
                    item.qianAll = "";
                    item.qianMingcheng = "";
                    item.TongAll = "";
                    indexing = 0;
                    string text = "";

                    foreach (inputCaipiaoDATA temp in ClaimReport_Server)
                    {
                        if (Convert.ToInt32(item.QiHao) > Convert.ToInt32(temp.QiHao) && indexing < Convert.ToInt32(qianqiqishu))
                        {
                            indexing++;
                            int xiangtongindex = 0;
                            string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.KaiJianHaoMa, " ");
                            string[] temp1 = System.Text.RegularExpressions.Regex.Split(temp.KaiJianHaoMa, " ");


                            #region 匹配相同次数
                            for (int i = 0; i < temp3.Length; i++)
                            {
                                //判断是否在自定义范围内的数据
                                bool next = false;
                                for (int oi = 0; oi < newi.Count; oi++)
                                {
                                    if (newi[oi] == i + 1)
                                        next = true;
                                }
                                if (next == false)
                                    continue;

                                for (int j1 = 0; j1 < temp1.Length; j1++)
                                {
                                    //判断是否在自定义范围内的数据
                                    bool nexti = false;
                                    for (int oi = 0; oi < newi.Count; oi++)
                                    {
                                        if (newi[oi] == j1 + 1)
                                            nexti = true;
                                    }
                                    if (nexti == false)
                                        continue;

                                    if (temp3[i] == temp1[j1])
                                        xiangtongindex++;
                                }
                            }

                            #endregion
                            //item.qianAll = item.qianAll + "\r\n前" + indexing + " " + xiangtongindex.ToString();
                            text = text + " " + xiangtongindex.ToString();
                            item.qianAll = item.qianAll + " " + xiangtongindex.ToString();
                            item.qianMingcheng = item.qianMingcheng + "\r\n前" + indexing;
                            //  qianmingcheng = item.qianMingcheng + "\r\n前" + indexing; ;
                            int isrun = 0;
                            for (int m = 0; m < qianmingcheng.Count; m++)
                            {
                                if (qianmingcheng[m] == "前" + indexing)
                                    isrun++;
                            }
                            if (isrun == 0)
                                qianmingcheng.Add("前" + indexing);

                        }
                        else if (indexing > Convert.ToInt32(qianqiqishu))
                        {
                            break;

                        }


                    }
                    string[] temptong = System.Text.RegularExpressions.Regex.Split(text, " ");

                    for (int j = 0; j < 15; j++)
                    {
                        int xiangtongindex = 0;

                        for (int i = 1; i < temptong.Length; i++)
                        {
                            if (j.ToString() == temptong[i])
                            {
                                xiangtongindex++;
                            }

                        }
                        item.TongAll = item.TongAll + "\r\n同" + j + " " + xiangtongindex.ToString();

                    }

                }
                var qtyTable = new DataTable();
                //foreach (var igrouping in ClaimReport_Server)
                //{
                //    // 生成 ioTable, use c{j}  instead of igrouping.Key, datagridview required
                //    //qtyTable.Columns.Add(igrouping._id, System.Type.GetType("System.String"));

                //    // qtyTable.Columns.Add(igrouping._id, System.Type.GetType("System.Int32"));
                //}
                int l = 0;
                //添加 抬头名称，如果 选中了前几期的combox 
                indexing = 1;
                qianmingcheng = new List<string>();
                for (int i = 1; i <= qianqiqishu; i++)
                {
                    qianmingcheng.Add("前" + indexing);
                    indexing++;
                }

                qtyTable.Columns.Add("期号", System.Type.GetType("System.String"));
                qtyTable.Columns.Add("开奖号码", System.Type.GetType("System.String"));

                for (int m = 0; m < qianmingcheng.Count; m++)
                {
                    qtyTable.Columns.Add(qianmingcheng[m], System.Type.GetType("System.String"));

                }
                //  qtyTable.Rows.Add(qtyTable.NewRow());
                foreach (var k in ClaimReport_Server)
                {
                    qtyTable.Rows.Add(qtyTable.NewRow());
                }

                int jk = 0;
                int cindex = 0;

                foreach (var item in ClaimReport_Server)
                {
                    cindex = 0;

                    if (item.qianAll != null)
                    {
                        string[] temp1 = System.Text.RegularExpressions.Regex.Split(item.qianAll, " ");
                        for (int i = 0; i < temp1.Length; i++)
                        {
                            cindex++;

                            if (i == 0 || i >= temp1.Length)
                                continue;

                            qtyTable.Rows[jk][cindex] = temp1[i];
                        }
                    }
                    qtyTable.Rows[jk][0] = item.QiHao;
                    qtyTable.Rows[jk][1] = item.KaiJianHaoMa;
                    // qtyTable.Rows[1][4] = item.QiHao;
                    jk++;
                }

                //   sortablePendingOrderList = new SortableBindingList<inputCaipiaoDATA>(qtyTable);
                //qtyTable.Sort(new Comp());
                //  this.bindingSource1.DataSource = null;
                this.bindingSource1.DataSource = qtyTable;
                bindingSource1.Sort = "期号  ASC";

                this.dataGridView1.DataSource = this.bindingSource1;

                dataGridView2.DataSource = qtyTable;

                string width = "";

                for (int j = 2; j < dataGridView2.ColumnCount; j++)
                {

                    dataGridView2.Columns[j].Width = 30;
                }
                toolStripLabel7.Text = "结束";
            }
        }

        private void NewMethodtab1()
        {
            try
            {
                //ClaimReport_Server = BusinessHelp.ReadclaimreportfromServer();
                clsAllnew BusinessHelp = new clsAllnew();

                #region 添加 基数 和前几期对比

                List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_FangAn("YES");

                //showSuijiResultlist = new List<string>();

                //foreach (FangAnLieBiaoDATA item in Result)
                //{
                //    string[] temp1 = System.Text.RegularExpressions.Regex.Split(item.Data, "\r\n");

                //    for (int i = 1; i < temp1.Length; i++)
                //    {
                //        showSuijiResultlist.Add(temp1[i]);
                //    }

                //}
                ClaimReport_Server.Sort(new CompsSmall());
                foreach (inputCaipiaoDATA item in ClaimReport_Server)
                {

                    foreach (FangAnLieBiaoDATA temp in Result)
                    {
                        string[] temp1 = System.Text.RegularExpressions.Regex.Split(temp.Data, "\r\n");

                        string[] temp2 = System.Text.RegularExpressions.Regex.Split(item.KaiJianHaoMa, " ");
                        for (int ii = 0; ii < temp2.Length; ii++)
                        {
                            for (int i = 1; i < temp1.Length; i++)
                            {
                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp1[i], "段");
                                int ss = ii + 1;

                                if (temp1[i].Contains(temp2[ii]))
                                {
                                    item.JiShu = item.JiShu + "基" + ss.ToString() + " " + temp3[0];
                                    if (ss == 1)
                                        item.JiShu1 = temp3[0];
                                    else if (ss == 2)
                                        item.JiShu2 = temp3[0];
                                    else if (ss == 3)
                                        item.JiShu3 = temp3[0];
                                    else if (ss == 4)
                                        item.JiShu4 = temp3[0];
                                    else if (ss == 5)
                                        item.JiShu5 = temp3[0];
                                    else if (ss == 6)
                                        item.JiShu6 = temp3[0];
                                    else if (ss == 7)
                                        item.JiShu7 = temp3[0];
                                    else if (ss == 8)
                                        item.JiShu8 = temp3[0];
                                    else if (ss == 9)
                                        item.JiShu9 = temp3[0];

                                    break;

                                }
                            }

                        }
                    }
                }

                #endregion

                // ClaimReport_Server.Sort(new Comp());
                int indexing = 0;
                foreach (inputCaipiaoDATA item in ClaimReport_Server)
                {
                    indexing = 0;

                    foreach (inputCaipiaoDATA temp in ClaimReport_Server)
                    {
                        if (Convert.ToInt32(item.QiHao) > Convert.ToInt32(temp.QiHao))
                        {
                            indexing++;
                            int xiangtongindex = 0;
                            #region 匹配相同次数
                            if (item.JiShu1 != null && item.JiShu1 == temp.JiShu1)
                                xiangtongindex++;
                            if (item.JiShu2 != null && item.JiShu2 == temp.JiShu2)
                                xiangtongindex++;
                            if (item.JiShu3 != null && item.JiShu3 == temp.JiShu3)
                                xiangtongindex++;
                            if (item.JiShu4 != null && item.JiShu4 == temp.JiShu4)
                                xiangtongindex++;
                            if (item.JiShu5 != null && item.JiShu5 == temp.JiShu5)
                                xiangtongindex++;
                            if (item.JiShu6 != null && item.JiShu6 == temp.JiShu6)
                                xiangtongindex++;
                            if (item.JiShu7 != null && item.JiShu7 == temp.JiShu7)
                                xiangtongindex++;
                            if (item.JiShu8 != null && item.JiShu8 == temp.JiShu8)
                                xiangtongindex++;
                            if (item.JiShu9 != null && item.JiShu9 == temp.JiShu9)
                                xiangtongindex++;
                            #endregion
                            #region MyRegion
                            if (indexing == 1)
                                item.qian1 = xiangtongindex.ToString();
                            else if (indexing == 2) item.qian2 = xiangtongindex.ToString();
                            else if (indexing == 3) item.qian3 = xiangtongindex.ToString();
                            else if (indexing == 4) item.qian4 = xiangtongindex.ToString();
                            else if (indexing == 5) item.qian5 = xiangtongindex.ToString();
                            else if (indexing == 6) item.qian6 = xiangtongindex.ToString();
                            else if (indexing == 7) item.qian7 = xiangtongindex.ToString();
                            else if (indexing == 8) item.qian8 = xiangtongindex.ToString();
                            else if (indexing == 9) item.qian9 = xiangtongindex.ToString();
                            else if (indexing == 10) item.qian10 = xiangtongindex.ToString();
                            else if (indexing == 11) item.qian11 = xiangtongindex.ToString();
                            else if (indexing == 12) item.qian12 = xiangtongindex.ToString();
                            else if (indexing == 13) item.qian13 = xiangtongindex.ToString();
                            else if (indexing == 14) item.qian14 = xiangtongindex.ToString();
                            else if (indexing == 15) item.qian15 = xiangtongindex.ToString();
                            else if (indexing == 16) item.qian16 = xiangtongindex.ToString();
                            else if (indexing == 17) item.qian17 = xiangtongindex.ToString();
                            else if (indexing == 18) item.qian18 = xiangtongindex.ToString();
                            else if (indexing == 19) item.qian19 = xiangtongindex.ToString();
                            else if (indexing == 20) item.qian20 = xiangtongindex.ToString();
                            else if (indexing == 21) item.qian21 = xiangtongindex.ToString();
                            else if (indexing == 22) item.qian22 = xiangtongindex.ToString();
                            else if (indexing == 23) item.qian23 = xiangtongindex.ToString();
                            else if (indexing == 24) item.qian24 = xiangtongindex.ToString();
                            else if (indexing == 25) item.qian25 = xiangtongindex.ToString();
                            else if (indexing == 26) item.qian26 = xiangtongindex.ToString();
                            else if (indexing == 27) item.qian27 = xiangtongindex.ToString();
                            else if (indexing == 28) item.qian28 = xiangtongindex.ToString();
                            else if (indexing == 29) item.qian29 = xiangtongindex.ToString();
                            else if (indexing == 30) item.qian30 = xiangtongindex.ToString();
                            else if (indexing == 31) item.qian31 = xiangtongindex.ToString();
                            else if (indexing == 32) item.qian32 = xiangtongindex.ToString();
                            else if (indexing == 33) item.qian33 = xiangtongindex.ToString();
                            else if (indexing == 34) item.qian34 = xiangtongindex.ToString();
                            else if (indexing == 35) item.qian35 = xiangtongindex.ToString();
                            else if (indexing == 36) item.qian36 = xiangtongindex.ToString();
                            else if (indexing == 37) item.qian37 = xiangtongindex.ToString();
                            else if (indexing == 38) item.qian38 = xiangtongindex.ToString();
                            else if (indexing == 39) item.qian39 = xiangtongindex.ToString();
                            else if (indexing == 40) item.qian40 = xiangtongindex.ToString();
                            else if (indexing == 41) item.qian41 = xiangtongindex.ToString();
                            else if (indexing == 42) item.qian42 = xiangtongindex.ToString();
                            else if (indexing == 43) item.qian43 = xiangtongindex.ToString();
                            else if (indexing == 44) item.qian44 = xiangtongindex.ToString();
                            else if (indexing == 45) item.qian45 = xiangtongindex.ToString();
                            else if (indexing == 46) item.qian46 = xiangtongindex.ToString();
                            else if (indexing == 47) item.qian47 = xiangtongindex.ToString();
                            else if (indexing == 48) item.qian48 = xiangtongindex.ToString();
                            else if (indexing == 49) item.qian49 = xiangtongindex.ToString();
                            else if (indexing == 50) item.qian50 = xiangtongindex.ToString();
                            else if (indexing == 51) item.qian51 = xiangtongindex.ToString();
                            else if (indexing == 52) item.qian52 = xiangtongindex.ToString();
                            else if (indexing == 53) item.qian53 = xiangtongindex.ToString();
                            else if (indexing == 54) item.qian54 = xiangtongindex.ToString();
                            else if (indexing == 55) item.qian55 = xiangtongindex.ToString();
                            else if (indexing == 56) item.qian56 = xiangtongindex.ToString();
                            else if (indexing == 57) item.qian57 = xiangtongindex.ToString();
                            else if (indexing == 58) item.qian58 = xiangtongindex.ToString();
                            else if (indexing == 59) item.qian59 = xiangtongindex.ToString();
                            else if (indexing == 60) item.qian60 = xiangtongindex.ToString();
                            else if (indexing == 61) item.qian61 = xiangtongindex.ToString();
                            else if (indexing == 62) item.qian62 = xiangtongindex.ToString();
                            else if (indexing == 63) item.qian63 = xiangtongindex.ToString();
                            else if (indexing == 64) item.qian64 = xiangtongindex.ToString();
                            else if (indexing == 65) item.qian65 = xiangtongindex.ToString();
                            else if (indexing == 66) item.qian66 = xiangtongindex.ToString();
                            else if (indexing == 67) item.qian67 = xiangtongindex.ToString();
                            else if (indexing == 68) item.qian68 = xiangtongindex.ToString();
                            else if (indexing == 69) item.qian69 = xiangtongindex.ToString();
                            else if (indexing == 70) item.qian70 = xiangtongindex.ToString();
                            else if (indexing == 71) item.qian71 = xiangtongindex.ToString();
                            else if (indexing == 72) item.qian72 = xiangtongindex.ToString();
                            else if (indexing == 73) item.qian73 = xiangtongindex.ToString();
                            else if (indexing == 74) item.qian74 = xiangtongindex.ToString();
                            else if (indexing == 75) item.qian75 = xiangtongindex.ToString();
                            else if (indexing == 76) item.qian76 = xiangtongindex.ToString();
                            else if (indexing == 77) item.qian77 = xiangtongindex.ToString();
                            else if (indexing == 78) item.qian78 = xiangtongindex.ToString();
                            else if (indexing == 79) item.qian79 = xiangtongindex.ToString();
                            else if (indexing == 80) item.qian80 = xiangtongindex.ToString();
                            else if (indexing == 81) item.qian81 = xiangtongindex.ToString();
                            else if (indexing == 82) item.qian82 = xiangtongindex.ToString();
                            else if (indexing == 83) item.qian83 = xiangtongindex.ToString();
                            else if (indexing == 84) item.qian84 = xiangtongindex.ToString();
                            else if (indexing == 85) item.qian85 = xiangtongindex.ToString();
                            else if (indexing == 86) item.qian86 = xiangtongindex.ToString();
                            else if (indexing == 87) item.qian87 = xiangtongindex.ToString();
                            else if (indexing == 88) item.qian88 = xiangtongindex.ToString();
                            else if (indexing == 89) item.qian89 = xiangtongindex.ToString();
                            else if (indexing == 90) item.qian90 = xiangtongindex.ToString();
                            else if (indexing == 91) item.qian91 = xiangtongindex.ToString();
                            else if (indexing == 92) item.qian92 = xiangtongindex.ToString();
                            else if (indexing == 93) item.qian93 = xiangtongindex.ToString();
                            else if (indexing == 94) item.qian94 = xiangtongindex.ToString();
                            else if (indexing == 95) item.qian95 = xiangtongindex.ToString();
                            else if (indexing == 96) item.qian96 = xiangtongindex.ToString();
                            else if (indexing == 97) item.qian97 = xiangtongindex.ToString();
                            else if (indexing == 98) item.qian98 = xiangtongindex.ToString();
                            else if (indexing == 99) item.qian99 = xiangtongindex.ToString();
                            #endregion
                        }

                    }
                }
                #region 显示信息
                //this.dataGridView1.DataSource = null;
                //this.dataGridView1.AutoGenerateColumns = false;
                if (ClaimReport_Server.Count != 0)
                {
                    NewMethod();

                    //sortablePendingOrderList = new SortableBindingList<inputCaipiaoDATA>(ClaimReport_Server);
                    //this.bindingSource2.DataSource = sortablePendingOrderList;
                    //this.dataGridView1.DataSource = this.bindingSource2;
                    //this.dataGridView1.DataSource = ClaimReport_Server;
                }
                //  inputCaipiaoDATA item1 = new inputCaipiaoDATA();

                //item1.QiHao = "全部";
                //ClaimReport_Server.Insert(0, item1);
                // this.toolStripComboBox1.Items.Add("全部");

             
                toolStripLabel7.Text = "结束";
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

                throw;
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (frmSetConfig == null)
            {
                frmSetConfig = new frmSetConfig();
                frmSetConfig.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
            }
            if (frmSetConfig == null)
            {
                frmSetConfig = new frmSetConfig();
            }
            frmSetConfig.ShowDialog();


            int s = this.tabControl1.SelectedIndex;
            if (s == 0)
            {
                toolStripLabel7.Text = "系统正在读取数据和内部计算，需要一段时间，请稍后....";
                GetDataforOutlookThread = new Thread(NewMethodtab1);
                GetDataforOutlookThread.Start();

            }
        }
        void FrmOMS_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is frmSetConfig)
            {
                frmSetConfig = null;
            }
            if (sender is frmUDF)
            {

                UDF = new List<int>();
                UDF = frmUDF.JIDTA;

                frmUDF = null;
            }
            if (sender is frmQianQiFenXi_Zidingyifenxi)
            {
                newi = new List<int>();
                newi = frmQianQiFenXi_Zidingyifenxi.newi;

                frmQianQiFenXi_Zidingyifenxi = null;
            }

        }

        #region 排序
        private class Comp : Comparer<inputCaipiaoDATA>
        {
            public override int Compare(inputCaipiaoDATA item, inputCaipiaoDATA iten1)
            {

                if (item.QiHao == null && item.QiHao == "")
                {
                    //  item.DO_NO = "1";
                    //  return 0;
                    if (iten1.QiHao == null || !iten1.QiHao.Contains("DO"))
                        return int.Parse("0") - int.Parse("0");

                    return int.Parse("0") - int.Parse("0");
                }
                return int.Parse(item.QiHao.Replace("2000", "")) - int.Parse(iten1.QiHao.Replace("2000", ""));
                ;

            }
        }
        private class CompsSmall : Comparer<inputCaipiaoDATA>
        {
            public override int Compare(inputCaipiaoDATA iten1, inputCaipiaoDATA item)
            {

                if (item.QiHao == null && item.QiHao == "")
                {
                    //  item.DO_NO = "1";
                    //  return 0;
                    if (iten1.QiHao == null || !iten1.QiHao.Contains("DO"))
                        return int.Parse("0") - int.Parse("0");

                    return int.Parse("0") - int.Parse("0");
                }
                return int.Parse(item.QiHao.Replace("2000", "")) - int.Parse(iten1.QiHao.Replace("2000", ""));
                ;

            }
        }
        public class SortableBindingList<T> : BindingList<T>
        {
            private bool isSortedCore = true;
            private ListSortDirection sortDirectionCore = ListSortDirection.Ascending;
            private PropertyDescriptor sortPropertyCore = null;
            private string defaultSortItem;

            public SortableBindingList() : base() { }

            public SortableBindingList(IList<T> list) : base(list) { }

            protected override bool SupportsSortingCore
            {
                get { return true; }
            }

            protected override bool SupportsSearchingCore
            {
                get { return true; }
            }

            protected override bool IsSortedCore
            {
                get { return isSortedCore; }
            }

            protected override ListSortDirection SortDirectionCore
            {
                get { return sortDirectionCore; }
            }

            protected override PropertyDescriptor SortPropertyCore
            {
                get { return sortPropertyCore; }
            }

            protected override int FindCore(PropertyDescriptor prop, object key)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (Equals(prop.GetValue(this[i]), key)) return i;
                }
                return -1;
            }

            protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
            {
                isSortedCore = true;
                sortPropertyCore = prop;
                sortDirectionCore = direction;
                Sort();
            }

            protected override void RemoveSortCore()
            {
                if (isSortedCore)
                {
                    isSortedCore = false;
                    sortPropertyCore = null;
                    sortDirectionCore = ListSortDirection.Ascending;
                    Sort();
                }
            }

            public string DefaultSortItem
            {
                get { return defaultSortItem; }
                set
                {
                    if (defaultSortItem != value)
                    {
                        defaultSortItem = value;
                        Sort();
                    }
                }
            }

            private void Sort()
            {
                List<T> list = (this.Items as List<T>);
                list.Sort(CompareCore);
                ResetBindings();
            }

            private int CompareCore(T o1, T o2)
            {
                int ret = 0;
                if (SortPropertyCore != null)
                {
                    ret = CompareValue(SortPropertyCore.GetValue(o1), SortPropertyCore.GetValue(o2), SortPropertyCore.PropertyType);
                }
                if (ret == 0 && DefaultSortItem != null)
                {
                    PropertyInfo property = typeof(T).GetProperty(DefaultSortItem, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.IgnoreCase, null, null, new Type[0], null);
                    if (property != null)
                    {
                        ret = CompareValue(property.GetValue(o1, null), property.GetValue(o2, null), property.PropertyType);
                    }
                }
                if (SortDirectionCore == ListSortDirection.Descending) ret = -ret;
                return ret;
            }

            private static int CompareValue(object o1, object o2, Type type)
            {
                if (o1 == null) return o2 == null ? 0 : -1;
                else if (o2 == null) return 1;
                else if (type.IsPrimitive || type.IsEnum) return Convert.ToDouble(o1).CompareTo(Convert.ToDouble(o2));
                else if (type == typeof(DateTime)) return Convert.ToDateTime(o1).CompareTo(o2);
                else return String.Compare(o1.ToString().Trim(), o2.ToString().Trim());
            }
        }

        #endregion

        private void 自定义分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int s2 = this.tabControl1.SelectedIndex;

            if (s2 == 0)
            {
                if (frmUDF == null)
                {
                    frmUDF = new frmUDF();
                    frmUDF.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
                }
                if (frmUDF == null)
                {
                    frmUDF = new frmUDF();
                }
                frmUDF.ShowDialog();

                if (UDF.Count != 0)
                {

                 
                    int s = this.tabControl1.SelectedIndex;
                    if (s == 0)
                    {
                        clsAllnew BusinessHelp = new clsAllnew();
                        List<CaipiaoZhongLeiDATA> CaipiaozhongleiResult = BusinessHelp.Read_CaiPiaoZhongLei_Moren("YES");
                        ClaimReport_Server = new List<inputCaipiaoDATA>();
                        ClaimReport_Server = BusinessHelp.ReadclaimreportfromServerBy_Xuan(CaipiaozhongleiResult[0].Name);
                        ClaimReport_Server.Sort(new Comp());

                       // InitialSystemInfo();
                        #region 原始 用  Dav 筛选
                        //   List<inputCaipiaoDATA> ClaimReport_Server = BusinessHelp.ReadclaimreportfromServer();
                        #region 添加 基数 和前几期对比

                        List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_FangAn("YES");
                        foreach (inputCaipiaoDATA item in ClaimReport_Server)
                        {
                            foreach (FangAnLieBiaoDATA temp in Result)
                            {
                                string[] temp1 = System.Text.RegularExpressions.Regex.Split(temp.Data, "\r\n");

                                string[] temp2 = System.Text.RegularExpressions.Regex.Split(item.KaiJianHaoMa, " ");
                                for (int ii = 0; ii < temp2.Length; ii++)
                                {
                                    for (int i = 1; i < temp1.Length; i++)
                                    {
                                        string[] temp3 = System.Text.RegularExpressions.Regex.Split(temp1[i], "段");
                                        int ss = ii + 1;
                                        bool isrun = false;

                                        for (int j = 0; j < UDF.Count; j++)
                                        {
                                            if (UDF[j] == ss)
                                                isrun = true;

                                        }
                                        if (isrun == false)
                                            continue;

                                        if (temp1[i].Contains(temp2[ii]))
                                        {
                                            item.JiShu = item.JiShu + "基" + ss.ToString() + " " + temp3[0];
                                            if (ss == 1)
                                                item.JiShu1 = temp3[0];
                                            else if (ss == 2)
                                                item.JiShu2 = temp3[0];
                                            else if (ss == 3)
                                                item.JiShu3 = temp3[0];
                                            else if (ss == 4)
                                                item.JiShu4 = temp3[0];
                                            else if (ss == 5)
                                                item.JiShu5 = temp3[0];
                                            else if (ss == 6)
                                                item.JiShu6 = temp3[0];
                                            else if (ss == 7)
                                                item.JiShu7 = temp3[0];
                                            else if (ss == 8)
                                                item.JiShu8 = temp3[0];
                                            else if (ss == 9)
                                                item.JiShu9 = temp3[0];
                                            break;
                                        }
                                    }

                                }
                            }
                        }

                        #endregion

                        //  ClaimReport_Server = new List<inputCaipiaoDATA>();

                        //  ClaimReport_Server.Sort(new Comp());
                        int indexing = 0;
                        foreach (inputCaipiaoDATA item in ClaimReport_Server)
                        {
                            indexing = 0;

                            foreach (inputCaipiaoDATA temp in ClaimReport_Server)
                            {
                                if (Convert.ToInt32(item.QiHao) > Convert.ToInt32(temp.QiHao))
                                {
                                    indexing++;
                                    int xiangtongindex = 0;

                                    #region 匹配相同次数
                                    for (int j = 0; j < UDF.Count; j++)
                                    {
                                        if (item.JiShu1 != null && item.JiShu1 == temp.JiShu1 && UDF[j] == 1)
                                            xiangtongindex++;
                                        if (item.JiShu2 != null && item.JiShu2 == temp.JiShu2 && UDF[j] == 2)
                                            xiangtongindex++;
                                        if (item.JiShu3 != null && item.JiShu3 == temp.JiShu3 && UDF[j] == 3)
                                            xiangtongindex++;
                                        if (item.JiShu4 != null && item.JiShu4 == temp.JiShu4 && UDF[j] == 4)
                                            xiangtongindex++;
                                        if (item.JiShu5 != null && item.JiShu5 == temp.JiShu5 && UDF[j] == 5)
                                            xiangtongindex++;
                                        if (item.JiShu6 != null && item.JiShu6 == temp.JiShu6 && UDF[j] == 6)
                                            xiangtongindex++;
                                        if (item.JiShu7 != null && item.JiShu7 == temp.JiShu7 && UDF[j] == 7)
                                            xiangtongindex++;
                                        if (item.JiShu8 != null && item.JiShu8 == temp.JiShu8 && UDF[j] == 8)
                                            xiangtongindex++;
                                        if (item.JiShu9 != null && item.JiShu9 == temp.JiShu9 && UDF[j] == 9)
                                            xiangtongindex++;
                                    }
                                    #endregion

                                    #region MyRegion
                                    if (indexing == 1)
                                        item.qian1 = xiangtongindex.ToString();

                                    else if (indexing == 2) item.qian2 = xiangtongindex.ToString();
                                    else if (indexing == 3) item.qian3 = xiangtongindex.ToString();
                                    else if (indexing == 4) item.qian4 = xiangtongindex.ToString();
                                    else if (indexing == 5) item.qian5 = xiangtongindex.ToString();
                                    else if (indexing == 6) item.qian6 = xiangtongindex.ToString();
                                    else if (indexing == 7) item.qian7 = xiangtongindex.ToString();
                                    else if (indexing == 8) item.qian8 = xiangtongindex.ToString();
                                    else if (indexing == 9) item.qian9 = xiangtongindex.ToString();
                                    else if (indexing == 10) item.qian10 = xiangtongindex.ToString();
                                    else if (indexing == 11) item.qian11 = xiangtongindex.ToString();
                                    else if (indexing == 12) item.qian12 = xiangtongindex.ToString();
                                    else if (indexing == 13) item.qian13 = xiangtongindex.ToString();
                                    else if (indexing == 14) item.qian14 = xiangtongindex.ToString();
                                    else if (indexing == 15) item.qian15 = xiangtongindex.ToString();
                                    else if (indexing == 16) item.qian16 = xiangtongindex.ToString();
                                    else if (indexing == 17) item.qian17 = xiangtongindex.ToString();
                                    else if (indexing == 18) item.qian18 = xiangtongindex.ToString();
                                    else if (indexing == 19) item.qian19 = xiangtongindex.ToString();
                                    else if (indexing == 20) item.qian20 = xiangtongindex.ToString();
                                    else if (indexing == 21) item.qian21 = xiangtongindex.ToString();
                                    else if (indexing == 22) item.qian22 = xiangtongindex.ToString();
                                    else if (indexing == 23) item.qian23 = xiangtongindex.ToString();
                                    else if (indexing == 24) item.qian24 = xiangtongindex.ToString();
                                    else if (indexing == 25) item.qian25 = xiangtongindex.ToString();
                                    else if (indexing == 26) item.qian26 = xiangtongindex.ToString();
                                    else if (indexing == 27) item.qian27 = xiangtongindex.ToString();
                                    else if (indexing == 28) item.qian28 = xiangtongindex.ToString();
                                    else if (indexing == 29) item.qian29 = xiangtongindex.ToString();
                                    else if (indexing == 30) item.qian30 = xiangtongindex.ToString();
                                    else if (indexing == 31) item.qian31 = xiangtongindex.ToString();
                                    else if (indexing == 32) item.qian32 = xiangtongindex.ToString();
                                    else if (indexing == 33) item.qian33 = xiangtongindex.ToString();
                                    else if (indexing == 34) item.qian34 = xiangtongindex.ToString();
                                    else if (indexing == 35) item.qian35 = xiangtongindex.ToString();
                                    else if (indexing == 36) item.qian36 = xiangtongindex.ToString();
                                    else if (indexing == 37) item.qian37 = xiangtongindex.ToString();
                                    else if (indexing == 38) item.qian38 = xiangtongindex.ToString();
                                    else if (indexing == 39) item.qian39 = xiangtongindex.ToString();
                                    else if (indexing == 40) item.qian40 = xiangtongindex.ToString();
                                    else if (indexing == 41) item.qian41 = xiangtongindex.ToString();
                                    else if (indexing == 42) item.qian42 = xiangtongindex.ToString();
                                    else if (indexing == 43) item.qian43 = xiangtongindex.ToString();
                                    else if (indexing == 44) item.qian44 = xiangtongindex.ToString();
                                    else if (indexing == 45) item.qian45 = xiangtongindex.ToString();
                                    else if (indexing == 46) item.qian46 = xiangtongindex.ToString();
                                    else if (indexing == 47) item.qian47 = xiangtongindex.ToString();
                                    else if (indexing == 48) item.qian48 = xiangtongindex.ToString();
                                    else if (indexing == 49) item.qian49 = xiangtongindex.ToString();
                                    else if (indexing == 50) item.qian50 = xiangtongindex.ToString();
                                    else if (indexing == 51) item.qian51 = xiangtongindex.ToString();
                                    else if (indexing == 52) item.qian52 = xiangtongindex.ToString();
                                    else if (indexing == 53) item.qian53 = xiangtongindex.ToString();
                                    else if (indexing == 54) item.qian54 = xiangtongindex.ToString();
                                    else if (indexing == 55) item.qian55 = xiangtongindex.ToString();
                                    else if (indexing == 56) item.qian56 = xiangtongindex.ToString();
                                    else if (indexing == 57) item.qian57 = xiangtongindex.ToString();
                                    else if (indexing == 58) item.qian58 = xiangtongindex.ToString();
                                    else if (indexing == 59) item.qian59 = xiangtongindex.ToString();
                                    else if (indexing == 60) item.qian60 = xiangtongindex.ToString();
                                    else if (indexing == 61) item.qian61 = xiangtongindex.ToString();
                                    else if (indexing == 62) item.qian62 = xiangtongindex.ToString();
                                    else if (indexing == 63) item.qian63 = xiangtongindex.ToString();
                                    else if (indexing == 64) item.qian64 = xiangtongindex.ToString();
                                    else if (indexing == 65) item.qian65 = xiangtongindex.ToString();
                                    else if (indexing == 66) item.qian66 = xiangtongindex.ToString();
                                    else if (indexing == 67) item.qian67 = xiangtongindex.ToString();
                                    else if (indexing == 68) item.qian68 = xiangtongindex.ToString();
                                    else if (indexing == 69) item.qian69 = xiangtongindex.ToString();
                                    else if (indexing == 70) item.qian70 = xiangtongindex.ToString();
                                    else if (indexing == 71) item.qian71 = xiangtongindex.ToString();
                                    else if (indexing == 72) item.qian72 = xiangtongindex.ToString();
                                    else if (indexing == 73) item.qian73 = xiangtongindex.ToString();
                                    else if (indexing == 74) item.qian74 = xiangtongindex.ToString();
                                    else if (indexing == 75) item.qian75 = xiangtongindex.ToString();
                                    else if (indexing == 76) item.qian76 = xiangtongindex.ToString();
                                    else if (indexing == 77) item.qian77 = xiangtongindex.ToString();
                                    else if (indexing == 78) item.qian78 = xiangtongindex.ToString();
                                    else if (indexing == 79) item.qian79 = xiangtongindex.ToString();
                                    else if (indexing == 80) item.qian80 = xiangtongindex.ToString();
                                    else if (indexing == 81) item.qian81 = xiangtongindex.ToString();
                                    else if (indexing == 82) item.qian82 = xiangtongindex.ToString();
                                    else if (indexing == 83) item.qian83 = xiangtongindex.ToString();
                                    else if (indexing == 84) item.qian84 = xiangtongindex.ToString();
                                    else if (indexing == 85) item.qian85 = xiangtongindex.ToString();
                                    else if (indexing == 86) item.qian86 = xiangtongindex.ToString();
                                    else if (indexing == 87) item.qian87 = xiangtongindex.ToString();
                                    else if (indexing == 88) item.qian88 = xiangtongindex.ToString();
                                    else if (indexing == 89) item.qian89 = xiangtongindex.ToString();
                                    else if (indexing == 90) item.qian90 = xiangtongindex.ToString();
                                    else if (indexing == 91) item.qian91 = xiangtongindex.ToString();
                                    else if (indexing == 92) item.qian92 = xiangtongindex.ToString();
                                    else if (indexing == 93) item.qian93 = xiangtongindex.ToString();
                                    else if (indexing == 94) item.qian94 = xiangtongindex.ToString();
                                    else if (indexing == 95) item.qian95 = xiangtongindex.ToString();
                                    else if (indexing == 96) item.qian96 = xiangtongindex.ToString();
                                    else if (indexing == 97) item.qian97 = xiangtongindex.ToString();
                                    else if (indexing == 98) item.qian98 = xiangtongindex.ToString();
                                    else if (indexing == 99) item.qian99 = xiangtongindex.ToString();

                                    #endregion

                                }
                            }
                        } 
                        #endregion


                        NewMethod();

                        //this.dataGridView1.DataSource = null;
                        //this.dataGridView1.AutoGenerateColumns = false;
                        //if (ClaimReport_Server.Count != 0)
                        //{
                        //    this.dataGridView1.DataSource = ClaimReport_Server;
                        //}
                        //this.toolStripComboBox1.ComboBox.DisplayMember = "QiHao";
                        //this.toolStripComboBox1.ComboBox.ValueMember = "QiHao";
                        //this.toolStripComboBox1.ComboBox.DataSource = ClaimReport_Server;

                        //this.toolStripComboBox2.ComboBox.DisplayMember = "QiHao";
                        //this.toolStripComboBox2.ComboBox.ValueMember = "QiHao";
                        //this.toolStripComboBox2.ComboBox.DataSource = ClaimReport_Server;
                        //this.toolStripComboBox1.SelectedIndex = 0;
                        //this.toolStripComboBox2.SelectedIndex = ClaimReport_Server.Count - 1;
                        //this.toolStripComboBox3.SelectedIndex = 2;
                        //this.toolStripComboBox4.SelectedIndex = 2;


                    }
                }
            }
            else if (s2 == 2)
            {
                if (frmQianQiFenXi_Zidingyifenxi == null)
                {
                    frmQianQiFenXi_Zidingyifenxi = new frmQianQiFenXi_Zidingyifenxi();
                    frmQianQiFenXi_Zidingyifenxi.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
                }
                if (frmQianQiFenXi_Zidingyifenxi == null)
                {
                    frmQianQiFenXi_Zidingyifenxi = new frmQianQiFenXi_Zidingyifenxi();
                }
                frmQianQiFenXi_Zidingyifenxi.ShowDialog();

                ZidingYi_tab2();




            }
        }

        private void toolStripComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            NewMethod();

        }

        private void NewMethod()
        {
            try
            {
                int s = this.tabControl1.SelectedIndex;
                if (s == 0)
                {
                    //for (int j = 11; j < dataGridView1.ColumnCount; j++)
                    //{
                    //    dataGridView1.Columns[j].Visible = true;
                    //}
                    //int i = 100 - Convert.ToInt32(toolStripComboBox4.Text);

                    ////for (int j = Convert.ToInt32(toolStripComboBox4.Text) + 11; j < i + 14; j++)
                    ////{
                    ////    dataGridView1.Columns[j].Visible = false;

                    ////}
                    //int startHidecloumn = Convert.ToInt32(toolStripComboBox4.Text) + 11;
                    //int totalcloumn = i + startHidecloumn - 1;
                    //for (int j = startHidecloumn; j <= totalcloumn; j++)
                    //{
                    //    dataGridView1.Columns[j].Visible = false;
                    //}

                    //自构造table
                    if (toolStripComboBox4.Text == null || toolStripComboBox4.Text == "")
                        return;

                    var qtyTable = new DataTable();
                    int comvalue = Convert.ToInt32(toolStripComboBox4.Text);

                    qtyTable.Columns.Add("期号", System.Type.GetType("System.String"));
                    qtyTable.Columns.Add("开奖号码", System.Type.GetType("System.String"));

                    for (int m = 0; m < 9; m++)
                    {
                        qtyTable.Columns.Add("基" + m, System.Type.GetType("System.String"));

                    }


                    for (int m = 0; m < Convert.ToInt32(toolStripComboBox4.Text); m++)
                    {
                        int ss = m + 1;

                        qtyTable.Columns.Add("前第+" + ss + "期相同位置", System.Type.GetType("System.String"));

                    }
                    //  qtyTable.Rows.Add(qtyTable.NewRow());
                    foreach (var k in ClaimReport_Server)
                    {
                        qtyTable.Rows.Add(qtyTable.NewRow());
                    }

                    int jk = 0;
                    int cindex = 12;

                    foreach (var item in ClaimReport_Server)
                    {
                        cindex = 10;


                        {
                            string allqian = item.qian1 + " " + item.qian2 + " " + item.qian3 + " " + item.qian4 + " " + item.qian5 + " " + item.qian6 + " " + item.qian7 + " " + item.qian8 + " " + item.qian9 + " " + item.qian10 + " " + item.qian11 + " " + item.qian12 + " " + item.qian13 + " " + item.qian14 + " " + item.qian15 + " " + item.qian16 + " " + item.qian17 + " " + item.qian18 + " " + item.qian19 + " " + item.qian20 + " " + item.qian21 + " " + item.qian22 + " " + item.qian23 + " " + item.qian24 + " " + item.qian25 + " " + item.qian26 + " " + item.qian27 + " " + item.qian28 + " " + item.qian29 + " " + item.qian30 + " " + item.qian31 + " " + item.qian32 + " " + item.qian33 + " " + item.qian34 + " " + item.qian35 + " " + item.qian36 + " " + item.qian37 + " " + item.qian38 + " " + item.qian39 + " " + item.qian40 + " " + item.qian41 + " " + item.qian42 + " " + item.qian43 + " " + item.qian44 + " " + item.qian45 + " " + item.qian46 + " " + item.qian47 + " " + item.qian48 + " " + item.qian49 + " " + item.qian50 + " " + item.qian51 + " " + item.qian52 + " " + item.qian53 + " " + item.qian54 + " " + item.qian55 + " " + item.qian56 + " " + item.qian57 + " " + item.qian58 + " " + item.qian59 + " " + item.qian60 + " " + item.qian61 + " " + item.qian62 + " " + item.qian63 + " " + item.qian64 + " " + item.qian65 + " " + item.qian66 + " " + item.qian67 + " " + item.qian68 + " " + item.qian69 + " " + item.qian70 + " " + item.qian71 + " " + item.qian72 + " " + item.qian73 + " " + item.qian74 + " " + item.qian75 + " " + item.qian76 + " " + item.qian77 + " " + item.qian78 + " " + item.qian79 + " " + item.qian80 + " " + item.qian81 + " " + item.qian82 + " " + item.qian83 + " " + item.qian84 + " " + item.qian85 + " " + item.qian86 + " " + item.qian87 + " " + item.qian88 + " " + item.qian89 + " " + item.qian90 + " " + item.qian91 + " " + item.qian92 + " " + item.qian93 + " " + item.qian94 + " " + item.qian95 + " " + item.qian96 + " " + item.qian97 + " " + item.qian98 + " " + item.qian99 + " " + item.qian100 + " ";

                            ;


                            string[] temp1 = System.Text.RegularExpressions.Regex.Split(allqian, " ");
                            for (int i = 0; i < temp1.Length; i++)
                            {
                                cindex++;
                                if (i >= comvalue)
                                    break;
                                qtyTable.Rows[jk][cindex] = temp1[i];
                            }
                        }
                        qtyTable.Rows[jk][0] = item.QiHao;
                        qtyTable.Rows[jk][1] = item.KaiJianHaoMa;
                        qtyTable.Rows[jk][2] = item.JiShu1;
                        qtyTable.Rows[jk][3] = item.JiShu2;
                        qtyTable.Rows[jk][4] = item.JiShu3;
                        qtyTable.Rows[jk][5] = item.JiShu4;
                        qtyTable.Rows[jk][6] = item.JiShu5;
                        qtyTable.Rows[jk][7] = item.JiShu6;
                        qtyTable.Rows[jk][8] = item.JiShu7;
                        qtyTable.Rows[jk][9] = item.JiShu8;
                        qtyTable.Rows[jk][10] = item.JiShu9;
                        // qtyTable.Rows[1][4] = item.QiHao;
                        jk++;
                    }

                    this.dataGridView1.DataSource = null;


                    //  this.dataGridView1.AutoGenerateColumns = false;
                    this.bindingSource2.DataSource = qtyTable;
                    bindingSource2.Sort = "期号  ASC";
                    this.dataGridView1.DataSource = this.bindingSource2;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据构造失败，请检查数据源", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                throw;
            }
        }

        private void 下载当前界面数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int s = this.tabControl1.SelectedIndex;
            if (s == 0)
            {

                {
                    if (this.dataGridView1.Rows.Count == 0)
                    {
                        MessageBox.Show("当前界面没有数据，请确认 !", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.DefaultExt = ".csv";
                    saveFileDialog.Filter = "csv|*.csv";
                    string strFileName = "Data " + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    saveFileDialog.FileName = strFileName;
                    if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        strFileName = saveFileDialog.FileName.ToString();
                    }
                    else
                    {
                        return;
                    }
                    FileStream fa = new FileStream(strFileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fa, Encoding.Unicode);
                    string delimiter = "\t";
                    string strHeader = "";
                    for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                    {
                        strHeader += this.dataGridView1.Columns[i].HeaderText + delimiter;
                    }
                    sw.WriteLine(strHeader);

                    //output rows data
                    for (int j = 0; j < this.dataGridView1.Rows.Count; j++)
                    {
                        string strRowValue = "";

                        for (int k = 0; k < this.dataGridView1.Columns.Count; k++)
                        {
                            if (this.dataGridView1.Rows[j].Cells[k].Value != null)
                            {
                                strRowValue += this.dataGridView1.Rows[j].Cells[k].Value.ToString().Replace("\r\n", " ").Replace("\n", "") + delimiter;
                                if (this.dataGridView1.Rows[j].Cells[k].Value.ToString() == "LIP201507-35")
                                {

                                }

                            }
                            else
                            {
                                strRowValue += this.dataGridView1.Rows[j].Cells[k].Value + delimiter;
                            }
                        }
                        sw.WriteLine(strRowValue);
                    }
                    sw.Close();
                    fa.Close();
                    MessageBox.Show("下载完成！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            else if (s == 0)
            {
                {
                    if (this.dataGridView2.Rows.Count == 0)
                    {
                        MessageBox.Show("当前界面没有数据，请确认  !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.DefaultExt = ".csv";
                    saveFileDialog.Filter = "csv|*.csv";
                    string strFileName = "Data" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    saveFileDialog.FileName = strFileName;
                    if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        strFileName = saveFileDialog.FileName.ToString();
                    }
                    else
                    {
                        return;
                    }
                    FileStream fa = new FileStream(strFileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fa, Encoding.Unicode);
                    string delimiter = "\t";
                    string strHeader = "";
                    for (int i = 0; i < this.dataGridView2.Columns.Count; i++)
                    {
                        strHeader += this.dataGridView2.Columns[i].HeaderText + delimiter;
                    }
                    sw.WriteLine(strHeader);

                    //output rows data
                    for (int j = 0; j < this.dataGridView2.Rows.Count; j++)
                    {
                        string strRowValue = "";

                        for (int k = 0; k < this.dataGridView2.Columns.Count; k++)
                        {
                            if (this.dataGridView2.Rows[j].Cells[k].Value != null)
                                strRowValue += this.dataGridView2.Rows[j].Cells[k].Value.ToString().Replace("\r\n", " ") + delimiter;
                            else
                                strRowValue += this.dataGridView2.Rows[j].Cells[k].Value + delimiter;
                        }
                        sw.WriteLine(strRowValue);
                    }

                    sw.Close();
                    fa.Close();
                    MessageBox.Show("下载完成！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            clsAllnew BusinessHelp = new clsAllnew();

            ClaimReport_Server = BusinessHelp.Fast_FindData(textBox1.Text.Trim().ToString());

            this.dataGridView1.DataSource = null;
            this.dataGridView1.AutoGenerateColumns = false;
            if (ClaimReport_Server.Count != 0)
            {
                this.dataGridView1.DataSource = ClaimReport_Server;
            }

            try
            {

                //ClaimReport_Server = new List<inputCaipiaoDATA>();
                int s = this.tabControl1.SelectedIndex;
                //if (s == 0)
                //{
                //    NewMethodtab1(BusinessHelp);

                //}
                //else if (s == 2)
                //{
                //    tab2(BusinessHelp);
                //}
                if (s == 0)
                {
                    toolStripLabel7.Text = "系统正在读取数据和内部计算，需要一段时间，请稍后....";
                    GetDataforOutlookThread = new Thread(NewMethodtab1);
                    GetDataforOutlookThread.Start();
                }
                else if (s == 2)
                {
                    toolStripLabel7.Text = "系统正在读取数据和内部计算，需要一段时间，请稍后....";
                    GetDataforOutlookThread = new Thread(tab2);
                    GetDataforOutlookThread.Start();
                    // tab2(BusinessHelp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return;

                throw;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            {
                try
                {

                    InitialBackGroundWorker();
                    bgWorker.DoWork += new DoWorkEventHandler(Refreshdata);

                    bgWorker.RunWorkerAsync();

                    // 启动消息显示画面
                    frmMessageShow = new frmMessageShow(clsShowMessage.MSG_001,
                                                        clsShowMessage.MSG_007,
                                                        clsConstant.Dialog_Status_Disable);
                    frmMessageShow.ShowDialog();

                    // 数据读取成功后在画面显示
                    if (blnBackGroundWorkIsOK)
                    {
                        this.dataGridView1.DataSource = null;
                        this.dataGridView1.AutoGenerateColumns = false;
                        if (ClaimReport_Server.Count != 0)
                        {
                            this.bindingSource1.DataSource = null;
                            this.bindingSource1.DataSource = sortablePendingOrderList;

                            this.dataGridView1.DataSource = this.bindingSource1;
                            //this.dataGridView1.DataSource = ClaimReport_Server;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return;
                    throw ex;
                }
            }

        }
        private void Refreshdata(object sender, DoWorkEventArgs e)
        {
            ClaimReport_Server = new List<inputCaipiaoDATA>();

            clsAllnew BusinessHelp = new clsAllnew();

            DateTime oldDate = DateTime.Now;


            ClaimReport_Server = new List<inputCaipiaoDATA>();
            ClaimReport_Server = BusinessHelp.ReadclaimreportfromServer();
            ClaimReport_Server.Sort(new Comp());

            sortablePendingOrderList = new SortableBindingList<inputCaipiaoDATA>(ClaimReport_Server);


            DateTime FinishTime = DateTime.Now;
            TimeSpan s = DateTime.Now - oldDate;
            string timei = s.Minutes.ToString() + ":" + s.Seconds.ToString();
            string Showtime = clsShowMessage.MSG_029 + timei.ToString();
            bgWorker.ReportProgress(clsConstant.Thread_Progress_OK, clsShowMessage.MSG_009 + "\r\n" + Showtime);
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            int s = this.tabControl1.SelectedIndex;

            if (s == 0)
            {

            }
        }

        private void toolStripComboBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int s = this.tabControl1.SelectedIndex;
            string shipper = this.toolStripComboBox1.Text;
            string county = toolStripComboBox2.Text;



            if (s == 0)
            {

                ApplyBindSourceFilter(shipper, county);
                //string s = this.toolStripComboBox2.Text;
                //string s1 = this.toolStripComboBox1.Text;
                //if (s == "全部" || s1 == "全部")
                //{
                //    this.dataGridView1.DataSource = this.bindingSource1;
                //}
                //else
                //{
                //    //bindingSource1.Filter = "QiHao >=" + s1 + "QiHao <=" + s;
                //    bindingSource1.Filter = "期号='20161216'";
                //    //sortablePendingOrderList.Where(s3 => Convert.ToInt32(s3.QiHao) >= Convert.ToInt32(s1) && Convert.ToInt32(s3.QiHao) <= Convert.ToInt32(s));
                //    dataGridView1.DataSource = bindingSource1;

                //}
            }

        }
        private void ApplyBindSourceFilter(string shipper, string county = "", string store = "")
        {

            //if (bindingSource1.Count > 0)
            {
                string filter = "";
                if (shipper.Length > 0)
                {
                    filter += " (期号>='" + shipper + "')";
                }

                if (county.Length > 0 && county != "")
                {
                    if (filter.Length > 0)
                    {
                        filter += " AND ";
                    }
                    filter += "(期号<=" + "'" + county + "'" + ")";
                }
                if (ClaimReport_Server.Count > 0)
                {
                    bindingSource2.Filter = filter;
                    this.dataGridView1.DataSource = bindingSource2;

                }

            }
        }

    }
}
