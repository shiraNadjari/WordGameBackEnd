using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using COMMON;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string path;

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            path = saveFileDialog1.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;
            List<COMCategory> cats = new List<COMCategory>();
            if (BLLcategory.GetCategories() != null)
            {
                cats = BLLcategory.GetCategories();
                comboBox1.DataSource = cats;
                comboBox1.DisplayMember = "CategoryName";
                comboBox1.ValueMember = "CategoryId";
            }
            List<COMuser> users = new List<COMuser>();
            if (BLLuser.GetUsers() != null)
            {
                users = BLLuser.GetUsers();
                comboBox3.DataSource = users;
                comboBox3.DisplayMember = "Email";
                comboBox3.ValueMember = "UserId";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox2.Text==textBox4.Text)
            {
                COMuser user = new COMuser();
                user.CategoryName = textBox3.Text;
                user.Email = textBox1.Text;
                user.Password = textBox2.Text;
                user.ImageURL = @"C:\Users\ריקי\Desktop\pictures\stationary\1.jpg";
                try
                {
                    BLLuser.AddUser(user);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("error: add user didnt success: "+ex);
                }
            }
            else
            {
                MessageBox.Show("verify password isnt like password. try again.");
            }
            groupBox1.Visible = false;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            COMCategory cat = new COMCategory();
            cat.CategoryName = textBox6.Text;
            cat.ImageURL = path;
            BLLcategory.AddCategory(cat, categoriesCounter);
            int catId = BLLcategory.GetCategoryIdByCategoryName(cat.CategoryName);
            try
            {
                BLLcategory.UpdateURL(catId, BLLgoogleVision.Storage(catId, cat.ImageURL, categoriesCounter, true));
            }
            catch (Exception)
            {
                throw;
            }
            groupBox2.Visible = false;
            textBox6.Text = string.Empty;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            COMimage img = new COMimage();
            img.UserId =Convert.ToInt32(comboBox3.SelectedValue);
            img.CategoryID = Convert.ToInt32(comboBox1.SelectedValue);
            img.URL = path;
            label10.Text = "";
            foreach (string objName in BLLimage.AddImage(img,categoriesCounter,voicesCounter))
            {
                label10.Text += objName + "\n";
            }
            groupBox3.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            path = saveFileDialog1.FileName;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            textBox6.Text = string.Empty;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;

        }
        public static Dictionary<string, int> categoriesCounter = new Dictionary<string, int>();
        public static Dictionary<string, int> voicesCounter = new Dictionary<string, int>();


        private void Form1_Load(object sender, EventArgs e)
        {
            int x = 0;
            foreach (COMCategory cat in BLLcategory.GetCategories())
            {
                x = BLLimage.Getimages().FindAll(img => img.CategoryID == cat.CategoryId).Count();
                categoriesCounter.Add(cat.CategoryName, x);
            }
            foreach (COMCategory cat in BLLcategory.GetCategories())
            {
                List<COMimageObject> objects = new List<COMimageObject>();
                //List <COMimage> images= BLLimage.Getimages().FindAll(img => img.CategoryID == cat.CategoryId);
                //foreach (COMimage img in images)
                //{
                //    foreach (COMimageObject obj in BLLobject.GetObjects().FindAll(obj => obj.ImageID == img.ImageID))
                //    {
                //        objects.Add(obj);
                //    }
                //}
                BLLimage.Getimages().FindAll(img => img.CategoryID == cat.CategoryId).ForEach(img => objects.AddRange(BLLobject.GetObjects().FindAll(obj => obj.ImageID == img.ImageID)));
                x = objects.Count;
                voicesCounter.Add(cat.CategoryName, x);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            BLLtextToSpeach.UpdateUrl(voicesCounter);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //BLLgoogleVision.AddUserImage();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            BLLgoogleVision.createBucket();
        }
    }
}
