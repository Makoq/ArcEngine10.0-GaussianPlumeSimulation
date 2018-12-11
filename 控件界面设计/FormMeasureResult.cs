using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 地信开发大作业
{
    public partial class FormMeasureResult : Form
    {
        //声明运行结果关闭事件
        public delegate void FormClosedEventHandler();
        public event FormClosedEventHandler frmClosed = null;

        public FormMeasureResult()
        {
            InitializeComponent();
        }

        //窗口关闭时引发委托事件
        private void FormMeasureResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmClosed != null)
            {
                frmClosed();
            }
        }

        private void FormMeasureResult_Load(object sender, EventArgs e)
        {

        }
    }
}
