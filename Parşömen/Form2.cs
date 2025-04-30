using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parşömen
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public Form2(string title, string promptText, string[] options)
        {
            InitializeComponent();
            this.Text = title;
            label1.Text = promptText;
            comboBox1.Items.AddRange(options);
            comboBox1.SelectedIndex = 0;
        }

        public string SelectedOption
        {
            get
            {
                return comboBox1.SelectedItem.ToString();
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
