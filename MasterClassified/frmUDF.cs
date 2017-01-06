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
    public partial class frmUDF : Form
    {

      public  List<int> JIDTA = new List<int>();

        public frmUDF()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JIDTA = new List<int>();

            if (checkBox1.Checked == true)
                JIDTA.Add(1);
            if (checkBox2.Checked == true)
                JIDTA.Add(2);
            if (checkBox3.Checked == true)
                JIDTA.Add(3);
            if (checkBox4.Checked == true)
                JIDTA.Add(4);
            if (checkBox5.Checked == true)
                JIDTA.Add(5);
            if (checkBox6.Checked == true)
                JIDTA.Add(6);
            if (checkBox7.Checked == true)
                JIDTA.Add(7);
            if (checkBox8.Checked == true)
                JIDTA.Add(8);
            if (checkBox9.Checked == true)
                JIDTA.Add(9);
            if (checkBox10.Checked == true)
                JIDTA.Add(10);


            if (JIDTA.Count > 0)
                this.Close();
            else
                MessageBox.Show("请选择要分析的条目，否则请点击取消关闭窗口", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
