using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MC.Buiness;
using MC.DB;
using System.Reflection;
using WeifenLuo.WinFormsUI.Docking;
using System.Text.RegularExpressions;

namespace MasterClassified
{
    public partial class frmDataCenter : DockContent
    {
        List<int> newlist;
        List<string> showSuijiResultlist = new List<string>();
        int RowRemark = 0;
        int cloumn = 0;
        private SortableBindingList<inputCaipiaoDATA> sortablePendingOrderList;
        private SortableBindingList<FangAnLieBiaoDATA> sortablePendingOrderList1;
        List<inputCaipiaoDATA> ClaimReport_Server;
        public frmDataCenter()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

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
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsAllnew BusinessHelp = new clsAllnew();
                int s = this.tabControl1.SelectedIndex;
                if (s == 0)
                {

                    ClaimReport_Server = new List<inputCaipiaoDATA>();
                    ClaimReport_Server = BusinessHelp.ReadclaimreportfromServer();
                    //  ClaimReport_Server.Sort(new Comp());

                    sortablePendingOrderList = new SortableBindingList<inputCaipiaoDATA>(ClaimReport_Server);

                    this.bindingSource1.DataSource = null;
                    this.bindingSource1.DataSource = sortablePendingOrderList;

                    this.dataGridView1.DataSource = this.bindingSource1;
                    //this.dataGridView1.DataSource = null;
                    //this.dataGridView1.AutoGenerateColumns = false;
                    //if (ClaimReport_Server.Count != 0)
                    //{
                    //    this.dataGridView1.DataSource = ClaimReport_Server;
                    //}
                }
                else if (s == 1)
                {
                    List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_AllFangAn();
                    sortablePendingOrderList1 = new SortableBindingList<FangAnLieBiaoDATA>(Result);

                    this.bindingSource2.DataSource = null;
                    this.bindingSource2.DataSource = sortablePendingOrderList1;

                    this.dataGridView2.DataSource = this.bindingSource2;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return;

                throw;
            }
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

        private void NewMethod1()
        {
            try
            {
                newlist = new List<int>();
                showSuijiResultlist = new List<string>();

                newlist.Add(0);
                newlist.Add(1);
                newlist.Add(2);
                newlist.Add(3);
                newlist.Add(4);
                newlist.Add(5);
                newlist.Add(6);
                newlist.Add(7);
                newlist.Add(8);
                newlist.Add(9);
                newlist = newlist.Select(a => new { a, newID = Guid.NewGuid() }).OrderBy(b => b.newID).Select(c => c.a).ToList();

                int duan = 3;
                int evertduan = 10 / duan;
                int ilast = 0;
                ilast = duan * evertduan;


                string first = "";
                showSuijiResultlist = new List<string>();
                for (int iq = 0; iq < duan; iq++)
                {
                    string num = "";
                    int ago = 0;

                    for (int i = 0; i <= evertduan; i++)
                    {
                        ago++;
                        if (ago > evertduan)
                            break;

                        num = num + " " + newlist[0];
                        newlist.RemoveAt(0);

                    }
                    first = first + "\r\n" + iq.ToString() + "段=" + " " + num;

                    showSuijiResultlist.Add(iq.ToString() + " 段= " + " " + num);

                }
                List<string> showSuijiResultlist1 = new List<string>();

                for (int ii = 0; ii < showSuijiResultlist.Count; ii++)
                {
                    for (int i = 0; i < newlist.Count; i++)
                    {
                        showSuijiResultlist[ii] = showSuijiResultlist[ii] + " " + newlist[i];
                        newlist.RemoveAt(i);
                        break;
                    }
                }

                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
                FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();


                for (int i = 0; i < showSuijiResultlist.Count; i++)
                {
                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(showSuijiResultlist[i], "=");
                    if (i == 0)
                        item.DuanWei1 = temp1[1].Trim();
                    else if (i == 1)
                        item.DuanWei2 = temp1[1].Trim();
                    else if (i == 2)
                        item.DuanWei3 = temp1[1].Trim();
                    else if (i == 3)
                        item.DuanWei4 = temp1[1].Trim();
                    else if (i == 4)
                        item.DuanWei5 = temp1[1].Trim();
                    else if (i == 5)
                        item.DuanWei6 = temp1[1].Trim();
                    else if (i == 6)
                        item.DuanWei7 = temp1[1].Trim();
                    else if (i == 7)
                        item.DuanWei8 = temp1[1].Trim();
                    else if (i == 8)
                        item.DuanWei9 = temp1[1].Trim();
                    else if (i == 9)
                        item.DuanWei10 = temp1[1].Trim();

                    item.Data = item.Data + "\r\n" + showSuijiResultlist[i];
                }
                item.ZhuJian = "YES";
                item.Name = "默认方案";//保存名称
                item.DuanShu = showSuijiResultlist.Count.ToString();
                item.Mobanleibie = "默认";

                Result.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Save_FangAn(Result);

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return;

                throw;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowRemark = e.RowIndex;
            cloumn = e.ColumnIndex;
        }

        private void notifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int s = this.tabControl1.SelectedIndex;
            if (s == 0)
            {
                if (RowRemark >= dataGridView1.Rows.Count)
                {
                    RowRemark = RowRemark - 1;
                }
                string QiHao = this.dataGridView1.Rows[RowRemark].Cells["_id"].EditedFormattedValue.ToString();
                clsAllnew BusinessHelp = new clsAllnew();

                BusinessHelp.deleteID_CaiPiaoData(QiHao);
            }
            else if (s == 1)
            {
                if (RowRemark >= dataGridView2.Rows.Count)
                {
                    RowRemark = RowRemark - 1;
                }
                string QiHao = this.dataGridView2.Rows[RowRemark].Cells["_id"].EditedFormattedValue.ToString();
                clsAllnew BusinessHelp = new clsAllnew();

                BusinessHelp.deleteID_FangAn(QiHao);
            }

            #region MyRegion

            NewMethod();

            #endregion
        }

        private void NewMethod()
        {
            try
            {
                clsAllnew BusinessHelp = new clsAllnew();
                int s = this.tabControl1.SelectedIndex;
                if (s == 0)
                {

                    ClaimReport_Server = new List<inputCaipiaoDATA>();
                    ClaimReport_Server = BusinessHelp.ReadclaimreportfromServer();
                    //  ClaimReport_Server.Sort(new Comp());

                    sortablePendingOrderList = new SortableBindingList<inputCaipiaoDATA>(ClaimReport_Server);

                    this.bindingSource1.DataSource = null;
                    this.bindingSource1.DataSource = sortablePendingOrderList;

                    this.dataGridView1.DataSource = this.bindingSource1;
                    //this.dataGridView1.DataSource = null;
                    //this.dataGridView1.AutoGenerateColumns = false;
                    //if (ClaimReport_Server.Count != 0)
                    //{
                    //    this.dataGridView1.DataSource = ClaimReport_Server;
                    //}
                }
                else if (s == 1)
                {

                    List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_AllFangAn();
                    sortablePendingOrderList1 = new SortableBindingList<FangAnLieBiaoDATA>(Result);

                    this.bindingSource2.DataSource = null;
                    this.bindingSource2.DataSource = sortablePendingOrderList1;

                    this.dataGridView2.DataSource = this.bindingSource2;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return;

                throw;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                NewMethod1();
                MessageBox.Show("初始化成功，请到数据分析中设置下的方案界面查询");


            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return;

                throw;
            }

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowRemark = e.RowIndex;
            cloumn = e.ColumnIndex;
        }

    }
}
