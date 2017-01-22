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
    public partial class frmChangeCaiPiaodata : Form
    {
        List<inputCaipiaoDATA> ReadResult;
        string ming = "";

        public frmChangeCaiPiaodata(string qihao ,string mingcheng)
        {
            InitializeComponent();
            clsAllnew BusinessHelp = new clsAllnew();
            ReadResult = new List<inputCaipiaoDATA>();

            ReadResult = BusinessHelp.ReadCaiPiaoData_One(qihao, mingcheng);
            foreach (inputCaipiaoDATA item in ReadResult)
            {
                if (item.QiHao != null)
                    textBox1.Text = item.QiHao;
                if (item.KaiJianRiqi != null)
                    textBox2.Text = item.KaiJianRiqi;
                if (item.KaiJianHaoMa != null)
                    textBox3.Text = item.KaiJianHaoMa;

            }
            ming = mingcheng;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {


                List<inputCaipiaoDATA> Result = new List<inputCaipiaoDATA>();
                inputCaipiaoDATA item = new inputCaipiaoDATA();
                item.QiHao = textBox1.Text;
                item.KaiJianRiqi = textBox2.Text;
                item.KaiJianHaoMa = textBox3.Text.Trim();
                item.Xuan = ReadResult[0].Xuan;
                item.Caipiaomingcheng = ming.ToString();
                Result.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.SPInputclaimreport_Server(Result);

                this.Close();

            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
