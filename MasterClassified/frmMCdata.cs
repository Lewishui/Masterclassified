using MC.Buiness;
using MC.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using System.Text.RegularExpressions;
using MC.Common;

namespace MasterClassified
{
    public partial class frmMCdata : DockContent
    {
        public log4net.ILog ProcessLogger;
        public log4net.ILog ExceptionLogger;
        List<inputCaipiaoDATA> ClaimReport_Server;
        private Hashtable datagrid_changes = null;
        private frmTimeSelect frmTimeSelect;
        int RowRemark = 0;
        int cloumn = 0;
        string zhiqianqianqi;

        DateTimePicker dtp = new DateTimePicker();
        Rectangle _Rectangle; //用来判断时间控件的位置
        public frmMCdata()
        {
            InitializeComponent();
            InitialSystemInfo();

        }
        private void InitialSystemInfo()
        {
            int errol = 0;
            try
            {
                errol = 0;

                #region 初始化配置
                ProcessLogger = log4net.LogManager.GetLogger("ProcessLogger");
                ExceptionLogger = log4net.LogManager.GetLogger("SystemExceptionLogger");
                ProcessLogger.Fatal("System Start " + DateTime.Now.ToString());
                #endregion

                this.datagrid_changes = new Hashtable();

                //this.listBox1.DisplayMember = "Name";
                //clsAllnew BusinessHelp = new clsAllnew();
                //List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_FangAnName();
                //List<FangAnLieBiaoDATA> filtered = Result.FindAll(s => s.Name != null);
                //this.listBox1.DataSource = filtered;
                clsAllnew BusinessHelp = new clsAllnew();
                errol = 1;

                List<CaipiaoZhongLeiDATA> CaipiaozhongleiResult = BusinessHelp.Read_CaiPiaoZhongLei_Moren("YES");
                ProcessLogger.Fatal("System Read_CaiPiaoZhongLei_Moren 70104 " + DateTime.Now.ToString());
                if (CaipiaozhongleiResult.Count == 0)
                {
                    MessageBox.Show("彩票默认运行类型没有选中", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;

                }
                errol = 2;
                this.label2.Text = CaipiaozhongleiResult[0].Name;
                //this.label4.Text = CaipiaozhongleiResult[0].Name;
                this.label6.Text = CaipiaozhongleiResult[0].JiBenHaoMaS + "-" + CaipiaozhongleiResult[0].JiBenHaoMaT;
                this.label8.Text = CaipiaozhongleiResult[0].Xuan;


                ClaimReport_Server = new List<inputCaipiaoDATA>();

                ProcessLogger.Fatal("System ReadclaimreportfromServerBy_Xuan 70105" + DateTime.Now.ToString());

                DateTime oldDate = DateTime.Now;
                ClaimReport_Server = new List<inputCaipiaoDATA>();
                ClaimReport_Server = BusinessHelp.ReadclaimreportfromServerBy_Xuan(this.label2.Text);
                errol = 3;
                bool runischina = false;
                foreach (inputCaipiaoDATA item in ClaimReport_Server)
                {
                    if (item.QiHao != null && item.QiHao != "")
                    {
                        bool ischina = HasChineseTest(item.QiHao);
                        if (ischina == true || Regex.Matches(item.QiHao, "[a-zA-Z]").Count > 0)
                        {
                            runischina = true;

                            MessageBox.Show("EX:异常类型,请修改或删除，不然会影响正常的数据判断，期号 ：" + item.QiHao);
                            break;


                        }
                    }
                }
                if (runischina == false)
                    ClaimReport_Server.Sort(new Comp());

                ProcessLogger.Fatal("System ReadclaimreportfromServerBy_Xuan 70106" + DateTime.Now.ToString());
                //this.dataGridView1.DataSource = null;
                //this.dataGridView1.AutoGenerateColumns = false;
                //if (ClaimReport_Server.Count != 0)
                //{
                //    this.dataGridView1.DataSource = ClaimReport_Server;
                //}

                #region table
                errol = 4;
                var qtyTable = new DataTable();
                //foreach (var igrouping in ClaimReport_Server)
                //{
                //    // 生成 ioTable, use c{j}  instead of igrouping.Key, datagridview required
                //    //qtyTable.Columns.Add(igrouping._id, System.Type.GetType("System.String"));

                //    // qtyTable.Columns.Add(igrouping._id, System.Type.GetType("System.Int32"));
                //}

                string[] temptong = System.Text.RegularExpressions.Regex.Split(CaipiaozhongleiResult[0].Xuan, " ");

                int l = 0;
                qtyTable.Columns.Add("期号", System.Type.GetType("System.Int32"));
                qtyTable.Columns.Add("开奖日期", System.Type.GetType("System.String"));

                int jiindex = 0;

                for (int m = 0; m < Convert.ToInt32(temptong[0]); m++)
                {
                    jiindex++;

                    qtyTable.Columns.Add("基号" + jiindex.ToString(), System.Type.GetType("System.String"));

                }
                foreach (var k in ClaimReport_Server)
                {
                    qtyTable.Rows.Add(qtyTable.NewRow());
                }
                int jk = 0;

                foreach (var item in ClaimReport_Server)
                {
                    if (item.KaiJianHaoMa != null)
                    {
                        //  continue;
                        string[] temp1 = System.Text.RegularExpressions.Regex.Split(item.KaiJianHaoMa, " ");
                        int lie = 2;
                        for (int i = 0; i < temp1.Length; i++)
                        {
                            if (i >= temp1.Length || lie - Convert.ToInt32(temptong[0]) > 1)
                                continue;

                            qtyTable.Rows[jk][lie] = temp1[i];
                            lie++;

                        }
                    }
                    qtyTable.Rows[jk][0] = item.QiHao;
                    qtyTable.Rows[jk][1] = item.KaiJianRiqi;

                    jk++;
                }
                ProcessLogger.Fatal("System  70107" + DateTime.Now.ToString());
                //   sortablePendingOrderList = new SortableBindingList<inputCaipiaoDATA>(qtyTable);

                // this.bindingSource1.DataSource = null;
                this.bindingSource1.DataSource = qtyTable;
                bindingSource1.Sort = "期号  ASC";
                this.dataGridView1.DataSource = this.bindingSource1;
                if (dataGridView1.Rows.Count != 0)
                {
                    int ii = dataGridView1.Rows.Count - 1;
                    dataGridView1.CurrentCell = dataGridView1[0, ii]; // 强制将光标指向i行
                    dataGridView1.Rows[ii].Selected = true;   //光标显示至i行 
                    RowRemark = ii;
                }
                #endregion

            }
            catch (Exception ex)
            {
                ProcessLogger.Fatal("System Start 0802832 :" + errol + DateTime.Now.ToString());

                MessageBox.Show("0802832 EX:数据初始化失败 ：" + ex);

                throw;
            }
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string qi = DateTime.Now.ToString("yyyyMMddss");

            if (ClaimReport_Server == null || ClaimReport_Server.Count == 0)
            {
                qi = DateTime.Now.ToString("yyyyMMddss");
            }
            else
            {
                //ClaimReport_Server.Sort(new Comp());
                qi = ClaimReport_Server[0].QiHao;
            }
            //int index = this.dataGridView1.Rows.Add();

            //this.dataGridView1.Rows[index].Cells[0].Value = Convert.ToInt32(qi) + 1;
            //this.dataGridView1.Rows[index].Cells[1].Value = DateTime.Now.ToString("yyyy-MM-dd");

            List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();
            inputCaipiaoDATA item = new inputCaipiaoDATA();
            int a = Convert.ToInt32(qi) + 1;
            item.QiHao = a.ToString();
            item.Caipiaomingcheng = this.label2.Text.ToString();
            item.KaiJianRiqi = DateTime.Now.ToString("yyyy/MM/dd").ToString();
            Result.Add(item);

            clsAllnew BusinessHelp = new clsAllnew();

            BusinessHelp.SPInputclaimreport_Server(Result);
            InitialSystemInfo();
            //dataGridView1.FirstDisplayedCell = dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0]; 
            //dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;// ;
            dataGridView1.Rows[dataGridView1.RowCount - 1].Selected = true;
            //DataGridViewRow row = new DataGridViewRow();
            //DataGridViewComboBoxCell comboxcell = new DataGridViewComboBoxCell();
            //row.Cells.Add(comboxcell);
            //dataGridView1.Rows.Add(row);
            //DataGridViewColumn newcolumn = new DataGridViewColumn();
            //dataGridView1.Rows.Add(row);
        }

        #region 排序
        private class Comp : Comparer<inputCaipiaoDATA>
        {
            public override int Compare(inputCaipiaoDATA iten1, inputCaipiaoDATA item)
            {
                #region 判断是否为汉字
                if (iten1.QiHao != null && iten1.QiHao != "")
                {
                    char[] c = iten1.QiHao.ToCharArray();
                    bool ischina = false;

                    for (int i = 0; i < c.Length; i++)
                    {
                        if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                            ischina = true;
                    }

                    if (ischina == true || Regex.Matches(iten1.QiHao, "[a-zA-Z]").Count > 0)
                    {
                        return 0;
                    }
                }
                else
                    return 0;

                if (iten1.QiHao != null && iten1.QiHao != "")
                {
                    char[] c = item.QiHao.ToCharArray();
                    bool ischina = false;
                    for (int i = 0; i < c.Length; i++)
                    {
                        if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                            ischina = true;
                    }
                    if (ischina == true || Regex.Matches(item.QiHao, "[a-zA-Z]").Count > 0)
                    {
                        return 0;
                    }
                }
                else
                    return 0;
                #endregion
                if (iten1.QiHao.Length > 10 || item.QiHao.Length > 10)
                {
                    return 0;

                }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowRemark = e.RowIndex;
            cloumn = e.ColumnIndex;
            zhiqianqianqi = this.dataGridView1.Rows[RowRemark].Cells[0].EditedFormattedValue.ToString();

            return;
            //if (e.ColumnIndex == 1)
            //{
            //    //  dataGridView1.Rows[RowRemark].Cells[cloumn] = new DataGridViewComboBoxCell();
            //    var form = new frmTimeSelect();
            //    //var form1 = form.ShowDialog();
            //    if (form.ShowDialog() == DialogResult.OK)
            //    {
            //        dataGridView1.Rows[RowRemark].Cells[cloumn].Value = form.dateclose;
            //    }
            //    dataGridView1.Rows[RowRemark].Cells[cloumn].Value = form.dateclose;
            //}
        }
        private void BindGvApply()
        {
            dataGridView1.Controls.Add(dtp);
            dtp.Visible = false;  //先不让它显示
            dtp.Format = DateTimePickerFormat.Custom;  //设置日期格式为2010-08-05
            dtp.TextChanged += new EventHandler(dtp_TextChange);
        }

        private void dtp_TextChange(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell.Value = dtp.Text.ToString();

            //时间控件选择时间时，就把时间赋给所在的单元格
        }


        void FrmOMS_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is frmTimeSelect)
            {
                dataGridView1.Rows[RowRemark].Cells[cloumn].Value = frmTimeSelect.dateclose;

                frmTimeSelect = null;
            }


        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    frmTimeSelect = new frmTimeSelect();
            //    if (this.dataGridView1.CurrentCell.ColumnIndex.ToString() == "4" || this.dataGridView1.CurrentCell.ColumnIndex.ToString() == "6" || this.dataGridView1.CurrentCell.ColumnIndex.ToString() == "5")//在此指定和哪一列绑定
            //    {
            //        System.Drawing.Rectangle rect = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex, false);
            //        frmTimeSelect.Left = rect.Left;
            //        frmTimeSelect.Top = rect.Top;
            //        frmTimeSelect.Width = rect.Width;
            //        frmTimeSelect.Height = rect.Height;
            //        frmTimeSelect.Visible = true;
            //        //i = this.dataGridView1.CurrentRow.Index;
            //        //j = this.dataGridView1.CurrentCell.ColumnIndex;
            //        dataGridView1.CurrentCell.Value = frmTimeSelect.dateclose;
            //    }
            //    else
            //    {
            //        frmTimeSelect.Visible = false;
            //    }

            //}
            //catch
            //{
            //}
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //   string qi = ClaimReport_Server[ClaimReport_Server.Count - 1].QiHao;
            List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();
            inputCaipiaoDATA item = new inputCaipiaoDATA();
            item.QiHao = this.dataGridView1.Rows[RowRemark].Cells[0].EditedFormattedValue.ToString();
            item.KaiJianRiqi = this.dataGridView1.Rows[RowRemark].Cells[1].EditedFormattedValue.ToString();

            item.Xuan = this.label8.Text;
            item.Caipiaomingcheng = this.label2.Text.ToString();
            for (int i = 2; i < dataGridView1.ColumnCount; i++)
            {

                {

                    item.KaiJianHaoMa = item.KaiJianHaoMa + " " + dataGridView1.Rows[RowRemark].Cells[i].EditedFormattedValue.ToString().Trim();
                }
            }
            item.KaiJianHaoMa = item.KaiJianHaoMa.Trim();

            Result.Add(item);

            clsAllnew BusinessHelp = new clsAllnew();

            BusinessHelp.SPInputclaimreport_Server(Result);

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            var form = new frmChangeCaiPiaodata(this.dataGridView1.Rows[RowRemark].Cells[0].EditedFormattedValue.ToString(), this.label2.Text.ToString());
            //var form1 = form.ShowDialog();

            if (form.ShowDialog() == DialogResult.OK)
            {
                InitialSystemInfo();
            }
            InitialSystemInfo();

        }

        private void notifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RowRemark >= dataGridView1.Rows.Count)
            {
                RowRemark = RowRemark - 1;
            }
            string QiHao = this.dataGridView1.Rows[RowRemark].Cells[0].EditedFormattedValue.ToString();
            clsAllnew BusinessHelp = new clsAllnew();

            BusinessHelp.delete_CaiPiaoData(QiHao);
            InitialSystemInfo();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.RowIndex != 0 && e.ColumnIndex != 0)
            {

                bool handle;
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals(DBNull.Value))
                {
                    handle = true;
                }
                else
                    handle = false;
                e.Cancel = handle;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 1)
            {

                var form = new frmTimeSelect();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.Rows[RowRemark].Cells[cloumn].Value = form.dateclose;

                }
                dataGridView1.Rows[RowRemark].Cells[cloumn].Value = form.dateclose;
                return;
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要清空当前类型的数据 ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {

            }
            else
                return;
            clsAllnew BusinessHelp = new clsAllnew();

            BusinessHelp.delete_CaiPiaoData(this.label8.Text);
            InitialSystemInfo();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // string qi = ClaimReport_Server[ClaimReport_Server.Count - 1].QiHao;
            List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();
            inputCaipiaoDATA item = new inputCaipiaoDATA();
            item.QiHao = this.dataGridView1.Rows[RowRemark].Cells[0].EditedFormattedValue.ToString();
            item.KaiJianRiqi = this.dataGridView1.Rows[RowRemark].Cells[1].EditedFormattedValue.ToString();
            //item.JiShu1 = this.dataGridView1.Rows[RowRemark].Cells[2].EditedFormattedValue.ToString();
            //item.JiShu2 = this.dataGridView1.Rows[RowRemark].Cells[3].EditedFormattedValue.ToString();
            //item.JiShu3 = this.dataGridView1.Rows[RowRemark].Cells[4].EditedFormattedValue.ToString();

            for (int i = 2; i < dataGridView1.ColumnCount; i++)
            {

                {

                    item.KaiJianHaoMa = item.KaiJianHaoMa + " " + dataGridView1.Rows[RowRemark].Cells[i].EditedFormattedValue.ToString().Trim();
                }
            }
            item.KaiJianHaoMa = item.KaiJianHaoMa.Trim();
            item.Xuan = this.label8.Text;
            item.Caipiaomingcheng = this.label2.Text.ToString();
            Result.Add(item);

            clsAllnew BusinessHelp = new clsAllnew();

            BusinessHelp.SPInputclaimreport_Server(Result);

            //IEnumerable<int> orderIds = GetChangedOrderIds();
            //List<inputCaipiaoDATA> orders = GetDataGridViewBoundOrders();
            //if (orderIds.Count() > 0)
            //{
            //    foreach (var id in orderIds.Distinct())
            //    {
            //        var pendingorder = orders.Find(o => o.QiHao == id.ToString());
            //            t_orderdata order = ctx.t_orderdata.Find(pendingorder.id受注データ);
            //    }
            //}
        }
        private List<inputCaipiaoDATA> GetDataGridViewBoundOrders()
        {
            List<inputCaipiaoDATA> orders = new List<inputCaipiaoDATA>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                orders.Add(dataGridView1.Rows[i].DataBoundItem as inputCaipiaoDATA);
            }

            return orders;
        }
        private IEnumerable<int> GetChangedOrderIds()
        {

            List<int> rows = new List<int>();
            foreach (DictionaryEntry entry in datagrid_changes)
            {
                var key = entry.Key as string;
                if (key.EndsWith("_changed"))
                {
                    int row = Int32.Parse(key.Split('_')[0]);
                    rows.Add(row);
                }
                //                    Console.WriteLine("Key -- {0}; Value --{1}.", entry.Key, entry.Value);
            }
            return rows.Distinct();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

            {

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




            }

        }



        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (RowRemark >= dataGridView1.RowCount)
                return;

            List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();
            inputCaipiaoDATA item = new inputCaipiaoDATA();
            item.QiHao = this.dataGridView1.Rows[RowRemark].Cells[0].EditedFormattedValue.ToString();
            item.KaiJianRiqi = this.dataGridView1.Rows[RowRemark].Cells[1].EditedFormattedValue.ToString();
            if (dataGridView1.ColumnCount - 2 != Convert.ToInt32(this.label8.Text))
            {
                //  MessageBox.Show("号码填写不准确或位数不匹配当前种类要求，请填写完整！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            for (int i = 2; i < dataGridView1.ColumnCount; i++)
            {
                if (dataGridView1.Rows[RowRemark].Cells[i].EditedFormattedValue.ToString().Trim() == "")
                    dataGridView1.Rows[RowRemark].Cells[i].Value = "0";
                item.KaiJianHaoMa = item.KaiJianHaoMa + " " + dataGridView1.Rows[RowRemark].Cells[i].EditedFormattedValue.ToString().Trim();
            }
            item.KaiJianHaoMa = item.KaiJianHaoMa.Trim();
            item.Xuan = this.label8.Text;
            item.Caipiaomingcheng = this.label2.Text.ToString();
            if (item.QiHao == null || item.QiHao == "")
            {
                MessageBox.Show("期号不能为空，请填写完整！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if (item.KaiJianRiqi == null || item.KaiJianRiqi == "")
            {
                MessageBox.Show("开奖日期不能为空，请填写完整！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if (item.Xuan == null || item.Xuan == "")
            {
                MessageBox.Show("彩票类型【选】配置错误，请填写完整！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }
            if (item.Caipiaomingcheng == null || item.Caipiaomingcheng == "")
            {
                MessageBox.Show("彩票类型[名称]配置错误，请填写完整！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }
            //判断期号是否包含字母，汉字
            bool ischina = HasChineseTest(item.QiHao);
            if (ischina == true || Regex.Matches(item.QiHao, "[a-zA-Z]").Count > 0)
            {
                MessageBox.Show("开奖号码填写信息类型错误，请重新填写！", "类型错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //判断KaiJianHaoMa是否包含字母，汉字
            ischina = HasChineseTest(item.KaiJianHaoMa);
            if (ischina == true || Regex.Matches(item.KaiJianHaoMa, "[a-zA-Z]").Count > 0)
            {
                MessageBox.Show("开奖号码填写信息类型错误，请重新填写！", "类型错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //   string a = clsCommHelp.objToDateTime(item.KaiJianRiqi);
            ischina = HasChineseTest(item.KaiJianRiqi);
            if (ischina == true || Regex.Matches(item.KaiJianRiqi, "[a-zA-Z]").Count > 0)
            {
                MessageBox.Show("开奖日期---填写信息类型错误，请重新填写！", "类型错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Result.Add(item);
            clsAllnew BusinessHelp = new clsAllnew();
            if (zhiqianqianqi != item.QiHao)
                BusinessHelp.SPInputclaimreport_Server1(Result, zhiqianqianqi);
            else
                BusinessHelp.SPInputclaimreport_Server(Result);

            ClaimReport_Server = new List<inputCaipiaoDATA>();
            ClaimReport_Server = BusinessHelp.ReadclaimreportfromServerBy_Xuan(this.label2.Text);

            ClaimReport_Server.Sort(new Comp());

        }
        //判断是否为汉字
        public bool HasChineseTest(string text)
        {
            //string text = "是不是汉字，ABC,keleyi.com";
            char[] c = text.ToCharArray();
            bool ischina = false;

            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                {
                    ischina = true;

                }
                //else
                //{
                //    ischina = false;
                //}
            }
            return ischina;

        }
    }
}
