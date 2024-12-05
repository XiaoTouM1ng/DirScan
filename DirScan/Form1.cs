using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace DirScan
{
    public partial class Form1 : Form
    {

        string dir = "";
        public static Form1 form;
        Scan tScan;
        public Form1()
        {
            InitializeComponent();
        }
        public static void threadProc(Scan scan)
        {

            scan.actionAsync();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string url = urlBox.Text.Trim(); //获取要扫描的地址
            if (url.Length == 0) {

                MessageBox.Show("请输入Url后再次尝试!");
                return;
            
            }

            if (url.StartsWith("https://") == false && url.StartsWith("http://") == false) {

                MessageBox.Show("请输入正确的Url后再次尝试!");
                return;

            }

            if (url.EndsWith("/") == false) {

                url = url + "/";
            
            }
            
            if (button1.Text == "开始") {

                if (listBox2.Items.Count == 0) {
                    MessageBox.Show("请添加目录后尝试");
                    return;
                }
                dirList.Items.Clear();
                tScan = new Scan(url, comboBox1.Text, codeBox.Text,cookieBox.Text,agentBox.Text,refBox.Text);
                foreach (var item in listBox2.Items) {
                    tScan.addFile(dir + item.ToString().Trim());
                }
                progressBar1.Value = 0;
                for (int i = 0; i < int.Parse(threadBox.Text.Trim()); i++) {
                    Task.Run(()=> threadProc(tScan));
                }
                button1.Text = "停止";

            }
            else
            {
                tScan.stop();
                button1.Text = "开始";
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            dir = currentDirectory + "\\Dir\\";

            if (Directory.Exists(dir) == false) { 
                Directory.CreateDirectory(dir);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            foreach (FileInfo file in dirInfo.GetFiles()) {

                if (File.Exists(file.FullName) && file.Extension == ".txt") {
                    listBox1.Items.Add(file.Name);
                }
                
            }

            form = this;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                return;
            }
            if (listBox1.SelectedIndex == -1) {

                return;

            }

            listBox2.Items.Add(listBox1.SelectedItem.ToString());
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count == 0)
            {
                return;
            }
            if (listBox2.SelectedIndex == -1)
            {
                return;
            }

            listBox1.Items.Add(listBox2.SelectedItem.ToString());
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                return;
            }
            if (listBox1.SelectedIndex == -1)
            {

                return;

            }

            listBox2.Items.Add(listBox1.SelectedItem.ToString());
            listBox1.Items.Remove(listBox1.SelectedItem);

        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.Items.Count == 0)
            {
                return;
            }
            if (listBox2.SelectedIndex == -1)
            {
                return;
            }

            listBox1.Items.Add(listBox2.SelectedItem.ToString());
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        public void addDirList(string url,string title,string code) {
            int count = dirList.Items.Count;
            string[] item = { count.ToString(),url, title, code };
            dirList.Items.Add(new ListViewItem(item));
        
        }

        public void addMsg(string msg) {

            DateTime now = DateTime.Now;
            MsgBox.Items.Add(now.ToString()+"  "+msg);

        }

        private void dirList_DoubleClick(object sender, EventArgs e)
        {
            if (dirList.Items.Count == 0)
            {
                return;
            }

            if (dirList.SelectedItems.Count == 0) {

                return;
            
            }
            foreach (ListViewItem item in dirList.SelectedItems) {

                Process.Start(item.SubItems[1].Text);
            
            }
            



        }
    }
}
