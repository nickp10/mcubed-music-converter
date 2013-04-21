using System.ComponentModel;
using System.Windows;
using mCubed.Converter.Core;

namespace mCubed.Converter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		#region Directory

		private string _directory = @"C:\Users\Nick\Downloads\";
		public string Directory
		{
			get { return _directory; }
			set
			{
				if (_directory != value)
				{
					_directory = value;
					OnPropertyChanged("Directory");
				}
			}
		}

		#endregion

		#region INotifyPropertyChanged Members

		protected void OnPropertyChanged(string property)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(property));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public MainWindow()
		{
			DataContext = this;
			InitializeComponent();
		}

		private void OnConvertClick(object sender, RoutedEventArgs e)
		{
			ConvertUtilities.ConvertDirectory(Directory);
		}
	}
}
