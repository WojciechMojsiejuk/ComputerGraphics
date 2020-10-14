using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics
{
    public static class StreamEOF
    {

        public static bool EOF(this BinaryReader binaryReader)
        {
            var bs = binaryReader.BaseStream;
            return (bs.Position == bs.Length);
        }

        /// <summary>
        /// Ommit comment line until new line character
        /// </summary>
        /// <exception cref="EndOfStreamException">
        /// 
        /// </exception>
        /// <param name="binaryReader"></param>

        public static bool In<T>(this T x, params T[] set)
        {
            return set.Contains(x);
        }

        public static BitmapImage ToBitmapImage(this WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }

    }

    public class PPMReader
    {
        const int MAX_FILE_SIZE = 99999;
        const int BUFFER_SIZE = 4096;
        const char LF = '\n';
        const char HASH = '#';

        enum Mode : int
        {
            ONEBYTE = 255,
            TWOBYTES = 65535,
        }

        Mode mode;

        public enum PPMFormat
        {
            P3,
            P6,
        }
        public char[] p3CharValues = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        BitmapImage bitmapImage;
        BinaryReader binaryReader;
        char[] fileIdentifier = new char[2];
        public PPMFormat format;
        StringBuilder maxValueString = new StringBuilder();
        StringBuilder widthString = new StringBuilder();
        StringBuilder heightString = new StringBuilder();
        int MaxValue { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        int X { get; set; } = 0;
        int Y { get; set; } = 0;
        bool LinearInterpolation { get; set; } = false;
        WriteableBitmap writeableBitmap;

        uint BufferIndex { get; set; } = 0;
        char[] Buffer;
        bool IncompleteReading { get; set; }
        bool IncompleteComment { get; set; }
        enum Color : int
        {
            R = 0,
            G = 1,
            B = 2,
        }
        Color currentColor = 0;

        StringBuilder RGBValueString = new StringBuilder();

        bool ColorReady { get; set; } = false;

        FileStream fs;

        List<byte> colorData = new List<byte>();

        public PPMReader(string filePath)
        {
            try
            {
                // Open the text file using a stream reader.
                FileStream fs = File.Open(filePath, FileMode.Open);

                //Initialize variables
                maxValueString.Capacity = 5;
                widthString.Capacity = 5;
                heightString.Capacity = 5;

                // Read the stream as a string, and write the string to the console.
                binaryReader = new BinaryReader(fs, new ASCIIEncoding());
                fileIdentifier[0] = binaryReader.ReadChar();
                fileIdentifier[1] = binaryReader.ReadChar();

                if (fileIdentifier[0].Equals('P') && fileIdentifier[1].Equals('3'))
                {
                    format = PPMFormat.P3;
                }
                else if (fileIdentifier[0].Equals('P') && fileIdentifier[1].Equals('6'))
                {
                    format = PPMFormat.P6;
                }
                else
                {
                    throw new InvalidDataException(String.Format("PPM file header should specify format. Expected P3 or P6, got {0}{1}", fileIdentifier[0], fileIdentifier[1]));
                }
                getProperty(widthString);
                Width = castPropertyToInt(widthString, 1, MAX_FILE_SIZE);
                getProperty(heightString);
                Height = castPropertyToInt(heightString, 1, MAX_FILE_SIZE);
                getProperty(maxValueString);
                MaxValue = castPropertyToInt(maxValueString, 1, (int)Mode.TWOBYTES);
                if (MaxValue > (int)Mode.ONEBYTE)
                {
                    mode = Mode.TWOBYTES;
                    if (MaxValue != (int)Mode.TWOBYTES)
                    {
                        LinearInterpolation = true;
                    }
                }
                else
                {
                    mode = Mode.ONEBYTE;
                    if (MaxValue != (int)Mode.ONEBYTE)
                    {
                        LinearInterpolation = true;
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                fs.Close();
            }
        }

        public BitmapImage readPPM3()
        {
            if(format == PPMFormat.P3)
            {

                writeableBitmap = new WriteableBitmap(
                           Width,
                           Height,
                           96,
                           96,
                           PixelFormats.Rgb24,
                           null);
            }
            else
            {

                writeableBitmap = new WriteableBitmap(
                           Width,
                           Height,
                           96,
                           96,
                           PixelFormats.Rgb48,
                           null);
            }

            while (!binaryReader.EOF())
            {
                bool isBufferFinished = false;
                int? RValue = null;
                int? GValue = null;
                int? BValue = null;
                BufferIndex = 0;
                // Read to buffer
                Buffer = binaryReader.ReadChars(BUFFER_SIZE);
                while (true)
                {
                    isBufferFinished = getPropertyFromBuffer(RGBValueString);
                    if (ColorReady)
                    {
                        if (currentColor == Color.R)
                            RValue = castPropertyToInt(RGBValueString, 0, (int)mode);
                        else if (currentColor == Color.G)
                            GValue = castPropertyToInt(RGBValueString, 0, (int)mode);
                        else if (currentColor == Color.B)
                            BValue = castPropertyToInt(RGBValueString, 0, (int)mode);

                        int nextColor = (int)(++currentColor) % 3;
                        currentColor = (Color)nextColor;

                        // clear StringBuilder
                        RGBValueString.Clear();
                        ColorReady = false;

                        if (RValue != null && GValue != null && BValue != null)
                        {

                            if (LinearInterpolation)
                            {
                                RValue = linearInterpolate((int)RValue);
                                GValue = linearInterpolate((int)GValue);
                                BValue = linearInterpolate((int)BValue);
                            }

                            if(mode == Mode.ONEBYTE)
                            {
                                colorData.Add(Convert.ToByte((int)RValue));
                                colorData.Add(Convert.ToByte((int)GValue));
                                colorData.Add(Convert.ToByte((int)BValue));
                            }
                            else
                            {
                                byte[] Rtemp = BitConverter.GetBytes((int)RValue);
                                byte[] Gtemp = BitConverter.GetBytes((int)GValue);
                                byte[] Btemp = BitConverter.GetBytes((int)BValue);

                                colorData.Add(Rtemp[0]);
                                colorData.Add(Rtemp[1]);
                                colorData.Add(Gtemp[0]);
                                colorData.Add(Gtemp[1]);
                                colorData.Add(Btemp[0]);
                                colorData.Add(Btemp[1]);
                            }

                           
                            System.Diagnostics.Debug.WriteLine(RValue);
                            System.Diagnostics.Debug.WriteLine(GValue);
                            System.Diagnostics.Debug.WriteLine(BValue);
                            System.Diagnostics.Debug.WriteLine("______________");


                            //DrawPixel((int)RValue, (int)GValue, (int)BValue);
                            ++X;
                            if (X == Width)
                            {
                                DrawLine();
                                ++Y;
                                X = 0;
                            }

                            RValue = null;
                            GValue = null;
                            BValue = null;
                        }
                    }

                    if (isBufferFinished)
                        break;
                }
            }
            binaryReader.Close();
            return writeableBitmap.ToBitmapImage();
        }


        public BitmapImage readPPM6()
        {

            writeableBitmap = new WriteableBitmap(
                       Width,
                       Height,
                       96,
                       96,
                       PixelFormats.Rgb24,
                       null);

            while (!binaryReader.EOF())
            {
                // Read to buffer
                byte[] byteBuffer = binaryReader.ReadBytes(writeableBitmap.BackBufferStride);
                colorData.AddRange(byteBuffer);
            }
            DrawPicture();
            binaryReader.Close();
            return writeableBitmap.ToBitmapImage();
        }

        public void getProperty(StringBuilder stringBuilder)
        {
            bool foundFirstChar = false;
            while (!binaryReader.EOF())
            {
                char propertyChar = binaryReader.ReadChar();
                if (propertyChar == '#')
                {
                    //Omit comment
                    while (true)
                    {
                        propertyChar = binaryReader.ReadChar();
                        if (propertyChar == LF)
                            break;
                    }
                    continue;

                }
                if ((propertyChar.In(p3CharValues) == false) && (foundFirstChar == false))
                    continue;
                else if ((propertyChar.In(p3CharValues) == false) && (foundFirstChar == true))
                    return;
                foundFirstChar = true;
                try
                {
                    stringBuilder.Append(propertyChar);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException("Image parameter too big");
                }
            }
            throw new EndOfStreamException("Could not read parameter from file");
        }

        int castPropertyToInt(StringBuilder stringBuilder, int allowedMin, int allowedMax)
        {
            int property = int.Parse(stringBuilder.ToString());
            if (property < allowedMin)
            {
                throw new InvalidDataException(String.Format("{0} is smaller than allowed min {1}", property, allowedMin));
            }
            if (property > allowedMax)
            {
                throw new InvalidDataException(String.Format("{0} is bigger than allowed max {1}", property, allowedMax));
            }
            return property;
        }

        int linearInterpolate(int value)
        {
            return (int)Math.Round((double)(value * (int)mode / MaxValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <returns>
        /// true if buffer reading is finished
        /// else false
        /// </returns>
        public bool getPropertyFromBuffer(StringBuilder stringBuilder)
        {
            bool foundFirstChar = false;
            while (BufferIndex != Buffer.Length)
            {
                char propertyChar = Buffer[BufferIndex];
                if (propertyChar == '#' || IncompleteComment)
                {
                    while (BufferIndex != Buffer.Length)
                    {
                        IncompleteComment = true;
                        propertyChar = Buffer[BufferIndex];
                        if (propertyChar == (char)0x0A)
                        {
                            IncompleteComment = false;
                            break;
                        }
                        BufferIndex++;
                    }
                    if (!IncompleteComment)
                        continue;
                    else
                        break;

                }
                if ((propertyChar.In(p3CharValues) == false) && (foundFirstChar == false))
                {
                    if (IncompleteReading)
                    {
                        //Last buffer did not finished at white space
                        ColorReady = true;
                        IncompleteReading = false;
                        BufferIndex++;
                        return false;
                    }
                    BufferIndex++;
                    continue;
                }
                else if ((propertyChar.In(p3CharValues) == false) && (foundFirstChar == true))
                {
                    ColorReady = true;
                    BufferIndex++;
                    return false;
                }
                IncompleteReading = false;
                foundFirstChar = true;
                try
                {
                    stringBuilder.Append(propertyChar);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException("Invalid RGB Value in file");
                }
                BufferIndex++;
            }
            if (foundFirstChar)
                IncompleteReading = true;
            return true;
        }

        [STAThread]
        void DrawPixel(int r, int g, int b)
        {
            try
            {
                // Reserve the back buffer for updates.
                writeableBitmap.Lock();

                unsafe
                {
                    // Get a pointer to the back buffer.
                    IntPtr pBackBuffer = writeableBitmap.BackBuffer;

                    // Find the address of the pixel to draw.
                    pBackBuffer += Y * writeableBitmap.BackBufferStride;
                    pBackBuffer += X * 4;

                    // Compute the pixel's color.
                    int color_data = r << 16; // R
                    color_data |= g << 8;   // G
                    color_data |= b << 0;   // B

                    // Assign the color data to the pixel.
                    *((int*)pBackBuffer) = color_data;
                }

                // Specify the area of the bitmap that changed.
                writeableBitmap.AddDirtyRect(new Int32Rect(X, Y, 1, 1));
            }
            finally
            {
                // Release the back buffer and make it available for display.
                writeableBitmap.Unlock();
            }
        }

        [STAThread]
        void DrawLine()
        {
            try
            {
                // Reserve the back buffer for updates.
                writeableBitmap.Lock();

                unsafe
                {
                   
                     writeableBitmap.WritePixels(new Int32Rect(0, Y, Width, 1), colorData.ToArray(), writeableBitmap.BackBufferStride, 0, Y); 
                }

            }
            finally
            {
                // Release the back buffer and make it available for display.
                writeableBitmap.Unlock();
            }
        }

        void DrawPicture()
        {
            try
            {
                // Reserve the back buffer for updates.
                writeableBitmap.Lock();

                unsafe
                {
                    writeableBitmap.WritePixels(new Int32Rect(0, 0, Width, Height), colorData.ToArray(), writeableBitmap.BackBufferStride, 0, 0);
                }

            }
            finally
            {
                // Release the back buffer and make it available for display.
                writeableBitmap.Unlock();
            }
        }
    }
}
