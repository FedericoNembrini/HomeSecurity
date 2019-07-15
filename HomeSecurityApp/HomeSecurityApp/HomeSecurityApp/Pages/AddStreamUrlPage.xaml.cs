using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeSecurityApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStreamUrlPage : ContentPage
    {
        #region Variables

        int StreamCounter;

        #endregion

        #region Constructor

        public AddStreamUrlPage(int StreamCounter)
        {
            InitializeComponent();

            this.StreamCounter = StreamCounter;
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            #if RELEASE
                if(!string.IsNullOrEmpty(eNewStreamName.Text) && !string.IsNullOrEmpty(eNewStreamUrl.Text))
                {
                    Preferences.Set(Utility.Utility.Key + StreamCounter, $"{eNewStreamName.Text}#{eNewStreamUrl.Text}");
                }
            #endif

            await Navigation.PopModalAsync(true);
        }

        #endregion
    }
}