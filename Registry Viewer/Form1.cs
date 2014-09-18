using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32; 
////////////////////////////////////////////////////////////////////////////
/* Author: Miguel Camacho + Isaac Daniel Reyna 
 * 
 *This is my initial project. It is suppose to be a registry manager application that is suppose to update certain registries
 * and edit them. 
 * 
 * Terms: 
 * 
 * Keys is the term for the directories
 * Root Keys or HKEYS: is the term for the root keys of a Windows' based registry
 * 
 * The following Namespace is needed for this project aside from the ones added by the compilier:
 * Microsoft.Win32; 
 * 
 * Important Objects: 
 * treeview1: treeview1 is a the object name of the treeview in use in this program
 * 
 * imagelist1: imagelist1 is an object that stores and provides the ability to reference images for the program.
 * 
 * Form2: is a form to edit Value data of the keys 
 * 
 * Form3: This object contains the folder images.
 * 
 * datagridview1: is the area that is set to contain the values of a key 
 * 
 * contextMenuStrip1: is an object that acts like a right click menu in this form
 * 
 * menuStrip1: is an object that functions like the Traditional Menu strip item. 
 * 
 * Non Important Objects:
 * 
 * label1: label1 is a display label meant to display the path of a selected node at the bottom of the program
 * 
 *
 * 
 * Notes: There will be spaces between declarations, operations, logic statements, and returns
 * 
 * Compilier: VS 2012 v. 11.0.507727.1 RTMREL
 * ¹²³⁴⁵⁶⁷⁸⁹⁰
 * 
 */
//////////////////////////////////////////////////////////////////////////// 
namespace My_Project
{


    public partial class Form1 : Form
    {
        /// <summary>
        /// These are private vaiables needed for the program
        /// 
        /// HKEYS is a list of Registry Keys manually loaded with all the main Keys in Windows Registry.
        /// The keys are loaded in initializeHkeyList() 
        /// StartingPoint is a permanent index to ignore "Computer\" from the path
        /// </summary>
        private List<RegistryKey> HKEYS = new List<RegistryKey>();
        private RegistryKey CurrentHKey;
        private RegistryKey CurrentSubKey;
        private static readonly byte StartingPoint = 9;
        private RowContainer CurrentRow = new RowContainer(); 
        /// <summary>
        /// form1() where all the magic happens or the main() of this program
        /// IntializeComponent() is standard by the compilier
        /// </summary>
        
        public Form1()
        {
            InitializeComponent();
            initializeHkeyList();
            initializeTree();
        }

        /// <summary>
        /// initializeTree() is a function that initializes the intial treeview with the purely cosmetic but represantational 
        /// "Computer" Node named initNode. [¹]
        /// 
        /// Assigns this node to as the root of the tree [²]
        /// 
        /// Assigns imagelist1 as the reference imagelist to treeview1 and assigns the images to the treeview
        /// and the initial image when node are selected and unselected.
        /// 
        /// Uses a foreach loop to loop through the HKEYS list to make and create a base node and a default empty child node
        /// attaches the child node to the parent and then the parent is subsequently attached to the initNode
        /// </summary>

        private void initializeTree()
        {
            TreeNode initNode = new TreeNode(); 
            initNode.Text = "Computer"; 

            treeView1.Nodes.Add(initNode);

            treeView1.ImageList = imageList1;
            treeView1.ImageIndex = 0;
            treeView1.SelectedImageIndex = 4;

            foreach (RegistryKey v in HKEYS)
            {
                TreeNode baseNode = new TreeNode();
                TreeNode childofBase = new TreeNode();

                baseNode.Text = v.Name;
                childofBase.Text = "Empty";
                
                baseNode.Nodes.Add(childofBase);
                initNode.Nodes.Add(baseNode);
            }
        }

        /// <summary>
        /// initializeHkeyList() just manually adds the base registry keys to a private list of RegistryKey type
        /// </summary>
        /// 
        private void initializeHkeyList()
        {
            HKEYS.Add(Registry.ClassesRoot);
            HKEYS.Add(Registry.CurrentUser);
            HKEYS.Add(Registry.LocalMachine);
            HKEYS.Add(Registry.Users);
            HKEYS.Add(Registry.CurrentConfig);
        }

        /// <summary>
        /// treeView1_AfterSelect(object sender, TreeViewEventArgs e) 
        /// 
        /// initializes function displayPath() after a node has been selected
        /// 
        /// initializes function FillDataGridView(e) upon selecting a node
        /// 
        /// initializes funcion SetCurrentNode(e) up selecting a node
        /// </summary>
        
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            displayPath(e);
            FillDataGridView(e);
            SetCurrentNode(e); 
            
        }
        /// <summary>
        /// SetCurrentNode(TreeViewEventArgs e) is a function that sets the current path into a global variable
        /// </summary>
        /// <param name="e"></param>
        private void SetCurrentNode(TreeViewEventArgs e)
        {
            string regispath = FindRegistryPath(e.Node.FullPath);
            string subkeypath = getSubKeyPath(e.Node.FullPath);

            foreach (RegistryKey v in HKEYS)
            {
                if (regispath == v.Name)
                {
                    CurrentHKey = v;
                    CurrentSubKey = v.OpenSubKey(@subkeypath, true);
                }
            }
            
        }

        /// <summary>
        /// treeview1_BeforeExpand(object sender, TreeViewCanacelEventArgs e) is an important function to the program
        /// loads new node before the expansion of a node on the treeview
        /// 
        /// Adds new nodes if a node is a named empty 
        /// 
        /// e is a reference to the object clicked
        /// 
        /// Calls BEaddpath() it sends the path of the node that was removed and the node that was selected as a reference 
        /// </summary>
       
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Text.Equals("Computer") == false)
            {
                if (e.Node.Nodes[0].Text == "Empty")
                {
                    e.Node.Nodes[0].Remove();
                    BEaddpath(e.Node.FullPath, e.Node);
                }
            }  
 
        }

        /// <summary>
        /// BEaddpath(string path, TreeNode Node) is a method that is suppose to add keys upon selecting the node 
        /// </summary>
       
        private void BEaddpath(string path, TreeNode Node)
        {

            ///Needs to discriminate between keys and HKEYS so it won't do this operation for the HKEYs
            string RegisPath = "";
            string SubKeyPath = "";
            string[] sk;
            int count = 0; 
            RegisPath = FindRegistryPath(path);
            SubKeyPath = getSubKeyPath(path);
         
            if (SubKeyPath.Equals("") == true )
            {
                foreach (RegistryKey v in HKEYS)
                {
                    if (RegisPath == v.Name)
                    {
   
                        RegistryKey subkeys = v.OpenSubKey(@SubKeyPath, true);
                        sk = subkeys.GetSubKeyNames();
             
                        foreach (string k in sk)
                        {

                            TreeNode Temp = new TreeNode();
                            Temp.Text = k;

                            try
                            {
                                RegistryKey TempKey = v.OpenSubKey(k, true);

                                if (TempKey.SubKeyCount > 0)
                                {
                                    Temp.Nodes.Add("Empty");
                                }
                            }
                            catch (Exception)
                            {
                                Temp.BackColor = Color.Red;
                                Temp.BackColor = Color.White;
                                RegistryKey TempKey = v.OpenSubKey(k, true);
                                if (TempKey.SubKeyCount > 0)
                                {
                                    //Temp.Nodes.Add("Restricted");
                                }
                            }

                            Node.Nodes.Add(Temp);
                        }

                    }
                }

            }
            else
            {
                foreach (RegistryKey v in HKEYS)
                {
                    if (RegisPath.Equals(v.Name)== true)
                    {
                        count = 0;
                        RegistryKey Key = v.OpenSubKey(@SubKeyPath);
                        sk = Key.GetSubKeyNames(); 

                        foreach (string k in sk)
                        {

                            TreeNode Temp = new TreeNode();
                            Temp.Text = k;

                            try
                            {
                                RegistryKey TempKey = Key.OpenSubKey(k, true);

                                if (TempKey.SubKeyCount > 0)
                                {
                                    Temp.Nodes.Add("Empty");
                                }
                            }
                            catch (Exception)
                            {
                                Temp.BackColor = Color.Red;
                                Temp.BackColor = Color.White;
                            }

                            Node.Nodes.Add(Temp);
                            count++;
                        }
                    }
                }

            }

        }

        /// <summary>
        /// FindRegistryPath() just finds the corresponding HKEY related to the path of the node that is sent through as a parameter string path
        /// and adds subkeys to the node if they exist
        /// </summary>

        private string FindRegistryPath(string path)
        {

            string pathOrigin = "";
            string tempPath = "";
            int MaxLength = path.Length - 1;
            int index = 0;

            if (StartingPoint <= MaxLength)
            {
                tempPath = path.Substring(StartingPoint);
                index = tempPath.IndexOf(@"\");
                //   MessageBox.Show("FRP index: " + index);
                if (index > 0)
                {
                    pathOrigin = path.Substring(StartingPoint, index);
                }
                else
                {
                    pathOrigin = path.Substring(StartingPoint, MaxLength - (StartingPoint - 1));
                }
                // MessageBox.Show("FRP pathOrigin: " + pathOrigin);
            }

            return pathOrigin;
        }

        /// <summary>
        /// displayPath() is a simple function that defines the object Label1 with the path of the currently selected Node
        /// "e" is a reference to the selected node on the treeview1
        /// 
        /// If no node is selected nothing will display
        /// </summary>
        /// <param name="e">TreeviewEventArgs</param>

        private void displayPath(TreeViewEventArgs e)
        {
            if (e.Node.Text != "Computer")
            {
                label1.Text = e.Node.FullPath;
            }
            else
            {
                label1.Text = "";
            }
        }
        /// <summary>
        /// getSubKeyPath(string path) is a function that returns the path minus both the initial "Computer\" and the particular HEKY it belongs to.
        /// it does this by using the variabel StartingPoint and then skipping the RegistryKey HKEY with IndexOf method
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string getSubKeyPath(string path)
        {

            string pathOrigin = "";
            int index = 0;
            int MaxLength = path.Length - 1;
            if( StartingPoint <= MaxLength)
            {
                pathOrigin = path.Substring(StartingPoint);
                index = pathOrigin.IndexOf(@"\");
                // MessageBox.Show("GSP index: " + index);
                if (index > 0)
                {
                  pathOrigin = pathOrigin.Substring(index + 1);
                }
                else//if no backslash is found
                {
                  pathOrigin = "";
                }
                //MessageBox.Show("GSP pathOrigin: " + pathOrigin);
               
            }
            return pathOrigin;
        }

        /// <summary>
        /// FillDataGridView(TreeViewEventArgs e) is a function that fills the datagridviewer1 with key information 
        /// </summary>
        /// <param name="e"></param>

        private void FillDataGridView(TreeViewEventArgs e)
        {
            string[] Values;
            string CurrentPath = getSubKeyPath(e.Node.FullPath);
            string CurrentHkey = FindRegistryPath(e.Node.FullPath); 
            TreeNode CurrentNode = e.Node;

            if (CurrentPath.Equals("") == false)
            {
                //   MessageBox.Show("Jump in if");
                foreach (RegistryKey v in HKEYS)
                {
                    if (CurrentHkey == v.Name)
                    {
                        RegistryKey SubKey = v.OpenSubKey(@CurrentPath, true );
                        Values = SubKey.GetValueNames();
                        
                        dataGridView1.Rows.Clear();
                        dataGridView1.Rows.Add("(Default)", "REG_SZ", "(value not set)");
                        //MessageBox.Show("Before the foreach");
                        foreach (String value in Values)
                        {

                           // MessageBox.Show("Hello from the foreach loop"); 
                            String data = Registry.GetValue(v.Name + "\\" + CurrentPath, value, "0").ToString();
                            String type = v.OpenSubKey(CurrentPath).GetValueKind(value).ToString();
                            CurrentRow.Name = value;
                            CurrentRow.Data = data;
                            CurrentRow.Type = type;
                            //int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                            //MessageBox.Show("Data: "+data);
                            //MessageBox.Show("Type: "+type); 
                            dataGridView1.Rows.Add(value, type, data);
                        }
                    }

                }

            }

            //RegistryKey CurrentSubKey = CurrentPath.
            //HKEYS.FindIndex(CurrentHkey); 
        }


        /// <summary>
        /// dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e) is a function that allows for the 
        /// editing of values in the subkeys. It retrives the index of the item desired to be changed and writes it in. Upon completion 
        /// the popup is disposed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                int NameColumnIndex = e.ColumnIndex;
                int NameRowIndex = e.RowIndex;
                string Name = dataGridView1.Rows[NameRowIndex].Cells[NameColumnIndex].Value.ToString();
                string Value = dataGridView1.Rows[NameRowIndex].Cells[NameColumnIndex + 2].Value.ToString();

                Form2 PopUpEdit = new Form2(Name, Value);

                if (PopUpEdit.ShowDialog() == DialogResult.OK)
                {

                }

                dataGridView1.Rows[NameRowIndex].Cells[NameColumnIndex + 2].Value = PopUpEdit.TextBoxValue;
                Value = dataGridView1.Rows[NameRowIndex].Cells[NameColumnIndex + 2].Value.ToString();
                CurrentSubKey.SetValue(Name, Value);

                PopUpEdit.Dispose();
            }
        }

        /// <summary>
        /// exitToolStripMenuItem_Click(object sender, EventArgs e) is a simple exit for the "File" Menu item sub Item "Exit"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// setValuesToKeysToolStripMenuItem_Click(object sender, EventArgs e) is a function that controls  when you 
        /// click on the option Set Values to Keys which is set to open the form to set the values of the keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void setValuesToKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 DataView = new Form3();
            DataView.ShowDialog();
            DataView.Dispose(); 
        }

        /// <summary>
        /// treeView1_NodeMouseClick(object sender, DataGridViewCellEventArgs e) is a function mainly to handle
        /// right clicks but in the treeview object on nodes. No function as of yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
           
        }

        /// <summary>
        /// SetVal_Click(object sender, EventArgs e) is the handler for clicking a the SetVal option in the right click menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void SetVal_Click(object sender, EventArgs e)
        {
            Form3 Temp = new Form3(CurrentHKey,CurrentSubKey,CurrentRow.Name,CurrentRow.Type,CurrentRow.Data);
            Temp.ShowDialog();
        }

        /// <summary>
        /// dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) handles the right click event by 
        /// highlighting cells on right click and making them the obeject of reference.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Selected = false;
            }

            dataGridView1.Rows[e.RowIndex].Selected = true;

            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }

            if (e.ColumnIndex == 0)
            {
                CurrentRow.Name = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                //MessageBox.Show(CurrentRow.Name);
                CurrentRow.Type = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString();
                //MessageBox.Show(CurrentRow.Type);
                CurrentRow.Data = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value.ToString();
                //MessageBox.Show(CurrentRow.Data);
            }

        }

        
    }

    /// <summary>
    /// RowCantainer is an object that contains the data for any given row in the datagridview
    /// </summary>
    
    public class RowContainer
    {
        public string Name;
        public string Type;
        public string Data; 
    };

}
