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
		String vartype = "";

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

		//change value of variable type
		public void setVarType(String s)
		{
			this.vartype = s;
		}
		
		public Boolean typeMatches(String checktype, String input)
		{
			Double d = 0;//temporary storage for double.tryParse
			
			switch(checktype){
				case "PLOWT":  if(Double.TryParse(input,out d))//check if input is a number
								return true;
							break;
				case "INTEDYER": //check if input is a number and does not contain a decimal point
                                if(Double.TryParse(input,out d) && !input.Contains("."))
								    return true;
                            break;
                case "KAR": //check if user input is only one character
                            if (input.Length == 1) return true;
                            break;
				case "ISTRING": //check if user has input any character
                            if (input.Length > 0) return true;
                            break;
				case "BULYAN": //check if input is "totoo" or "peke"
								if(input=="totoo" || input=="peke")
									return true;
                            break;
			}
			
			return false;
		}
		
        public bool isClosed()
        {
            if (closed == true) return true;
            else return false;
        }

        public void button1_Click(object sender, EventArgs e)
        {
			//get the value entered by the user
			String str = textBox1.Text;
			
			//check the type of variable
			if(typeMatches(vartype,str)){
				this.Close();
				closed = true;
			}
        }
    }
}
