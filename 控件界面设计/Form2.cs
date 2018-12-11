using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 地信开发大作业.控件界面设计
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string show()
        {
            this.ShowDialog();
            return richTextBox1.Text;
        }       
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
