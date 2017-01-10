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
        DateTimePicker dtp = new DateTimePicker();
        Rectangle _Rectangle; //用来判断时间控件的位置
        public frmMCdata()
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

            this.datagrid_changes = new Hashtable();

            //this.listBox1.DisplayMember = "Name";
            //clsAllnew BusinessHelp = new clsAllnew();
            //List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_FangAnName();
            //List<FangAnLieBiaoDATA> filtered = Result.FindAll(s => s.Name != null);
            //this.listBox1.DataSource = filtered;
            clsAllnew BusinessHelp = new clsAllnew();

            List<CaipiaoZhongLeiDATA> CaipiaozhongleiResult = BusinessHelp.Read_CaiPiaoZhongLei_Moren("YES");

            if (CaipiaozhongleiResult.Count == 0)
            {
                MessageBox.Show("彩票默认运行类型没有选中", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }
            this.label2.Text = CaipiaozhongleiResult[0].Name;
            //this.label4.Text = CaipiaozhongleiResult[0].Name;
            this.label6.Text = CaipiaozhongleiResult[0].JiBenHaoMaS + "-" + CaipiaozhongleiResult[0].JiBenHaoMaT;
            this.label8.Text = CaipiaozhongleiResult[0].Xuan;



            ClaimReport_Server = new List<inputCaipiaoDATA>();


            DateTime oldDate = DateTime.Now;
            ClaimReport_Server = new List<inputCaipiaoDATA>();
            ClaimReport_Server = BusinessHelp.ReadclaimreportfromServerBy_Xuan(this.label8.Text);
            ClaimReport_Server.Sort(new Comp());
            //this.dataGridView1.DataSource = null;
            //this.dataGridView1.AutoGenerateColumns = false;
            //if (ClaimReport_Server.Count != 0)
            //{
            //    this.dataGridView1.DataSource = ClaimReport_Server;
            //}

            #region table

            var qtyTable = new DataTable();
            //foreach (var igrouping in ClaimReport_Server)
            //{
            //    // 生成 ioTable, use c{j}  instead of igrouping.Key, datagridview required
            //    //qtyTable.Columns.Add(igrouping._id, System.Type.GetType("System.String"));

            //    // qtyTable.Columns.Add(igrouping._id, System.Type.GetType("System.Int32"));
            //}

            string[] temptong = System.Text.RegularExpressions.Regex.Split(CaipiaozhongleiResult[0].Xuan, " ");

            int l = 0;
            qtyTable.Columns.Add("期号", System.Type.GetType("System.String"));
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
                if (item.KaiJianHaoMa == null)
                    continue;
                string[] temp1 = System.Text.RegularExpressions.Regex.Split(item.KaiJianHaoMa, " ");
                int lie = 2;
                for (int i = 0; i < temp1.Length; i++)
                {
                    if (i >= temp1.Length)
                        continue;

                    qtyTable.Rows[jk][lie] = temp1[i];
                    lie++;

                }
                qtyTable.Rows[jk][0] = item.QiHao;
                qtyTable.Rows[jk][1] = item.KaiJianRiqi;

                jk++;
            }

            //   sortablePendingOrderList = new SortableBindingList<inputCaipiaoDATA>(qtyTable);

            this.bindingSource1.DataSource = null;
            this.bindingSource1.DataSource = qtyTable;
            this.dataGridView1.DataSource = this.bindingSource1;

            //  dataGridView1.DataSource = qtyTable;


            #endregion

        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string qi = ClaimReport_Server[ClaimReport_Server.Count - 1].QiHao;

            int index = this.dataGridView1.Rows.Add();

            this.dataGridView1.Rows[index].Cells[0].Value = Convert.ToInt32(qi) + 1;
            this.dataGridView1.Rows[index].Cells[1].Value = DateTime.Now.ToString("yyyy-MM-dd");

            List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();
            inputCaipiaoDATA item = new inputCaipiaoDATA();
            int a = Convert.ToInt32(qi) + 1;
            item.QiHao = a.ToString();
            item.KaiJianRiqi = DateTime.Now.ToString("yyyy-MM-dd").ToString();
            Result.Add(item);

            clsAllnew BusinessHelp = new clsAllnew();

            BusinessHelp.SPInputclaimreport_Server(Result);


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
            return;
            if (e.ColumnIndex == 1)
            {
                //  dataGridView1.Rows[RowRemark].Cells[cloumn] = new DataGridViewComboBoxCell();

                var form = new frmTimeSelect();
                //var form1 = form.ShowDialog();

                if (form.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.Rows[RowRemark].Cells[cloumn].Value = form.dateclose;

                }
                dataGridView1.Rows[RowRemark].Cells[cloumn].Value = form.dateclose;



                if (frmTimeSelect == null)
                {
                    frmTimeSelect = new frmTimeSelect();
                    frmTimeSelect.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
                }
                if (frmTimeSelect == null)
                {
                    frmTimeSelect = new frmTimeSelect();
                }
                frmTimeSelect.Show();
            }
            //System.Drawing.Rectangle rect = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex, false);
            //frmTimeSelect.Left = rect.Left;
            //frmTimeSelect.Top = rect.Top;
            //frmTimeSelect.Width = rect.Width;
            //frmTimeSelect.Height = rect.Height;


            ////this.frmTimeSelect.Location = new System.Drawing.Point(RowRemark, cloumn);



            //DataGridViewTextBoxCell starttime = ((DataGridViewTextBoxCell)dataGridView1.Rows[e.RowIndex].Cells["qihao"]);
            //if (e.ColumnIndex == 1)
            //{
            //    _Rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

            //    //得到所在单元格位置和大小
            //    dtp.Size = new Size(_Rectangle.Width, _Rectangle.Height);

            //    //把单元格大小赋给时间控件
            //    dtp.Location = new Point(_Rectangle.X, _Rectangle.Y); //把单元格位置赋给时间控件
            //    dtp.Visible = true;  //可以显示控件了
            //    starttime.Value = DateTime.Now;


            //}
            //else
            //{
            //    dtp.Visible = false;
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
            string qi = ClaimReport_Server[ClaimReport_Server.Count - 1].QiHao;




            List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();
            inputCaipiaoDATA item = new inputCaipiaoDATA();
            item.QiHao = this.dataGridView1.Rows[RowRemark].Cells[0].EditedFormattedValue.ToString();
            item.KaiJianRiqi = this.dataGridView1.Rows[RowRemark].Cells[1].EditedFormattedValue.ToString();
            //item.JiShu1 = this.dataGridView1.Rows[RowRemark].Cells[2].EditedFormattedValue.ToString();
            //item.JiShu2 = this.dataGridView1.Rows[RowRemark].Cells[3].EditedFormattedValue.ToString();
            //item.JiShu3 = this.dataGridView1.Rows[RowRemark].Cells[4].EditedFormattedValue.ToString();
            item.Xuan = this.label8.Text;
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

            var form = new frmChangeCaiPiaodata(this.dataGridView1.Rows[RowRemark].Cells[0].EditedFormattedValue.ToString());
            //var form1 = form.ShowDialog();

            if (form.ShowDialog() == DialogResult.OK)
            {
                InitialSystemInfo();
            }
            InitialSystemInfo();

        }

        private void notifyToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
            string qi = ClaimReport_Server[ClaimReport_Server.Count - 1].QiHao;
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
    }
}
