using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeSecurityApp.ViewModels
{
    public class SingleCameraVisualizationViewModel : BaseViewModel
    {
        #region Variables

        private CameraObjectViewModel _cameraObjectViewModel;

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _isErrorMessageVisible = false;

        public bool IsErrorMessageVisible 
        {
            get => _isErrorMessageVisible;
            set
            {
                _isErrorMessageVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isVideoViewVisible = true;

        public bool IsVideoViewVisible
        {
            get => _isVideoViewVisible;
            set
            {
                _isVideoViewVisible = value;
                OnPropertyChanged();
            }
        }

        private MediaPlayer _mediaPlayer;

        public MediaPlayer MediaPlayer
        {
            private set => _mediaPlayer = value;
            get 
            {
                if (_mediaPlayer is null)
                {
                    var libVlc = new LibVLC();
                    var media = new Media(libVlc, _cameraObjectViewModel.ConnectionUrl, FromType.FromLocation);
                    _mediaPlayer = new MediaPlayer(media);
                    _mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
                    _mediaPlayer.Media.StateChanged += Media_StateChanged;
                    media.Dispose();
                    libVlc.Dispose();
                }

                return _mediaPlayer;
            }
        }

        #endregion

        #region Constructor

        public SingleCameraVisualizationViewModel() { }

        public SingleCameraVisualizationViewModel(CameraObjectViewModel cameraObjectViewModel)
        {
            _cameraObjectViewModel = cameraObjectViewModel;
        }

        #endregion

        #region Method

        public void StartView()
        {
            MediaPlayer.Play();
        }

        public void StopView()
        {
            MediaPlayer.Stop();
        }

        #endregion

        #region Events

        private void Media_StateChanged(object sender, MediaStateChangedEventArgs e)
        {
            try
            {
                if (e.State == VLCState.Ended)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsVideoViewVisible = false;

                        ErrorMessage = $"{_cameraObjectViewModel.Name} no more data received.";
                        IsErrorMessageVisible = true;
                    });

                    Trace.TraceError($"{nameof(SingleCameraVisualizationViewModel)} - {nameof(MediaPlayer_EncounteredError)}: {MediaPlayer.Media.Mrl}");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"{nameof(SingleCameraVisualizationViewModel)} - {nameof(Media_StateChanged)}: {ex.Message}");
            }
        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            try
            {
                MediaPlayer.Pause();

                Device.BeginInvokeOnMainThread(() =>
                {
                    IsVideoViewVisible = false;

                    ErrorMessage = $"{nameof(MediaPlayer_EncounteredError)} of {MediaPlayer.Media.Mrl}";
                    IsErrorMessageVisible = true;
                });
                
                Trace.TraceError($"{nameof(SingleCameraVisualizationViewModel)} - {nameof(MediaPlayer_EncounteredError)}: {MediaPlayer.Media.Mrl}");
            }
            catch (Exception ex)
            {
                Trace.TraceError($"{nameof(SingleCameraVisualizationViewModel)} - {nameof(MediaPlayer_EncounteredError)}: {ex.Message}");
            }
        }

        #endregion
    }
}
