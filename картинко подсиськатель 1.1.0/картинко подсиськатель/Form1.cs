using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace картинко_подсиськатель
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;

            this.KeyDown += Form1_KeyDown;

            dawnload_image();
            download_checked();
        }




        //вставка ctrl v
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, что нажата комбинация Ctrl+V
            if ((e.Control && e.KeyCode == Keys.V) && Clipboard.ContainsImage())
            {
                //pictureBox1.Image = Clipboard.GetImage();
                Image rh = Clipboard.GetImage();
                pictureBox1.Image = rh;
                rh.Save(@"C:\PictureLook\temp");
            }
        }







        //вставка перетаскиванием
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            // Получаем список файлов, которые были перетащены
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string file = files[0]; // Берем первый файл (в случае перетаскивания нескольких)
            pictureBox1.Load(file); // Загружаем файл в PictureBox
            make_backup(file);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = colorDialog1.Color;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                Image rh = Image.FromFile(openFileDialog1.FileName);
                rh.Save(@"C:\PictureLook\temp");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        bool turn = true;
        private void button4_Click(object sender, EventArgs e)
        {
            if (turn) { this.TopMost = true; turn = false;  button4.BackColor = Color.Red; }
            else { this.TopMost = false; turn = true; button4.BackColor = Color.FromArgb(192, 255, 192); }
            
        }


        private void make_backup(string sourceFilePath) 
        {
                // Указываем новое имя файла "temp" и расширение исходного файла
                string fileExtension = Path.GetExtension(sourceFilePath);  // Получаем расширение исходного файла

                // Копируем файл в папку назначения
                File.Copy(sourceFilePath, @"C:\PictureLook", true); // true — перезаписать файл, если он уже существует
        }


        private void dawnload_image()
        {
            if (!Directory.Exists(@"C:\PictureLook")) { Directory.CreateDirectory(@"C:\PictureLook"); }


            if (File.Exists(@"C:\PictureLook\temp"))
            {
                // Если файл существует, загружаем изображение
                Image rh = Image.FromFile(@"C:\PictureLook\temp");
                rh.Save(@"C:\PictureLook\temp_now");
                rh.Dispose();
                pictureBox1.Load(@"C:\PictureLook\temp_now");
            }
        }

        private void download_checked()
        {
            if (File.Exists(@"C:\PictureLook\checked"))
            {
                // Читаем состояние из файла и преобразуем в bool
                checkBox1.Checked = bool.Parse(File.ReadAllText(@"C:\PictureLook\checked"));
            }
        }

        private void Backup_checkbox1()
        {
            File.WriteAllText(@"C:\PictureLook\checked", checkBox1.Checked.ToString());
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            pictureBox1.Load(@"C:\PictureLook\temp");
            File.Delete(@"C:\PictureLook\temp_now");
            Backup_checkbox1();
        }
    }
}
