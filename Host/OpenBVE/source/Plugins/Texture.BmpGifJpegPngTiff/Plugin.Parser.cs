using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using OpenBveApi.Colors;
using OpenBveApi.Textures;
using OpenBveApi.Hosts;

namespace Plugin {
	public partial class Plugin {
		
		/// <summary>Loads a texture from the specified file.</summary>
		/// <param name="file">The file that holds the texture.</param>
		/// <param name="texture">Receives the texture.</param>
		/// <returns>Whether loading the texture was successful.</returns>
		private bool Parse(string file, out Texture texture) {
			/*
			 * Read the bitmap. This will be a bitmap of just
			 * any format, not necessarily the one that allows
			 * us to extract the bitmap data easily.
			 */
			Bitmap bitmap = new Bitmap(file);
			Color24[] p = null;
			if (file.ToLowerInvariant().EndsWith(".bmp") && EnabledHacks.ReduceTransparencyColorDepth && (bitmap.PixelFormat == PixelFormat.Format32bppArgb || bitmap.PixelFormat == PixelFormat.Format24bppRgb))
			{
				/*
				 * Our source bitmap is *not* a 256 color bitmap but has been made for BVE2 / BVE4.
				 * These process transparency in 256 colors (even if the file is 24bpp / 32bpp), thus:
				 * Let's open the bitmap, and attempt to construct a reduced color pallette
				 * If our bitmap contains more than 256 unique colors, we break out of the loop
				 * and assume that this file is an incorrect match
				 *
				 * WARNING NOTE:
				 * Unfortunately, we can't just pull out the color pallette from the bitmap, as there
				 * is no native way to remove unused entries. We therefore have to itinerate through
				 * each pixel.....
				 * This is *slow* so use with caution!
				 *
				 */

				BitmapData inputData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				HashSet<Color24> reducedPallette = new HashSet<Color24>();
				unsafe
				{
					byte* bmpPtr = (byte*) inputData.Scan0.ToPointer();
					int ic, oc, r;
					if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
					{
						for (r = 0; r < inputData.Height; r++)
						for (ic = oc = 0; oc < inputData.Width; ic += 3, oc++)
						{
							byte blue = bmpPtr[r * inputData.Stride + ic];
							byte green = bmpPtr[r * inputData.Stride + ic + 1];
							byte red = bmpPtr[r * inputData.Stride + ic + 2];
							Color24 c = new Color24(red, green, blue);
							if (!reducedPallette.Contains(c))
							{
								reducedPallette.Add(c);
							}
							if (reducedPallette.Count > 256)
							{
								//as breaking out of nested loops is a pita
								goto EndLoop;
							}
						}
					}
					else
					{
						for (r = 0; r < inputData.Height; r++)
						for (ic = oc = 0; oc < inputData.Width; ic += 4, oc++)
						{
							byte blue = bmpPtr[r * inputData.Stride + ic];
							byte green = bmpPtr[r * inputData.Stride + ic + 1];
							byte red = bmpPtr[r * inputData.Stride + ic + 2];
							Color24 c = new Color24(red, green, blue);
							if (!reducedPallette.Contains(c))
							{
								reducedPallette.Add(c);
							}
							if (reducedPallette.Count > 256)
							{
								//as breaking out of nested loops is a pita
								goto EndLoop;
							}
						}
					}
				}

				p = reducedPallette.ToArray();
				EndLoop:
				bitmap.UnlockBits(inputData);
			}
			
			
			if (bitmap.PixelFormat != PixelFormat.Format32bppArgb && bitmap.PixelFormat != PixelFormat.Format24bppRgb && p == null)
			{
				/* Otherwise, only store the color palette data for
				 * textures using a restricted palette
				 * With a large number of textures loaded at
				 * once, this can save a decent chunk of memory
				 */
				p = new Color24[bitmap.Palette.Entries.Length];
				for (int i = 0; i < bitmap.Palette.Entries.Length; i++)
				{
					p[i] = bitmap.Palette.Entries[i];
				}
			}
			Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			/* 
			 * If the bitmap format is not already 32-bit BGRA,
			 * then convert it to 32-bit BGRA.
			 */
			if (bitmap.PixelFormat != PixelFormat.Format32bppArgb) {
				Bitmap compatibleBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
				Graphics graphics = Graphics.FromImage(compatibleBitmap);
				graphics.DrawImage(bitmap, rect, rect, GraphicsUnit.Pixel);
				graphics.Dispose();
				bitmap.Dispose();
				bitmap = compatibleBitmap;
			}
			/*
			 * Extract the raw bitmap data.
			 */
			BitmapData data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
			if (data.Stride == 4 * data.Width) {
				/*
				 * Copy the data from the bitmap
				 * to the array in BGRA format.
				 */
				byte[] raw = new byte[data.Stride * data.Height];
				System.Runtime.InteropServices.Marshal.Copy(data.Scan0, raw, 0, data.Stride * data.Height);
				bitmap.UnlockBits(data);
				int width = bitmap.Width;
				int height = bitmap.Height;
				
				/*
				 * Change the byte order from BGRA to RGBA.
				 */
				for (int i = 0; i < raw.Length; i += 4) {
					byte temp = raw[i];
					raw[i] = raw[i + 2];
					raw[i + 2] = temp;
				}
				texture = new Texture(width, height, 32, raw, p);
				bitmap.Dispose();
				return true;
			}

			/*
			 * The stride is invalid. This indicates that the
			 * CLI either does not implement the conversion to
			 * 32-bit BGRA correctly, or that the CLI has
			 * applied additional padding that we do not
			 * support.
			 */
			bitmap.UnlockBits(data);
			bitmap.Dispose();
			CurrentHost.ReportProblem(ProblemType.InvalidOperation, "Invalid stride encountered.");
			texture = null;
			return false;
		}
		
	}
}
