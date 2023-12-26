using CommunityToolkit.Maui.Views;
using Months18.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace Months18.ViewModels
{
    public class MusicPlayerViewModel : BaseViewModel
    {
        public MusicPlayerViewModel(MusicPlayerService service)
        {
            _musicPlayerService = service;

            InitializeCommands();
        }

        public ObservableCollection<string> PlayList { get; } = new();
        public Command? PlayCommand;
        public Command? PauseCommand;
        public Command StopCommand;

        private MediaElement? _mediaElement;

        public MediaElement? MediaElement
        {
            get { return _mediaElement; }
            set
            {
                if (_mediaElement != value)
                {
                    _mediaElement = value;
                    OnPropertyChanged(nameof(MediaElement));
                }
            }
        }
        private MusicPlayerService _musicPlayerService;

 
        private void InitializeCommands()
        {
            PlayCommand = new Command(HandlePlay);
            PauseCommand = new Command(HandlePause);
            StopCommand = new Command(HandleStop);

        }

        public void HandlePlay() => _mediaElement?.Play();

        public void HandlePause() => _mediaElement?.Pause();

        public void HandleStop() => _mediaElement?.Stop();
    }

}
