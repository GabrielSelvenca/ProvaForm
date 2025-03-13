using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiorentinoForm.Components
{
    public partial class LetterControl : UserControl
    {
        public LetterControl()
        {
            InitializeComponent();
        }

        public LetterControl(Form1 form1, char i)
        {
            InitializeComponent();
            Form1 = form1;
            I = i;
            label1.Text = i.ToString();
        }

        public Form1 Form1 { get; }
        public char I { get; }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
