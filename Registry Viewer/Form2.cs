using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Author: Miguel Camacho
 * 
 * Summary: Form2 is part of the RegistryManager project. It is a form that pops up when trying to 
 * edit or view data on the datagridviewer in the registry manager. It's purpose is to allow editing
 * to the Registry.
 * 
 * 
 */
///////////////////////////////////////////////////////////////////////////////////////////////
namespace My_Project
{
    public partial class Form2 : Form
    {
        private string TextBox2Text; 

        public Form2(string name, string value)
        {
            InitializeComponent();
            InitializeTextBox(name, value);

           
        }
        /// <summary>
        /// InitializeTextBox(stirng name, string value) sets the information retrived from the datagridviewer1 to the textboxes
        /// of Form2
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void InitializeTextBox(string name, string value)
        {
            textBox1.Text = name;
            textBox2.Text = value;
            TextBox2Text = value;
        }

        /// <summary>
        /// TextBoxValue is a function (I think) that retrives the Text from textBox2
        /// </summary>
        public String TextBoxValue // retrieving a value from
        {
            get
            {
                return this.textBox2.Text;
            }
        }
        /// <summary>
        /// Ok_Click closes the form without making changes to the information that has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Ok_Click(object sender, EventArgs e)
        {
      
             this.Close();
            
        }
        /// <summary>
        /// Cancel_Click closes the form but reverts the information to it's original content.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {

            this.textBox2.Text = TextBox2Text; 

            this.Close();
        }
        
    }
}
