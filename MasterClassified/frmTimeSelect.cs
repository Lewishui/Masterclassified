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
    public partial class frmTimeSelect : Form
    {
        public string dateclose = "";

        public frmTimeSelect()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dateclose = dateTimePicker1.SelectionEnd.ToString("yyyy/MM/dd");
            this.Close();
        }
    }
}
