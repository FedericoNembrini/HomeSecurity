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
    public partial class AddStreamUrlPage : ContentPage
    {
        #region Variables

        #endregion

        #region Constructor

        public AddStreamUrlPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Override Region

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        #endregion

        #region Private Method

        #endregion

        #region Event Hanlder

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        #endregion
    }
}