using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Goruntuisleme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string fileWay;//Dosya yolunu tutar.
        //Picturebox içinde resim olup olmadığını kontrol eder.Yoksa uyarı döndürür.
        private void errorCheck()
        {
        if(pictureBox1.Image==null)
            {
                MessageBox.Show("Lütfen önce fotoğraf seçiniz.", "Hata");
            }
        }
        private void imageInfo()
        {
            FileInfo fileInfo = new FileInfo(fileWay);
            MessageBox.Show("Fotoğrafın adı:"+ fileInfo.Name.Replace(fileInfo.Extension, "") + 
                "\nFotoğrafın Boyutu:"+ (fileInfo.Length / 1024.0).ToString("0.0") + " KB"
                +"\nFotoğrafın Tarihi:"+ fileInfo.CreationTime.ToString("dddd MMMM dd, yyyy")
                + "\nFotoğrafın Türü:" + fileInfo.Extension, "Hata");
        }

        private Bitmap grayScale(Bitmap bmp)
        {
            errorCheck();
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    int grayValue = (bmp.GetPixel(j, i).R + bmp.GetPixel(j, i).G + bmp.GetPixel(j, i).B) / 3;
                    //Renkli görüntüyü griye çevirdik.
                    Color color;
                    color = Color.FromArgb(grayValue, grayValue, grayValue);
                    bmp.SetPixel(j, i, color);
                }
            }
            return bmp;
        }
        //Görüntüyü sağa döndürmeye yarayan fonksiyon
        public Bitmap turnRight(Bitmap bmp)
        {
            Bitmap b = new Bitmap(bmp.Height, bmp.Width);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    b.SetPixel(b.Width - j - 1, i, bmp.GetPixel(i, j));
                }
            }
            return b;
        }
        //Görüntüyü sola döndürmeye yarayan fonksiyon
        public Bitmap turnLeft(Bitmap bmp)
        {
            Bitmap b = new Bitmap(bmp.Height, bmp.Width);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    b.SetPixel(j, b.Height - 1 - i, bmp.GetPixel(i, j));
                }
            }
            return b;
        }
        //Fotoğrafın negatifini alır.
        private Bitmap negative(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color p = bmp.GetPixel(x, y);
                    //Pixellerin renk değerlerini değişkene atama.
                    int a = p.A; 
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    //Negatif değer işlemleri
                    r = 255 - r;
                    g = 255 - g;
                    b = 255 - b;
                    //pixellere yeni argb değerlerini koy
                    bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            return bmp;
        }
        //Fotoğrafa kırmızı filtreleme yapar.
        private Bitmap redFilter(Bitmap bmp)
        {
            bmp = new Bitmap(fileWay);

            //Orjinal resmi image1'e ata.
            pictureBox1.Image = Image.FromFile(fileWay);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmp.SetPixel(i, j, Color.FromArgb(bmp.GetPixel(i, j).R, 0, 0));
                }
            }
            return bmp;

        }
        //Fotoğrafa yeşil filtreleme yapar.
        private Bitmap greenFilter(Bitmap bmp)
        {

             bmp = new Bitmap(fileWay);
           
         
            //Orjinal resmi image1'e ata.
            pictureBox1.Image = Image.FromFile(fileWay);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmp.SetPixel(i, j, Color.FromArgb(0, bmp.GetPixel(i, j).G, 0));
                }
            }
         
            return bmp;
        }
        //Fotoğrafa mavi filtreleme yapar.
        private Bitmap blueFilter(Bitmap bmp)
        {
           bmp = new Bitmap(fileWay);

            //Orjinal resmi image1'e ata.
           pictureBox1.Image = Image.FromFile(fileWay);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmp.SetPixel(i, j, Color.FromArgb(0, 0, bmp.GetPixel(i, j).B));
                }
            }
            return bmp;
        }
        // //Fotoğrafa aynalama yapar.
        private Bitmap Mirroring(Bitmap bmp)
        {
            Bitmap b = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    b.SetPixel(b.Width - i - 1, j, bmp.GetPixel(i, j));
                }
            }
            return b;
        }
        //Resmi ölçeklendirmemize yarar.
        public Bitmap Scaling(Bitmap bmp)
        {
            Bitmap myBitmap = new Bitmap(pictureBox1.Image);
            int originalWidth = myBitmap.Width;//Gerçek genişlik
            int originalHeight = myBitmap.Height;//gerçek yükseklik
            string newWidth = textBox1.Text;//Alınan genişlik   
            string newHeight = textBox2.Text;//Alınan yükseklik
            Bitmap formed = new Bitmap(myBitmap, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
            return formed;
        }
        //Fotoğrafı zoomlamak için
        Image Zoom(Image img, Size size)
        {
            Bitmap bmp = new Bitmap(img, img.Width + (img.Width * size.Width / 100), img.Height + (img.Height * size.Height / 100));
            return bmp;

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "Resim Dosyası |*.jpg;*.nef;*.png| Video|*.avi| Tüm Dosyalar |*.*";
            dosya.ShowDialog();
            fileWay = dosya.FileName;
            pictureBox1.ImageLocation = fileWay;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();//yeni bir kaydetme diyaloğu oluşturuyoruz.
            sfd.Filter = "jpeg dosyası(*.jpg)|*.jpg|Bitmap(*.bmp)|*.bmp";//.bmp veya .jpg olarak kayıt imkanı sağlıyoruz.
            sfd.Title = "Kayıt";//diyaloğumuzun başlığını belirliyoruz.
            sfd.FileName = "resim";//kaydedilen resmimizin adını 'resim' olarak belirliyoruz.
            DialogResult sonuç = sfd.ShowDialog();
            if (sonuç == DialogResult.OK)
            {
                pictureBox1.Image.Save(sfd.FileName);//Böylelikle resmi istediğimiz yere kaydediyoruz.
            }
        }
    
        //Tekrar Açma
        private void button3_Click(object sender, EventArgs e)
        {
            errorCheck();
            pictureBox1.ImageLocation = fileWay;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            errorCheck();
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap finalState = turnRight(image);
            pictureBox1.Image = finalState;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            errorCheck();
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap finalState = turnLeft(image);
            pictureBox1.Image = finalState;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            errorCheck();
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap finalState = grayScale(image);
            pictureBox1.Image = finalState;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            errorCheck();
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap finalState = negative(image);
            pictureBox1.Image = finalState;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            errorCheck();
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap finalState = Mirroring(image);
            pictureBox1.Image = finalState;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            errorCheck();
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap finalState = redFilter(image);
            pictureBox1.Image = finalState;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            errorCheck();
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap finalState = blueFilter(image);
            pictureBox1.Image = finalState;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            errorCheck();
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap finalState = greenFilter(image);
            pictureBox1.Image = finalState;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            errorCheck();
            Image img = pictureBox1.Image;
            Bitmap bmp = new Bitmap(img);
            int[] red = new int[256];
            int[] green = new int[256];
            int[] blue = new int[256];
            int[] colorBrightness = new int[256*3];
           // int[] gri = new int[256];

            for (int i = 0; i < bmp.Size.Height; i++)
                for (int j = 0; j < bmp.Size.Width; j++)
                {
                    Color renk = bmp.GetPixel(j, i);
                    red[renk.R]++;
                    green[renk.G]++;
                    blue[renk.B]++;
                    colorBrightness[(renk.R + renk.G + renk.B)]++;
                 ///   gri[(renk.R / 3) + (renk.G / 3) + (renk.B / 3)]++;
                }
          //Verileri grafiğe atama işlemi
            chart1.Series[0].Points.DataBindY(red);
            chart1.Series[1].Points.DataBindY(green);
           chart1.Series[2].Points.DataBindY(blue);
            //chart1.Series[3].Points.DataBindY(gri);
            chart2.Series[0].Points.DataBindY(colorBrightness);
        }
     
        private void button9_Click(object sender, EventArgs e)
        {
            errorCheck();
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap yeniHal = Scaling(image);
            pictureBox1.Image = yeniHal;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            errorCheck();
            label4.Text = trackBar1.Value.ToString();
            Bitmap myBitmap = new Bitmap(fileWay);
            pictureBox1.Image = Image.FromFile(fileWay);
            if (trackBar1.Value>0)
            {
                pictureBox1.Image = Zoom(myBitmap, new Size(trackBar1.Value, trackBar1.Value));
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            imageInfo();
        }
    }
}
