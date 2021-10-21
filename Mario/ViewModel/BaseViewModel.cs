using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Mario.ViewModel
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		private readonly Dispatcher _dispatcher;

		protected BaseViewModel()
		{
			_dispatcher = Dispatcher.CurrentDispatcher;
		}

		[Display(AutoGenerateField = false)]
		public Dispatcher UiDispatcher
		{
			get { return _dispatcher; }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName = "")
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
