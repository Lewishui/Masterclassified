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
    public partial class frmMessageShow : Form
    {
        private int _status;
        public frmMessageShow(string title, string message, int status)
        {
            InitializeComponent(); 
            setTitle(title);
            setMessage(message);
            setStatus(status);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        public void setTitle(string title)
        {
            this.Text = title;
        }

        public void setMessage(string message)
        {
            this.labMessage.Text = message;
            toolTip1.SetToolTip(this.labMessage, this.labMessage.Text);
        }

        public void setStatus(int status)
        {
            _status = status;
            if (status == 0)
            {
                this.btnOK.Enabled = true;
            }
            else if (status == 1)
            {
                this.btnOK.Enabled = false;
            }
        }

        public void setInfo(string message, int status)
        {
            if (message != null && message != "")
            {
                setMessage(message);
                setStatus(status);
            }
        }



        private void frmMessageShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._status == 1)
            {
                e.Cancel = true;
            }
        }
    }
}
