using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
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

        private MediaPlayer _mediaPlayer;

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
                    media.Dispose();
                    libVlc.Dispose();
                }

                return _mediaPlayer;
            }
        }

        private ICommand _playCommand;

        public ICommand PlayCommand
        {
            private set => _playCommand = value;
            get => _playCommand ?? (_playCommand = new Command(execute: () =>
                {
                    MediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
                    MediaPlayer.Media.StateChanged += Media_StateChanged;
                    MediaPlayer.Play();
                }));
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

        #endregion

        #region Events

        private void Media_StateChanged(object sender, MediaStateChangedEventArgs e)
        {
            //            try
            //            {
            //                if(e.State == VLCState.Ended)
            //                {
            //                    Device.BeginInvokeOnMainThread(() =>
            //                    {
            //                        videoViewToDisplay.IsVisible = false;

            //                        lInfo.Text = $"{cameraObject.Name} no more data received.";
            //                        lInfo.IsVisible = true;
            //                    });

            //                    Trace.TraceError($"{nameof(MediaPlayer_EncounteredError)} of {videoViewToDisplay.MediaPlayer.Media.Mrl}");
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //#if DEBUG
            //                Trace.TraceError($"SingleCameraVisualization - MediaPlayer_EncounteredError: {ex.Message}");
            //#endif
            //            }
        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            //            try
            //            {
            //                videoViewToDisplay.MediaPlayer.Pause();
            //                videoViewToDisplay.IsVisible = false;

            //                lInfo.Text = $"{nameof(MediaPlayer_EncounteredError)} of {MediaPlayerToUse.Media.Mrl}";
            //                lInfo.IsVisible = true;

            //                Trace.TraceError($"{nameof(MediaPlayer_EncounteredError)} of {videoViewToDisplay.MediaPlayer.Media.Mrl}");
            //            }
            //            catch (Exception ex)
            //            {
            //#if DEBUG
            //                Trace.TraceError($"SingleCameraVisualization - MediaPlayer_EncounteredError: {ex.Message}");
            //#endif
            //            }
        }

        #endregion

    }
}
