using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace mCubed.Converter.Core
{
	public static class ConvertUtilities
	{
		public static void ConvertDirectory(string directory)
		{
			ThreadPool.QueueUserWorkItem(q =>
			{
				var info = new DirectoryInfo(directory);
				if (info.Exists)
				{
					foreach (var file in info.GetFiles().Where(f => (f.Attributes & FileAttributes.Hidden) == 0))
					{
						ConvertFile(file.FullName);
					}
				}
			});
		}

		/// <summary>
		/// Converts the given file to an .mp3. If the file is already an .mp3, then no
		/// conversion will take place. Otherwise, the file will be converter to an .mp3
		/// with the given bitrate and channels. The converted file will be placed in the
		/// input directory with the same filename, except with a .mp3 extension.
		/// </summary>
		/// <param name="file">The input file to convert.</param>
		/// <param name="bitrate">The bitrate for the .mp3 (defaults to 320 kb/s).</param>
		/// <param name="channels">The number of channels for the .mp3 (1 = mono, 2 = stereo).</param>
		/// <returns>The path to the converted file, or null if the conversion did not occur.</returns>
		public static string ConvertFile(string file, int bitrate = 320, int channels = 2)
		{
			file = file == null ? null : Path.GetFullPath(file);
			if (file != null && File.Exists(file))
			{
				string ext = Path.GetExtension(file);
				if (!string.IsNullOrEmpty(ext))
				{
					if (ext == ".mp3")
					{
						return file;
					}
					else
					{
						string outputFile;
						var command = CreateVLCCommand(file, bitrate, channels, out outputFile);
						using (var process = new Process
						{
							StartInfo = new ProcessStartInfo
							{
								FileName = GetVLCPath(),
								WindowStyle = ProcessWindowStyle.Hidden,
								CreateNoWindow = true,
								Arguments = command,
								UseShellExecute = false
							}
						})
						{
							process.Start();
							process.WaitForExit();
						}
						return outputFile;
					}
				}
			}
			return null;
		}

		private static string CreateVLCCommand(string inputFile, int bitrate, int channels, out string outputFile)
		{
			outputFile = Path.ChangeExtension(inputFile, ".mp3");
			return string.Format("\"{0}\" --sout=#transcode{{acodec=mp3,vcodec=dummy,ab={1},channels={2}}}:standard{{access=file,mux=raw,dst=\"{3}\"}} vlc://quit", inputFile, bitrate, channels, outputFile);
		}

		private static string GetVLCPath()
		{
			var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			var path = Path.Combine(programFiles, "VideoLAN", "VLC", "vlc.exe");
			if (File.Exists(path))
			{
				return path;
			}
			programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
			path = Path.Combine(programFiles, "VideoLAN", "VLC", "vlc.exe");
			if (File.Exists(path))
			{
				return path;
			}
			return null;
		}
	}
}
