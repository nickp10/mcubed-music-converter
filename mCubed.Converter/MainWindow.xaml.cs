using System;
using System.IO;
using System.Windows;
using mCubed.Converter.Core;

namespace mCubed.Converter
{
	public partial class MainWindow : Window
	{
		#region Properties

		#region Directory

		public static readonly DependencyProperty DirectoryProperty =
			DependencyProperty.Register("Directory", typeof(string), typeof(MainWindow), new PropertyMetadata(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")));

		public string Directory
		{
			get { return (string)GetValue(DirectoryProperty); }
			set { SetValue(DirectoryProperty, value); }
		}

		#endregion

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();
		}

		#endregion

		#region Event Handlers

		private void OnConvertClick(object sender, RoutedEventArgs e)
		{
			ConvertUtilities.ConvertDirectory(Directory);
		}

		#endregion
	}
}
