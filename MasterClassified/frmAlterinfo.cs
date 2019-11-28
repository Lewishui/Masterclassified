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
    public partial class frmAlterinfo : Form
    {
        public frmAlterinfo(string iun)
        {
            InitializeComponent();
            this.labMessage.Text = iun;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
