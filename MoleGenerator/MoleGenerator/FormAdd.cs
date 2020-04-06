using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoleGenerator
{
    public partial class FormAdd : Form
    {
        public Form1 mainForm;
        public FormAdd()
        {
            InitializeComponent();
        }
        public FormAdd(Form1 f1)
        {
            InitializeComponent();
            mainForm = f1;
        }
        
        private void buttonOK_Click(object sender, EventArgs e)
        {
            mainForm.newName= textBoxName.Text;
            mainForm.newMail= textBoxMail.Text;
            this.Close();
        }
    }
}
