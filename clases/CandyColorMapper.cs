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
		Thread.Sleep(10);

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
				Rectangle rect = new Rectangle(col * 71, row * 63, 71, 63);
				Bitmap cell = gameBitmap.Clone(rect, gameBitmap.PixelFormat);
				
				cell.Save("cell " + row.ToString() + col.ToString() + ".png", ImageFormat.Png);

				Console.WriteLine("cell " + row.ToString() + ", " + col.ToString());
				Color pixelColor = gameBitmap.GetPixel(centerX, centerY);
				var cellCol = CalculateAverageColor(cell);
				Console.WriteLine(cellCol);
				var dupla = MapColorToCandyType(cellCol);
				Console.WriteLine(dupla[0].ToString() + dupla[1].ToString());
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
		int minDiversion = 15; // drop pixels that do not differ by at least minDiversion between color values (white, gray or black)
		int dropped = 0; // keep track of dropped pixels
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
		int avgR = (int)(totals[2] / count);
		int avgG = (int)(totals[1] / count);
		int avgB = (int)(totals[0] / count);

		return System.Drawing.Color.FromArgb(avgR, avgG, avgB);
	}


	public static int[] MapColorToCandyType(Color color)
	{

		// Extract color components
		int red = color.R;
		int blue = color.B;
		int green = color.G;
		int[] candyColor = new int[2];

		// Map the color to a candy type based on thresholds
		// RED
		if (red > 135 && red < 170 && blue < 100 && green < 95)
		{
			candyColor[0] = 4;
			candyColor[1] = 2;
			return candyColor; //Red Striped
		}
		else if (red > 170 && blue < 80 && green < 75)
		{
			candyColor[0] = 4;
			candyColor[1] = 3;
			return candyColor; //Red Packet
		}
		else if (red > 95 && blue < 91 && green < 81)
		{
			candyColor[0] = 4;
			candyColor[1] = 1;
			return candyColor; //Red
		}
		// BLUE
		else if (blue > 195 && red < 60 && green < 150)
		{
			candyColor[0] = 6;
			candyColor[1] = 3;
			return candyColor; //Blue Packet
		}
		else if (blue < 165 && blue > 130 && red < 75 && red > 55 && green > 100 && green < 130)
		{
			candyColor[0] = 6;
			candyColor[1] = 2;
			return candyColor; //Blue Striped
		}
		else if (blue > 140 && red < 65 && green < 120)
		{
			candyColor[0] = 6;
			candyColor[1] = 1;
			return candyColor; //Blue
		}
		// GREEN
		else if (green > 155 && red < 90 && blue < 80 && blue > 55)
		{
			candyColor[0] = 5;
			candyColor[1] = 3;
			return candyColor; //Green Packet
		}
		else if (green > 140 && red < 100 && blue < 100 && blue > 60)
		{
			candyColor[0] = 5;
			candyColor[1] = 2;
			return candyColor; //Green Striped
		}
		else if (green > 115 && red < 70 && blue < 60)
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
		else if (red > 120 && red < 155 && green < 150 && green > 115 && blue < 100 && blue > 80)
		{
			candyColor[0] = 1;
			candyColor[1] = 2;
			return candyColor; //Yellow Striped
		}
		else if (red > 120 && red < 161 && green < 150 && green > 118 && blue < 80)
		{
			candyColor[0] = 1;
			candyColor[1] = 1;
			return candyColor; //Yellow 
		}
		// PURPLE
		else if (red > 150 && green < 85 && blue > 200)
		{
			candyColor[0] = 2;
			candyColor[1] = 3;
			return candyColor; //Purple Packet
		}
		else if (red > 100 && red < 140 && green < 110 && green > 75 && blue > 150)
		{
			candyColor[0] = 2;
			candyColor[1] = 2;
			return candyColor; //Purple Striped
		}
		else if (red > 100 && red < 140 && green < 75 && green > 45 && blue > 150)
		{
			candyColor[0] = 2;
			candyColor[1] = 1;
			return candyColor; //Purple
		}
		// ORANGE
		else if (red > 130 && green > 110 && blue > 80)
		{
			candyColor[0] = 3;
			candyColor[1] = 2;
			return candyColor; //Orange Striped
		}
		else if (red > 200 && green > 125 && blue < 81)
		{
			candyColor[0] = 3;
			candyColor[1] = 3;
			return candyColor; //Orange Packet
		}
		else if (red > 125 && green > 89 && blue < 85)
		{
			candyColor[0] = 3;
			candyColor[1] = 1;
			return candyColor; //Orange (Bug with Yellow) 
		}
		// BROWNIE
		else if (red > 70 && red < 100 && green > 60 && green < 100 && blue > 60 && blue < 100)
		{
			candyColor[0] = 7;
			candyColor[1] = 7;
			return candyColor; //Brownie
		}
		// If no match is found, return a default value or handle it accordingly
		candyColor[0] = 0;
		candyColor[1] = 0;
		return candyColor;
	}

}


