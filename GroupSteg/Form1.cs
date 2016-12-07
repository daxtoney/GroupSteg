using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Forms.PictureBox;

namespace GroupSteg
{
    public partial class Form1 : Form
    {
        private Bitmap MyImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                ShowMyImage(openFileDialog1.FileName, 522, 262);
                sr.Close();
            }
        }

        public void ShowMyImage(String fileToDisplay, int xSize, int ySize)
        {
            // Sets up an image object to be displayed.
            if (MyImage != null)
            {
                MyImage.Dispose();
            }

            // Stretches the image to fit the pictureBox.
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            MyImage = new Bitmap(fileToDisplay);
            pictureBox1.ClientSize = new Size(xSize, ySize);
            pictureBox1.Image = (Image)MyImage;
        }

        private void Read_Click(object sender, EventArgs e)
        {

        }

        private void Write_Click(object sender, EventArgs e)
        {

        }

    }
    /* 
     * I'm just making stuff up now... 
     * Trying to copy everything over and 
     * convert it to C#
     *      
    public class SteganographyCodex : Form
    {
        private byte rawBytes;
        int width;
        int height;

        public SteganographyCodex(string filename)
        {
            rawBytes = BMP_Handler.loadBMP(filename, width, height);
        }

        private string getMessage(){
            const byte magicNumber[] = 316;
	        const byte mask = 0x1;
	        uint currCharIndex = 0;	//"iterator"
	        byte constructChar;
	
	        //read enough to check for magic number
	        byte[] buffer;
	        for(int i = 0; i < 3; i++) {
		        constructChar = 0;
		        for(int j = 0; j < 8; j++) {
			        //to read a bit use and, to write a bit use or
			        constructChar |= (rawBytes[currCharIndex++] & mask) << j;
		        }
		        buffer[i] = constructChar;
	        }

	        if(buffer[0] != magicNumber[0] || buffer[1] != magicNumber[1] || buffer[2] != magicNumber[2]) {
		        return "";
	        }

	        ostringstream out;

	        do {
		        constructChar = 0;
		        for(int i = 0; i < 8; i++) {
			        constructChar |= (rawBytes[currCharIndex++] & mask) << i;
		        }
		        out << constructChar;
	        } while(constructChar != '\0');

	        return out.str();
        }
    }

    public class BMP_Handler : Form
    {
        public BMP_Handler() { }

        public void loadBMP(string filename, int width, int height)
        {

        }
    }
    */
}
