using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MC.Buiness;
using MC.DB;

namespace MasterClassified
{
    public partial class frmSetConfig : Form
    {
        List<int> newlist;
        List<string> showSuijiResultlist = new List<string>();
        public log4net.ILog ProcessLogger;
        public log4net.ILog ExceptionLogger;


        private frmAddFanAnName frmAddFanAnName;


        public frmSetConfig()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.listBox1.SelectedIndex = 0;
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
            List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_FangAnName();
            List<FangAnLieBiaoDATA> filtered = Result.FindAll(s => s.Name != null);
            this.listBox1.DataSource = filtered;
            List<FangAnLieBiaoDATA> Result12 = BusinessHelp.Read_FangAn("YES");
            int index = 0;

            foreach (FangAnLieBiaoDATA ite in filtered)
            {

                if (Result12[0].Name == ite.Name)
                    break;
                index++;
            }
            listBox1.SelectedIndex = index;

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                NewMethod1();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);

                throw;
            }

        }

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

                //for (int i = 0; i < newlist.Count; i++)
                //{
                //    Binding mybd9 = new Binding("text", newlist, newlist[i].ToString());
                //    this.textBox1.DataBindings.Add(mybd9);
                //}

                int duan = Convert.ToInt32(comboBox1.Text);
                int evertduan = 10 / duan;
                int ilast = 0;
                ilast = duan * evertduan;

                //if (ilast > 0)
                //{
                string first = "";
                showSuijiResultlist = new List<string>();
                for (int iq = 0; iq < duan; iq++)
                {
                    string num = "";
                    int ago = 0;

                    //  for (int i = 0; i <= newlist.Count; i++)
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
                        //  showSuijiResultlist1.Add(showSuijiResultlist[ii]);

                        break;

                    }
                }


                //    showSuijiResultlist.Add(first);

                //}


                this.listBox3.DataSource = showSuijiResultlist;



                //int[,] rst = new int[3, 2];
                //int times = 0;
                //for (int i = 0, j = 0; i < 1800; i++)
                //{
                //    int newIndex = (new Random()).Next(0, newlist.Count - 1);
                //    int newValue = newlist[newIndex];
                //    newlist.RemoveAt(newIndex);
                //    times++;
                //    if (times % 2 == 0)
                //    {
                //        j++;
                //    }
                //    rst[j, times % 2] = newValue;
                //}
                //     string[] array = mySplit(1000, 11);

                // this.listBox3.DataSource = array;
                //int number = 10;//人数

                //int groups = Convert.ToInt32(comboBox1.Text);  //组数
                //  NewMethod();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return;

                throw;
            }
        }

        private void NewMethod()
        {
            int number = 10;//人数

            int groups = 7;//组数
            string[] strArr = Group(number, groups);

            for (int i = 0; i < strArr.Length; i++)
            {
                showSuijiResultlist.Add(i + 1 + "段=" + strArr[i]);
                //Console.WriteLine("第" + (i + 1) + "组 " + strArr[i]);
            }
            this.listBox3.DataSource = showSuijiResultlist;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {


            this.Close();

        }

        static string mySplit(int num, int n)//num为要拆分的数字，n为分段数
        {
            n = 5;

            int[] rules = new int[] { 5, 3, 2, 1 };
            string temp = num.ToString();
            int count = 0;
            int j = Array.IndexOf(rules, int.Parse(temp[0].ToString()));//取第一位
            if (j == -1 || !System.Text.RegularExpressions.Regex.IsMatch(temp, @"^\d0*$"))
                return "Error";
            else
            {
                string zero = temp.Substring(1);
                List<string> list = new List<string>();
                while (count < n)
                {
                    list.Add(temp);
                    j++;
                    if (j >= rules.Length)
                    {
                        j = 0;
                        if (zero.Length > 0) zero = zero.Substring(0, zero.Length - 1);
                        else break;
                    }
                    temp = rules[j].ToString() + zero;
                    count++;
                }
                string[] array = list.ToArray();
                //  this.listBox3.DataSource = array;
                // Array.Reverse(array);
                return String.Join(",", array);
            }
        }
        //static void Main(string[] args)
        //{
        //    Console.WriteLine(mySplit(1000, 11));//测试，10000分11段
        //    Console.ReadKey();
        //}

        static string[] Group(int number, int groups)
        {

            List<int> list = new List<int>();

            int num = number / groups;

            string[] strArr = new string[groups];

            for (int i = 1; i <= number; i++)

                list.Add(i);

            for (int i = 0; i < groups; i++)
            {

                for (int j = 0; j < num; j++)
                {

                    int value = list[new Random((int)DateTime.Now.Ticks).Next(0, list.Count)];

                    list.Remove(value);

                    strArr[i] += value.ToString("D2") + " ";

                    Thread.Sleep(20);

                }

            }

            return strArr;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text == null)
            {
                MessageBox.Show("请选择方案名称，在保存在其名下！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;


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
            if (this.checkBox1.Checked == true)
                item.ZhuJian = "YES";

            item.Name = this.listBox1.Text.ToString();//保存名称
            item.DuanShu = showSuijiResultlist.Count.ToString();
            Result.Add(item);
            clsAllnew BusinessHelp = new clsAllnew();
            BusinessHelp.Save_FangAn(Result);

            MessageBox.Show("保存成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {



            clsAllnew BusinessHelp = new clsAllnew();
            List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_FangAn(this.listBox1.Text.ToString());

            showSuijiResultlist = new List<string>();

            foreach (FangAnLieBiaoDATA item in Result)
            {
                if (item.Data == null)
                    continue;

                string[] temp1 = System.Text.RegularExpressions.Regex.Split(item.Data, "\r\n");

                for (int i = 1; i < temp1.Length; i++)
                {
                    showSuijiResultlist.Add(temp1[i]);
                }
                //  
            }
            this.listBox3.DataSource = showSuijiResultlist;

        }

        private void 增加ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (frmAddFanAnName == null)
            {
                frmAddFanAnName = new frmAddFanAnName("");
                frmAddFanAnName.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
            }
            if (frmAddFanAnName == null)
            {
                frmAddFanAnName = new frmAddFanAnName("");
            }
            frmAddFanAnName.Show();


        }

        void FrmOMS_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is frmAddFanAnName)
            {
                InitialSystemInfo();
                frmAddFanAnName = null;
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            {
                if (this.listBox1.Text == null)
                {
                    MessageBox.Show("请选择方案名称，在保存在其名下！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;


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
                if (this.checkBox1.Checked == true)
                    item.ZhuJian = "YES";

                item.Name = DateTime.Now.ToString("yyyyMMddHHmmss");//保存名称
                item.DuanShu = showSuijiResultlist.Count.ToString();
                Result.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Save_FangAn(Result);

                MessageBox.Show("保存成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                InitialSystemInfo();
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Text == null)
            {
                MessageBox.Show("请选择方案名称！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clsAllnew BusinessHelp = new clsAllnew();
            BusinessHelp.delete_FangAn(this.listBox1.Text);
            MessageBox.Show("删除{0}成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            InitialSystemInfo();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            {
                if (this.listBox1.Text == null)
                {
                    MessageBox.Show("请选择方案名称，在保存在其名下！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;


                }

                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
                FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();


                for (int i = 0; i < showSuijiResultlist.Count; i++)
                {
                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(showSuijiResultlist[i], "=");
                    if (i == 0)
                        item.DuanWei1 = "";
                    else if (i == 1)
                        item.DuanWei2 = "";
                    else if (i == 2)
                        item.DuanWei3 = "";
                    else if (i == 3)
                        item.DuanWei4 = "";
                    else if (i == 4)
                        item.DuanWei5 = "";
                    else if (i == 5)
                        item.DuanWei6 = "";
                    else if (i == 6)
                        item.DuanWei7 = "";
                    else if (i == 7)
                        item.DuanWei8 = "";
                    else if (i == 8)
                        item.DuanWei9 = "";
                    else if (i == 9)
                        item.DuanWei10 = "";

                    item.Data = "";
                }
                if (this.checkBox1.Checked == true)
                    item.ZhuJian = "YES";

                item.Name = this.listBox1.Text.ToString();//保存名称
                item.DuanShu = showSuijiResultlist.Count.ToString();
                Result.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Save_FangAn(Result);

                MessageBox.Show("保存成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }

        }

        private void 改名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmAddFanAnName == null)
            {
                frmAddFanAnName = new frmAddFanAnName(this.listBox1.Text);
                frmAddFanAnName.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
            }
            if (frmAddFanAnName == null)
            {
                frmAddFanAnName = new frmAddFanAnName(this.listBox1.Text);
            }
            frmAddFanAnName.Show();


        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                if (this.listBox1.Text == null)
                {
                    MessageBox.Show("请选择方案名称，在保存在其名下！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;


                }

                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
                FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();


                for (int i = 0; i < showSuijiResultlist.Count; i++)
                {
                    string[] temp1 = System.Text.RegularExpressions.Regex.Split(showSuijiResultlist[i], "=");
                    if (i == 0)
                        item.DuanWei1 = "";
                    else if (i == 1)
                        item.DuanWei2 = "";
                    else if (i == 2)
                        item.DuanWei3 = "";
                    else if (i == 3)
                        item.DuanWei4 = "";
                    else if (i == 4)
                        item.DuanWei5 = "";
                    else if (i == 5)
                        item.DuanWei6 = "";
                    else if (i == 6)
                        item.DuanWei7 = "";
                    else if (i == 7)
                        item.DuanWei8 = "";
                    else if (i == 8)
                        item.DuanWei9 = "";
                    else if (i == 9)
                        item.DuanWei10 = "";

                    item.Data = "";
                }
                if (this.checkBox1.Checked == true)
                    item.ZhuJian = "YES";

                item.Name = this.listBox1.Text.ToString();//保存名称
                item.DuanShu = showSuijiResultlist.Count.ToString();
                Result.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Save_FangAn(Result);

                MessageBox.Show("保存成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }

        }
    }
}
