using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MC.Buiness;
using MC.DB;

namespace MasterClassified
{
    public partial class frmAddFanAnName : Form
    {
        string changname = "";


        public frmAddFanAnName(string name)
        {
            InitializeComponent();
            changname = name;

            if (name != "")
            {
                this.Text = "更改名称";

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (changname != "")
            {
                if (textBox1.Text == null || textBox1.Text == "")
                {
                    MessageBox.Show("方案名称不能为空，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
          
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Update_FangAn(changname, textBox1.Text.Trim().ToString());

                MessageBox.Show("修改成功 ！", "确认", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }
            else
            {
                if (textBox1.Text == null || textBox1.Text == "")
                {
                    MessageBox.Show("方案名称不能为空，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<FangAnLieBiaoDATA> Result = new List<FangAnLieBiaoDATA>();
                FangAnLieBiaoDATA item = new FangAnLieBiaoDATA();
                item.Name = textBox1.Text.Trim();//保存名称
                Result.Add(item);
                clsAllnew BusinessHelp = new clsAllnew();
                BusinessHelp.Save_FangAn(Result);

                MessageBox.Show("创建成功 ！", "确认", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

        }
    }
}
