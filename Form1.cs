using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public string save_path=System.Windows.Forms.Application.StartupPath+"\\ef";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "保存目录："+ save_path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "请选择文件夹";
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowserDialog1.ShowNewFolderButton = true; 
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                save_path = folderBrowserDialog1.SelectedPath; 
                label1.Text ="保存目录：" +save_path;
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            foreach(DataRowView item in checkedListBox1.CheckedItems)
            {
                string tbname = item.Row[0].ToString();
                EntityHelper.EntityHelper.GenerateEF(save_path, tbname);
                 
            }
            MessageBox.Show("生成完成");


        }

        private void button1_Click(object sender, EventArgs e)
        {

            DataSet ds= EntityHelper.EntityHelper.GetDataSet(@"Select Name FROM SysObjects Where XType='U'");

            checkedListBox1.DataSource = ds.Tables[0];
            checkedListBox1.ValueMember = "Name";
            checkedListBox1.DisplayMember = "Name";

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
