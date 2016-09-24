// ThotNot
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleBirmap
{
    public partial class Form1 : Form
    {
        #region Properties
        private Bitmap picCell;
        private int cellSize, cellNumbers = 3, sizeviewPic = 190, xp = 0, yp = 12;
        private int k = 0, p, sumCell, g = 0, count = 0, counted, max;
        Panel dynamicPanel;
        Label lblPhut, lblGiay, lblCount;
        PictureBox[] pic;
        PictureBox viewPic;
        #endregion Properties
        public Form1()
        {
            InitializeComponent();
            show();
        }

        string filename = Application.StartupPath + @"/ss.bmp";
        #region Initialize
        private void show()
        {
            sumCell = cellNumbers * cellNumbers - 1;
            this.Controls.Remove(dynamicPanel);
            this.Controls.Remove(lblPhut);
            this.Controls.Remove(lblGiay);
            this.Controls.Remove(lblCount);
            this.Controls.Remove(viewPic);
            btnOneMove.Enabled = false;
            timer1.Stop();
            cellSize = 180;
            if (cellNumbers > 3)
                cellSize = 120;
            if (cellNumbers > 5)
                cellSize = 70;
            //
            // Add panel contain PictureBox
            //
            dynamicPanel = new Panel();
            dynamicPanel.Location = new System.Drawing.Point(xp, yp);
            dynamicPanel.Name = "Panel1";
            dynamicPanel.Size = new System.Drawing.Size(
                cellSize * cellNumbers + (cellNumbers - 1), cellSize * cellNumbers + cellNumbers - 1);
            dynamicPanel.BackColor = Color.Peru;
            dynamicPanel.BorderStyle = BorderStyle.Fixed3D;
            //
            // Add  lblPhut
            //
            lblPhut = new Label();
            lblPhut.Size = new System.Drawing.Size(70, 40);
            lblPhut.Location = new Point(cellNumbers * cellSize + 20, 100);
            lblPhut.Text = "03";
            lblPhut.ForeColor = Color.Red;
            lblPhut.Font = new System.Drawing.Font("Time New Roman", 30);
            Controls.Add(lblPhut);
            //
            // Add lblgiay
            //
            lblGiay = new Label();
            lblGiay.Size = new System.Drawing.Size(70, 40);
            lblGiay.Location = new Point(lblPhut.Location.X + 60, 100);
            lblGiay.Text = "00";
            lblGiay.ForeColor = Color.Red;
            lblGiay.Font = new System.Drawing.Font("Time New Roman", 30);
            Controls.Add(lblGiay);
            //
            // Add lblCount
            //
            count = 0;
            lblCount = new Label();
            lblCount.Size = new System.Drawing.Size(50, 40);
            lblCount.Location = new Point(lblPhut.Location.X, 150);
            lblCount.Text = "00";
            lblCount.ForeColor = Color.Blue;
            lblCount.Font = new System.Drawing.Font("Time New Roman", 15);
            Controls.Add(lblCount);
            //
            // Add  lblMoved
            //
            lblMoved.Size = new System.Drawing.Size(50, 40);
            lblMoved.Location = new Point(lblGiay.Location.X, 150);
            lblMoved.ForeColor = Color.Blue;
            lblMoved.Text = counted.ToString();
            lblMoved.Font = new System.Drawing.Font("Time New Roman", 15);
            Controls.Add(lblMoved);
            //
            // Initiallize PicCell[]
            //
            pic = new PictureBox[cellNumbers * cellNumbers];
            for (int i = 0; i < cellNumbers; i++)
                for (int j = 0; j < cellNumbers; j++)
                {
                    int pos = cellNumbers * i + j;
                    pic[pos] = new PictureBox();
                    pic[pos].Location = new Point(j * (cellSize + 1), (cellSize + 1) * i);
                    pic[pos].Size = new Size(cellSize, cellSize);
                    dynamicPanel.Controls.Add(pic[pos]);
                }
            Controls.Add(dynamicPanel);
            Bitmap img;
            try
            {
                img = new Bitmap(filename);
                picCell = new Bitmap(img, cellSize * cellNumbers, cellSize * cellNumbers);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Em không mở file được rùi!!! ^-^\n" + ex.Message);
                return;
            }
            //
            // add image to cellpic
            //
            for (int i = 0; i < cellNumbers; i++)
                for (int j = 0; j < cellNumbers; j++)
                {
                    int pos = cellNumbers * i + j;
                    Rectangle rec = new Rectangle(j * cellSize, i * cellSize, cellSize, cellSize);
                    pic[pos].Image = null;
                    pic[pos].Tag = pos.ToString();
                    if (pos != cellNumbers * cellNumbers - 1)
                        pic[pos].Image = picCell.Clone(rec, PixelFormat.DontCare);
                }
            //
            // Add viewPic
            //
            viewPic = new PictureBox();
            viewPic.Location = new System.Drawing.Point(lblPhut.Location.X, dynamicPanel.Width + yp - sizeviewPic);
            viewPic.Name = "viewPic";
            viewPic.Size = new System.Drawing.Size(sizeviewPic, sizeviewPic);
            viewPic.BackColor = Color.Peru;
            viewPic.Image = picCell;
            viewPic.SizeMode = PictureBoxSizeMode.StretchImage;
            viewPic.BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(viewPic);
            //
            // reset size of form and btnStart, location of btnOneMove
            //
            this.Size = new Size(cellSize * cellNumbers + 250, dynamicPanel.Height + 50);
            btnStart.Location = new Point(lblPhut.Location.X, 50);
            this.StartPosition = FormStartPosition.CenterScreen;
            btnOneMove.Location = new System.Drawing.Point(lblPhut.Location.X + 110, 50);
            btnStart.Enabled = true;
        }
        private void randomPicture()
        {

            // Bitmap temp;
            Random rnd = new Random();
            for (int i = 0; i < sumCell; i++)
            {
                int a = rnd.Next(sumCell);
                int b = rnd.Next(sumCell);
                if (a != b)
                {
                    try
                    {
                        HoanVis(pic[a], pic[b]);
                    }
                    catch { }
                }
            }
        }
        #endregion Initialize
        #region Event click
        private void btnStart_Click_1(object sender, EventArgs e)
        {
            timer1.Start();
            btnOneMove.Enabled = true;
            btnStart.Enabled = false;
            randomPicture();
            this.Refresh();
            for (int i = 0; i <= sumCell; i++)
                pic[i].Click += Play_Click;
        }
        private void Play_Click(object sender, EventArgs e)
        {
            PictureBox picA = (PictureBox)sender;
            for (int i = 0; i <= sumCell; i++)
            {
                //
            //    int x = picA.Location.X;
            //    int k = x % cellSize;
            //    if (pic[i].Image == null && (i % 3 - k) == 1 || (k - i % 3) == 1 || (i - cellNumbers) == 1
            //        ||(i  + cellNumbers ) == 1)
            //    {
            //        {
            //            HoanVi(pic[i], picA);
            //            lblCount.Text = (++count).ToString();
            //        }
            //    }
            //}
                if (pic[i].Location.X == picA.Location.X && pic[i].Image == null)
                    if ((pic[i].Location.Y - picA.Location.Y) == cellSize + 1 ||
                        (picA.Location.Y - pic[i].Location.Y) == cellSize + 1)
                    {
                        HoanVi(pic[i], picA);
                        lblCount.Text = (++count).ToString();
                    }
                //
                if (pic[i].Location.Y == picA.Location.Y && pic[i].Image == null)
                    if ((pic[i].Location.X - picA.Location.X) == cellSize + 1 ||
                        (picA.Location.X - pic[i].Location.X) == cellSize + 1)
                    {
                        HoanVi(pic[i], picA);
                        lblCount.Text = (++count).ToString();
                    }
            }
            Check();
        }
        private void btnOneMove_Click(object sender, EventArgs e)
        {
            int t = sumCell;
            while (t >= 0)
            {
                for (int i = 0; i <= sumCell; i++)
                {
                    int p = int.Parse(pic[t].Tag.ToString());
                    if (t != p)
                        HoanVis(pic[t], pic[p]);
                }
                t--;
            }
            t = sumCell;
            HoanVi(pic[t], pic[t - 1]);
            btnOneMove.Enabled = false;
        }
        #endregion Event click
        #region Swap
        void HoanVi(PictureBox a, PictureBox b)
        {
            string tem = a.Tag.ToString();

            a.Image = b.Image;
            a.Tag = b.Tag;

            b.Image = null;
            b.Tag = tem;
        }
        void HoanVis(PictureBox a, PictureBox b)
        {
            Bitmap temp;
            temp = (Bitmap)a.Image;
            temp.Tag = a.Tag;

            a.Image = b.Image;
            a.Tag = b.Tag;

            b.Image = temp;
            b.Tag = temp.Tag;
        }
        #endregion  
        #region Check completed
        void Check()
        {
            int d = 0;
            max = sumCell;
            while (max >= 0)
            {
                if (int.Parse(pic[max].Tag.ToString()) == max)
                    d++;
                max--;
            }
            if (d == sumCell + 1)
            {
                timer1.Stop();
                MessageBox.Show("Chúc mừng, Chúc mừng. Bạn đã xếp xong. ^-^\nClick Start để chơi tiếp nha! hihi");
                btnStart.Enabled = true;
                if (int.Parse(lblMoved.Text) == 0 || int.Parse(lblMoved.Text) > int.Parse(lblCount.Text))
                    counted = int.Parse(lblCount.Text);
                lblCount.Text = "00";
                show();
                if (cellNumbers == 5)
                    lblPhut.Text = "10";
                if (cellNumbers == 8)
                    lblPhut.Text = "15";
            }
        }
        #endregion Check completed
        #region Menu
        // biginner
        private void beginerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cellNumbers = 3;
            show();
            lblMoved.Text = "0";
        }
        // intermadiate
        private void intermediateToolStripMenuItem_Click(object sender, EventArgs e)
        {

            cellNumbers = 5;
            show();
            lblPhut.Text = "10";
            lblGiay.Text = "00";
            lblMoved.Text = "0";
        }
        // Advance
        private void advanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cellNumbers = 8;
            show();
            lblPhut.Text = "15";
            lblGiay.Text = "00";
            lblMoved.Text = "0";
        }
        // Instruction
        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn cứ xếp hình đi, đúng thì mình sẽ nói!@@ hề hề");
        }
        // exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion Menu
        #region Timer tick
        private void timer1_Tick(object sender, EventArgs e)
        {
            p = int.Parse(lblPhut.Text);
            g = int.Parse(lblGiay.Text);
            if (int.Parse(lblGiay.Text) == 0)
            {
                k = 0;
                lblGiay.Text = "59";
                lblPhut.Text = (p - 1).ToString();
            }
            k++;
            lblGiay.Text = (60 - k).ToString();
            if (p == 0 && g == 0)
            {
                btnStart.Enabled = true;
                this.Controls.Remove(lblGiay);
                this.Controls.Remove(lblPhut);
                MessageBox.Show("^_^ Xin lỗi bb!!! Hết giờ mất rùi!\nClick Start để chơi lại nhé! :)", "Thua rùi!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                show();
                timer1.Stop();
            }
        }
        #endregion Timer tick

    }
}


