using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cmsc214project
{
    public partial class Form2 : Form
    {
        bool closed = false;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public bool isClosed()
        {
            if (closed == true) return true;
            else return false;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            closed = true;
        }
    }
}
