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
    public struct _Win3xBitmapHeader
    {
	    // size = 20 + 16 + 4 = 40
        public uint Size;            /* Size of this header in bytes */
        public Int32 Width;           /* Image width in pixels */
        public Int32 Height;          /* Image height in pixels */
        public Int16 Planes;          /* Number of color planes */
        public Int16 BitsPerPixel;    /* Number of bits per pixel */
	    /* Fields added for Windows 3.x follow this line */
        public uint Compression;     /* Compression methods used */
        public uint SizeOfBitmap;    /* Size of bitmap in bytes */
        public Int32 HorzResolution;  /* Horizontal resolution in pixels per meter */
        public Int32 VertResolution;  /* Vertical resolution in pixels per meter */
        public uint ColorsUsed;      /* Number of colors in the image */
        public uint ColorsImportant; /* Minimum number of important colors */
    }

    public struct tagBITMAPFILEHEADER 
    {
        public char[] type;// = new char[2];
        public Int32 Size;
        public Int16 Reserved1;
        public Int16 Reserved2;
        public Int32 OffBits;
        // size = 2 4 2 2 4 = 14
    }

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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                // Save file here
                sr.Close();
            }
        }

    }
    /* 
     * I'm just making stuff up now... 
     * Trying to copy everything over and 
     * convert it to C#
     */
    public class SteganographyCodex : Form
    {
        private byte[] rawBytes;
        int width;
        int height;

        public SteganographyCodex(string filename)
        {
            rawBytes = BMP_Handler.loadBMP(filename, width, height);
        }

        // Almost converted to C# entirely 
        // All except for constructChar
        private string getMessage(){
            var magicNumber = new byte[] { 0x3, 0x1, 0x6 };
            //const byte magicNumber[] = 316;
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

            // Initialize the streams 
            // Create the stream
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            // Create the mechanism to write to the stream
            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
            writer.Flush();
            stream.Position = 0; 

	        //ostringstream out;

	        do {
		        constructChar = 0;
		        for(int i = 0; i < 8; i++) {
			        constructChar |= (rawBytes[currCharIndex++] & mask) << i;
		        }
                writer.Write(constructChar);
		        //out << constructChar;
	        } while(constructChar != '\0');

            return Encoding.ASCII.GetString(stream.ToArray());
        }

        private void setMessage(string message) {
	        //const unsigned char magicNumber[4] = "316";
            var magicNumber = new byte[] { 0x3, 0x1, 0x6 };
	        const byte mask = 0x1;
	        const byte stripMask = 0xfe;	//only the last bit is a 0
	        uint currCharIndex = 0;	//"iterator"

	        //write the magic number
	        for(int i = 0; i < 3; i++) {
		        for(int j = 0; j < 8; j++) {
			        //strip the last bit of the current byte
			        rawBytes[currCharIndex] &= stripMask;

			        //to read a bit use and, to write a bit use or
			        rawBytes[currCharIndex] |= (magicNumber[i] & (mask << j)) ? 1 : 0;

			        currCharIndex++;
		        }
	        }

	        //write the rest of it, make sure to null-terminate
	        for(int i = 0; i < message.Length; i++) {
		        for(int j = 0; j < 8; j++) {
			        //strip the last bit of the current byte
			        rawBytes[currCharIndex] &= stripMask;

			        //to read a bit use and, to write a bit use or
			        rawBytes[currCharIndex] |= (message[i] & (mask << j)) ? 1 : 0;

			        currCharIndex++;
		        }
	        }

	        for(int j = 0; j < 8; j++) {
		        //strip the last bit of the current byte 8 times to create the terminal null
		        rawBytes[currCharIndex] &= stripMask;
	
		        currCharIndex++;
	        }
        }

        private void writeImage(string filename) {
	        BMP_Handler.saveBMP(filename, rawBytes, width, height);
        }
    }

    public class BMP_Handler : Form
    {
        public BMP_Handler() { }

        public static byte[] loadBMP(string filename, int width, int height)
        {
            byte[] ret = new byte[10];
            ret[0] = 0x2;
            return ret;
        }

        public static void saveBMP(string file, byte[] imageData, int width, int height)
        {
            int padSize = 4-((width*3)%4);
	        if(padSize==4)
            {
		        padSize	= 0;
            }

	        int size = 3*width*height;

	        //Create the headers and set the values appropriately 
	        tagBITMAPFILEHEADER fileHead;
	        fileHead.type[0] = 'B';
	        fileHead.type[1] = 'M';
	        fileHead.Size = 54 + size + (padSize*height);
	        fileHead.Reserved1 = 0;	
	        fileHead.Reserved2 = 0;
	        fileHead.OffBits = 54;

	        _Win3xBitmapHeader fileInfo;
	        fileInfo.Size = 40;
	        fileInfo.Width = width;
	        fileInfo.Height = height;
	        fileInfo.Planes = 1;
	        fileInfo.BitsPerPixel = 24;
	        fileInfo.Compression = 0;
	        fileInfo.SizeOfBitmap = 0;
	        fileInfo.HorzResolution = 0x00000b13;
	        fileInfo.VertResolution = 0x00000b13;
	        fileInfo.ColorsUsed = 0;
	        fileInfo.ColorsImportant = 0;

	        // Transfer the image to a new array so the values can be swapped
	        byte[] transferArr = new byte [size];
	        for(int i = 0; i<size; i++){
		        transferArr[i] = imageData[i];
	        }

	        // Swap the R and B values
	        byte[] swappedArr = new byte [size];
	        for(int i = 0; i<size;i+=3){
		        swappedArr[i] = transferArr[i+2];
		        swappedArr[i+1] = transferArr[i+1];
		        swappedArr[i+2] = transferArr[i];
	        }

	        // Set the padding array up
	        byte[] padArr = new byte[padSize];
	        for(int i = 0; i<padSize; i++){
		        padArr[i] = 0;
	        }

            ////////////////////////////////////
            // Converted to C# up to here 
            ////////////////////////////////////

	        // Output the headers to the new bitmap file
	        std::ofstream fout(file, std::ios::out | std::ios::binary);
	        if(fout.fail())
	        {
		        exit(1);
	        }
	        fout.seekp(0);
	        fout.write(reinterpret_cast<char*>(&fileHead), 14);
	        fout.write(reinterpret_cast<char*>(&fileInfo), 40);
	        // Output the image data to the file
	        for(int i = 0; i<height; i++){
		        fout.write(reinterpret_cast<char*>(swappedArr+width*3*i),width*3);
		        fout.write(reinterpret_cast<char*>(padArr),padSize);
	        }

	        // Clean up
	        fout.close();
        }
    }
}
