using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
///////////////////////////////////////////////////////////////////////////
/*Author Miguel Camacho
 * 
 * Form3 is a form that deals with tracking specific values and sets them
 * to user specifications and keep them from change
 * 
 * 
 * 
 * 
 */
///////////////////////////////////////////////////////////////////////////
namespace My_Project
{
    public partial class Form3 : Form
    {
        private LineItem NewItem = new LineItem(); 
        public Form3()
        {
            InitializeComponent();
            InitializeList();
        }
        public Form3(RegistryKey CurrentHKey, RegistryKey CurrentSubKey, string name, string type, string data)
        {
            /*
            MessageBox.Show(CurrentHKey.ToString());
            MessageBox.Show(CurrentSubKey.ToString());
            MessageBox.Show("Name: " + name);
            MessageBox.Show("Type: " + type);
            MessageBox.Show("Data: " + data);
             */ 
            NewItem.SetRootKey(CurrentHKey);
            NewItem.SetSubkey(CurrentSubKey);
            NewItem.SetValueName(name);
            NewItem.SetValueType(type);
            NewItem.SetValueData(data);
            InitializeComponent();
            LoadGrid();
        }
        private void LoadGrid()
        {
            //int CurrentIndex = 0;
            ///
            //Load from xml file here
            ///
            /// 
            ///Add new here
            ////decision; make a single line item then a box like form2 with pertinent info
            dataGridView1.Rows.Add("Unconsolidated",NewItem.GetSubkey(),NewItem.GetValueName().ToString(),NewItem.GetValueType().ToString(),NewItem.GetValueData().ToString(),"Origninal");
            //dataGridView1.Rows.Add("Unconsolidated", NewItem.GetSubkey(), NewItem.GetValueName().ToString(), NewItem.GetValueType().ToString(), NewItem.GetValueData().ToString(),"User Set");
        }
        private void Commit_Click(object sender, EventArgs e)
        {

        }
        private void InitializeList()
        {
            Loadlist();
        }
        private void Loadlist()
        {
            XmlTextReader reader = new XmlTextReader("CurrentKeys.xml");
              //PathAndValues Loader = new PathAndValues();
            while (reader.Read())
            {
            // Load list here
            }

        }
        private void AddItem()
        {
        }
        private void Add_Click(object sender, EventArgs e)
        {
            //Form1 Temp = new Form1(); 
        }
        




       

    }
    /// <summary>
    /// Local Data Structure that is designed to store information
    /// </summary>
    public class LineItem
    {
  
        RegistryKey RootKey;
        RegistryKey SubKeyPath;
        String ValueName;
        String ValueType;
        String ValueData;

        public void SetRootKey(RegistryKey RootKey)
        {
            this.RootKey = RootKey;
        }
        public RegistryKey GetRootKey()
        {
           return RootKey;
        }
        public void SetSubkey(RegistryKey SubKeyPath)
        {
            this.SubKeyPath = SubKeyPath;
        }
        public RegistryKey GetSubkey()
        {
            return SubKeyPath;
        }
        public void SetValueName(string ValueName)
        {
            this.ValueName = ValueName;
        }
        public string GetValueName()
        {
            return ValueName;
        }
        public void SetValueType(string ValueType)
        {
            this.ValueType = ValueType;
        }
        public string GetValueType()
        {
            return ValueType;
        }

        public void SetValueData(string ValueData)
        {
            this.ValueData = ValueData; 
        }
        public string GetValueData()
        {
            return ValueData;
        }
    }
   



}
