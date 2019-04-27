using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeSecurityApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShellApp : Xamarin.Forms.Shell
	{
		public ShellApp ()
		{
			InitializeComponent ();
		}
	}
}