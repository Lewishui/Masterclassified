using MC.Buiness;
using MC.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MasterClassified
{
    public partial class frmSelfPiLiangFangAN : Form
    {
        List<string> showSuijiResultlist = new List<string>();
        List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
        string checkinfo;

        public string fanganindex;
        List<inputCaipiaoDATA> ClaimReport_ServerNew;
        public List<string> oncheckinfo;

        public frmSelfPiLiangFangAN(List<inputCaipiaoDATA> ClaimReport_Server)
        {
            InitializeComponent();
            ClaimReport_ServerNew = new List<inputCaipiaoDATA>();
            Result = new List<FangAnLieBiaoDATA>();
            clsAllnew BusinessHelp = new clsAllnew();
            Result = BusinessHelp.Read_Piliang_AllFangAn();
            this.listBox1.DisplayMember = "Name";
            List<FangAnLieBiaoDATA> filtered = Result.FindAll(s => s.Name != null);
            this.listBox1.DataSource = filtered;
            if (Result.Count > 0)
                listBox1.SelectedIndex = 0;

            listBox1_Click(this, EventArgs.Empty);
            string[] temp1 = System.Text.RegularExpressions.Regex.Split(ClaimReport_Server[0].KaiJianHaoMa, " ");
            for (int i = 0; i < temp1.Length; i++)
            {
                int kdkd = i + 1;
                this.checkedListBox1.Items.Add("第" + kdkd + "位");

            }
            oncheckinfo = new List<string>();

        }

        private void listBox1_Click(object sender, EventArgs e)
        {

            //List<FangAnLieBiaoDATA> Result = BusinessHelp.Read_FangAn(this.listBox1.Text.ToString());

            List<FangAnLieBiaoDATA> Result1 = Result.FindAll(s => s.Name != null && s.Name == this.listBox1.Text.ToString());
            showSuijiResultlist = new List<string>();

            foreach (FangAnLieBiaoDATA item in Result1)
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

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            fanganindex = this.listBox1.Text.ToString();
            //List<string> alist = new List<string>();
            //if (this.checkedListBox1.CheckedItems.Count > 0)
            //{
            //    foreach (string status in this.checkedListBox1.CheckedItems)
            //    {
            //        alist.Add(status);
            //    }
            //}
            fanganindex = checkinfo + fanganindex;
            if (checkinfo != null)
            {
                var item = oncheckinfo.Find((x) => { return x.Contains(checkinfo); });
                if (item == null)
                    oncheckinfo.Add(fanganindex);
                else
                {
                    oncheckinfo.Remove(item);
                    oncheckinfo.Add(fanganindex);
                }
            }
            //   this.Close();
            List<string> alist = new List<string>();
            if (this.checkedListBox1.CheckedItems.Count > 0)
            {
                foreach (string status in this.checkedListBox1.CheckedItems)
                {
                    var item1 = oncheckinfo.Find((x) => { return x.Contains(status); });
                    if (item1 != null)
                        alist.Add(status + "-" + item1.Replace(status, ""));

                }
            }
            this.listBox2.DataSource = null;
            this.listBox2.DataSource = alist;

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem != null)
                checkinfo = checkedListBox1.SelectedItem.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            oncheckinfo = new List<string>();

            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
