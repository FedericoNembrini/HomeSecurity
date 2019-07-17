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
        private void ENewStreamName_TextChanged(object sender, TextChangedEventArgs e)
        {
            lNameError.Text = string.Empty;
            slNameError.IsVisible = false;
        }

        private void ENewStreamUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            lUrlError.Text = string.Empty;
            slUrlError.IsVisible = false;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
#if RELEASE
            if (string.IsNullOrEmpty(eNewStreamName.Text))
            {
                lNameError.Text = "Name Required";
                slNameError.IsVisible = true;
                return;
            }

            if (string.IsNullOrEmpty(eNewStreamUrl.Text))
            {
                lUrlError.Text = "Url Required";
                slUrlError.IsVisible = true;
                return;
            }

            if (eNewStreamUrl.Text.Contains('#'))
            {
                lUrlError.Text = "Url can't contain #";
                slUrlError.IsVisible = true;
                return;
            }

            Preferences.Set(Utility.Utility.Key + StreamCounter, $"{eNewStreamName.Text}#{eNewStreamUrl.Text}");
#endif
            await Navigation.PopModalAsync(true);
        }

        #endregion
    }
}