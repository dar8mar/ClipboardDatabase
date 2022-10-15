using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Nodes;
using System.Timers;
using System.Windows.Forms;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlTypes;
using System.Data;
using System.Text.Encodings.Web;
 
namespace CopyBase
{
    public partial class Form1 : Form
    {

        public Dictionary<string,int> ClipList = new Dictionary<string, int>();
        public int numberOfFires = 0;
        public string text1 = String.Empty;
        


        public Form1()
        {



            InitializeComponent();

            listBox1.DataSource = new BindingSource(ClipList, null);
            listBox1.DisplayMember = "Value";
            



        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label1.Text = text;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            text1 = listBox1.GetItemText(listBox1.SelectedItem);
            SetText(ClipList.Count.ToString());

        }

        private void button1_Click(object sender, EventArgs e)
        {





            if (!ClipList.ContainsKey(Clipboard.GetText(TextDataFormat.Text)))
            {
                ClipList.Add(Clipboard.GetText(),ClipList.Count);
            }

            listBox1.DataSource = null;
            listBox1.DataSource = new BindingSource(ClipList, null);
            listBox1.DisplayMember = "Key";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = listBox1.GetItemText(listBox1.SelectedItem);
            ClipList.Remove(text);
            listBox1.DataSource = null;
            listBox1.DataSource = new BindingSource(ClipList, null);
            listBox1.DisplayMember = "Key";
        }



      

        private void button3_Click(object sender, EventArgs e)
        {
            if (text1 != null)
            {
                Clipboard.SetText(text1);
            }
        }


        public void SaveFile()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            
            
            

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                   

                    myStream.Close();

                    File.WriteAllText(saveFileDialog1.FileName, JsonSerializer.Serialize(ClipList));

                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            SaveFile();


        }



        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Close();
        }


        public void OpenFileDialogForm()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Text Files",  
  
                CheckFileExists = true,  
                CheckPathExists = true,  
  
                DefaultExt = "txt",  
                Filter = "txt files (*.txt)|*.txt",  
                FilterIndex = 2,  
                RestoreDirectory = true,  
  
                ReadOnlyChecked = true,  
                ShowReadOnly = true
    
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ClipList = JsonSerializer.Deserialize<Dictionary<string, int>>(File.ReadAllText(openFileDialog1.FileName));
                
                listBox1.DataSource = null;
                listBox1.DataSource = new BindingSource(ClipList, null);
                listBox1.DisplayMember = "Key";
            }




        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialogForm();

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialogForm();
        }
    }
}