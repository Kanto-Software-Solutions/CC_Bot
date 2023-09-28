namespace cc_bot.Clases;

// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

public class CandyColorMapper
{

	[StructLayout(LayoutKind.Sequential)]
	public struct Rect
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	[DllImport("user32.dll")]
	private static extern int SetForegroundWindow(IntPtr hWnd);

	private const int SW_RESTORE = 9;

	[DllImport("user32.dll")]
	private static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);

	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

	public static Bitmap CaptureApplication(string procName)
	{
		Process proc;

		// Cater for cases when the process can't be located.
		try
		{
			proc = Process.GetProcessesByName(procName)[0];
		}
		catch (IndexOutOfRangeException e)
		{
			return null;
		}

		// You need to focus on the application
		SetForegroundWindow(proc.MainWindowHandle);
		ShowWindow(proc.MainWindowHandle, SW_RESTORE);

		// You need some amount of delay, but 1 second may be overkill
		Thread.Sleep(20);

		Rect rect = new Rect();
		IntPtr error = GetWindowRect(proc.MainWindowHandle, ref rect);

		// sometimes it gives error.
		while (error == (IntPtr)0)
		{
			error = GetWindowRect(proc.MainWindowHandle, ref rect);
		}

		int width = 639; // Calibrar para emulador
		int height = 567; //Calibrar para emulador

		Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
		Graphics.FromImage(bmp).CopyFromScreen(rect.left + 110, rect.top + 47, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

		return bmp;
	}

	public static int[,] GetBoard()
	{
		int rows = 9;
		int columns = 9;
		int i = 0;

		Bitmap gameBitmap = CaptureApplication("flashplayer_32_sa");
		//gameBitmap.Save("xd.png", ImageFormat.Png);

		int[,] board = new int[81, 2];

		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < columns; col++)
			{
				// Calculate the center pixel coordinates of the current cell
				int centerX = (col * 71) + (71 / 2);
				int centerY = (row * 63) + (63 / 2);

				// Get the average pixel color
				Rectangle rect1 = new Rectangle((col * 71), (row * 63), 15, 15);
				Bitmap cell1 = gameBitmap.Clone(rect1, gameBitmap.PixelFormat);

				Rectangle rect2 = new Rectangle((col * 71) + 20, (row * 63) + 18, 35, 35);
				Bitmap cell2 = gameBitmap.Clone(rect2, gameBitmap.PixelFormat);


				//cell1.Save("cell1 " + row.ToString() + col.ToString() + ".png", ImageFormat.Png);
				//cell2.Save("cell2 " + row.ToString() + col.ToString() + ".png", ImageFormat.Png);

				//Console.WriteLine("cell1 " + row.ToString() + ", " + col.ToString());
				//Console.WriteLine("cell2 " + row.ToString() + ", " + col.ToString());

				//Color pixelColor = gameBitmap.GetPixel(centerX, centerY);
				var cellCol1 = CalculateAverageColor(cell1);
				var cellCol2 = CalculateAverageColor(cell2);

				//Console.WriteLine(cellCol1 + " " + cellCol2);

				var dupla = MapColorToCandyType(cellCol1, cellCol2);
				//Console.WriteLine(dupla[0].ToString() + dupla[1].ToString());
				board[i, 0] = dupla[0];
				board[i, 1] = dupla[1];
				i++;
			}
		}
		return board;
	}

	public static System.Drawing.Color CalculateAverageColor(Bitmap bm)
	{
		int width = bm.Width;
		int height = bm.Height;
		int red = 0;
		int green = 0;
		int blue = 0;
		int minDiversion = 15;  // drop pixels that do not differ by at least minDiversion between color values (white, gray or black)
		int dropped = 0;        // keep track of dropped pixels
		long[] totals = new long[] { 0, 0, 0 };
		int bppModifier = bm.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4; // cutting corners, will fail on anything else but 32 and 24 bit images

		BitmapData srcData = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, bm.PixelFormat);
		int stride = srcData.Stride;
		IntPtr Scan0 = srcData.Scan0;

		unsafe
		{
			byte* p = (byte*)(void*)Scan0;

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					int idx = (y * stride) + x * bppModifier;
					red = p[idx + 2];
					green = p[idx + 1];
					blue = p[idx];
					if (Math.Abs(red - green) > minDiversion || Math.Abs(red - blue) > minDiversion || Math.Abs(green - blue) > minDiversion)
					{
						totals[2] += red;
						totals[1] += green;
						totals[0] += blue;
					}
					else
					{
						dropped++;
					}
				}
			}
		}

		int count = width * height - dropped;
		if (count == 0) count = 1; // prevent divide by zero errors (pure black image
		int avgR = (int)(totals[2] / count);
		int avgG = (int)(totals[1] / count);
		int avgB = (int)(totals[0] / count);

		return System.Drawing.Color.FromArgb(avgR, avgG, avgB);
	}

	public static int[] MapColorToCandyType(Color cell1, Color cell2)
	{

		// Extract color components
		int red = cell1.R;
		int blue = cell1.B;
		int green = cell1.G;

		int red2 = cell2.R;
		int blue2 = cell2.B;
		int green2 = cell2.G;
		int[] candyColor = new int[2];

		// Map the color to a candy type based on thresholds
		// RED
		if (red2 > 205 && red2 < 230 && blue2 < 81 && blue2 > 65 && green2 < 81 && green2 > 65)
		{
			candyColor[0] = 4;
			candyColor[1] = 2;
			return candyColor; //Red Striped
		}
		else if (red > 120 && blue < 85 && green < 80 && green > 65 && (red2 > 210 && green2 < 55 && blue2 < 55))
		{
			candyColor[0] = 4;
			candyColor[1] = 3;
			return candyColor; //Red Packet
		}
		else if (red2 > 190 && red2 < 205 && green2 < 25 && blue2 < 30)
		{
			candyColor[0] = 4;
			candyColor[1] = 1;
			return candyColor; //Red
		}
		// BLUE
		else if (blue > 150 && red > 60 && green < 150 && (blue2 < 255 && blue2 > 225 && red2 < 45 && red2 > 20 && green2 > 145 && green2 < 170))
		{
			candyColor[0] = 6;
			candyColor[1] = 3;
			return candyColor; //Blue Packet
		}
		else if (blue2 < 235 && blue2 > 219 && red2 < 75 && red2 > 55 && green2 > 150 && green2 < 170)
		{
			candyColor[0] = 6;
			candyColor[1] = 2;
			return candyColor; //Blue Striped
		}

		else if (blue2 > 230 && red2 < 40 && green2 > 120 && green2 < 135)
		{
			candyColor[0] = 6;
			candyColor[1] = 1;
			return candyColor; //Blue
		}
		// GREEN
		else if (red > 65 && red < 85 && green > 110 && green < 135 && blue < 95 && (red2 > 70 && red2 < 100 && green2 > 190 && blue2 < 50))
		{
			candyColor[0] = 5;
			candyColor[1] = 3;
			return candyColor; //Green Packet
		}
		else if (green2 > 210 && red2 > 100 && red2 < 125 && blue2 < 100 && blue2 > 60)
		{
			candyColor[0] = 5;
			candyColor[1] = 2;
			return candyColor; //Green Striped
		}
		else if (green2 == 160 && red2 == 51 && blue2 == 6)
		{
			candyColor[0] = 5;
			candyColor[1] = 1;
			return candyColor; //Green 
		}
		// YELLOW
		else if (red > 200 && green > 165 && blue < 90)
		{
			candyColor[0] = 1;
			candyColor[1] = 3;
			return candyColor; //Yellow Packet
		}
		else if (red2 > 220 && green2 < 210 && green2 > 185 && blue2 < 75 && blue2 > 55)
		{
			candyColor[0] = 1;
			candyColor[1] = 2;
			return candyColor; //Yellow Striped
		}
		else if (red2 > 235 && red2 < 240 && green2 < 189 && green2 > 183 && blue2 < 8)
		{
			candyColor[0] = 1;
			candyColor[1] = 1;
			return candyColor; //Yellow 
		}
		// PURPLE
		else if (red > 100 && red < 130 && green < 110 && green > 80 && blue > 140 && (red2 > 180 && red2 < 230 && green2 < 60 && green2 > 30 && blue2 > 210))
		{
			candyColor[0] = 2;
			candyColor[1] = 3;
			return candyColor; //Purple Packet
		}
		else if (red2 > 200 && red2 < 230 && green2 < 110 && green2 > 75 && blue2 > 150)
		{
			candyColor[0] = 2;
			candyColor[1] = 2;
			return candyColor; //Purple Striped
		}
		else if (red2 > 184 && red2 < 195 && green2 < 40 && green2 > 30 && blue2 > 245 && blue2 < 255)
		{
			candyColor[0] = 2;
			candyColor[1] = 1;
			return candyColor; //Purple
		}
		// ORANGE
		else if (red2 > 230 && green2 > 165 && green2 < 185 && blue2 < 110 && blue2 > 80)
		{
			candyColor[0] = 3;
			candyColor[1] = 2;
			return candyColor; //Orange Striped
		}
		else if (red > 150 && red < 180 && green > 110 && green < 130 && blue < 95 && (red2 > 230 && green2 > 145 && green2 < 170 && blue2 < 47))
		{
			candyColor[0] = 3;
			candyColor[1] = 3;
			return candyColor; //Orange Packet
		}
		else if (red2 > 240 && red2 > 240 && green2 > 129 && green2 < 140 && blue2 > 15 && blue2 < 28)
		{
			candyColor[0] = 3;
			candyColor[1] = 1;
			return candyColor; //Orange
		}
		// BROWNIE
		else if (red2 > 100 && red2 < 120 && green2 > 60 && green2 < 80 && blue2 > 40 && blue2 < 70)
		{
			candyColor[0] = 7;
			candyColor[1] = 7;
			return candyColor; //Brownie
		}
		// If no match is found, return a default value or handle it accordingly
		candyColor[0] = 8;
		candyColor[1] = 8;
		return candyColor;
	}

}


