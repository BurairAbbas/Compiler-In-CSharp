using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CC_Compiler_In_WFA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateToken_Click(object sender, EventArgs e)
        {
            tokentb.Text = string.Empty;
            string[] code = Codetb.Lines;
            LexicalAnalysis la = new LexicalAnalysis(code);
            foreach (var temp in la.GetToken)
            {
                tokentb.Text += temp + "\r\n";
            }
        }
    }
}
