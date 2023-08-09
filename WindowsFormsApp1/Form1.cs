using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Crypto;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            double size;
            OpenFileDialog openFile= new OpenFileDialog();
            openFile.Multiselect = false;
            openFile.ShowDialog();
            string path = openFile.FileName;
            if (!string.IsNullOrEmpty(path))
            {
                for (int i = 0; i < path.Length; i++)
                {
                    if (Convert.ToString(path[i]) == "\\")
                    {
                        path.Insert(i, "\\");
                    }
                }
                size = new FileInfo(path).Length * 0.000001;
                MessageBox.Show($"{size} Mega Byte", "Size");
                textBox1.Text = path;
            }
        }

        private async void Encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == string.Empty)
                {
                    throw new Exception("The Text Input Is Empty");
                }
                else if (textBox2.Text.Length < 5)
                {
                    throw new Exception("The Password Must Be Bigger than 5 letters and digits");
                }
                
                var task1= Task.Run(() =>
                {
                    textBox1.Enabled = false; 
                    textBox2.Enabled=false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    Warriox_crypto.Encrypt(textBox1.Text, textBox1.Text + ".Black", textBox2.Text);
                }).ContinueWith((task) =>
                {
                    MessageBox.Show("Encrypted", "Enc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                });


                await Task.WhenAll(task1);

                label1.Text = "Success";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private async void Decrypt_Click(object sender, EventArgs e)
        {

            try
            {
                if (textBox1.Text == string.Empty)
                {
                    throw new Exception("The Text Input Is Empty");
                }
                else if (textBox2.Text.Length < 5)
                {
                    throw new Exception("The Password Must Be Bigger than 5 letters and digits");
                }
                else
                {
                    string inputFile = textBox1.Text;

                    string outputFile = inputFile.Remove(inputFile.Length-6);
                    await Task.Run(() =>
                    {
                        Warriox_crypto.Decrypt(inputFile, outputFile, textBox2.Text);
                    }).ContinueWith((task) =>
                    {
                        MessageBox.Show("Decrypted", "Dec", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                    label1.Text = "Success";
                }
                
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower() =="padding is invalid and cannot be removed.")
                {
                    MessageBox.Show("Password is not correct", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex.Message.ToLower() == "the input data is not a complete block.")
                {
                    MessageBox.Show("The File Is Not Encrypted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void About_Click(object sender, EventArgs e)
        {
            string url = "https://warriorx0.bsite.net";
            var process = new ProcessStartInfo { UseShellExecute=true,FileName=url};
            Process.Start(process);
        }
    }
}
