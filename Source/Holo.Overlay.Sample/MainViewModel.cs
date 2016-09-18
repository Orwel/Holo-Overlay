using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Holo.Overlay.Sample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object adornerContent;
        private object waitingContent;
        private object messageContent;
        public object RootAdornerContent { get { return adornerContent; } set { SetProperty(ref adornerContent, value); } }
        public object WaitingContent { get { return waitingContent; } set { SetProperty(ref waitingContent, value); } }
        public object MessageContent { get { return messageContent; } set { SetProperty(ref messageContent, value); } }
        public RelayCommand WorkflowCommand { get; set; }
        public RelayCommand WaitingCommand { get; set; }
        public RelayCommand MessageBoxCommand { get; set; }

        public MainViewModel()
        {
            WaitingCommand = new RelayCommand(DisplayWaiting);
            MessageBoxCommand = new RelayCommand(DisplayMessageBox);
            WorkflowCommand = new RelayCommand(Worflow);
        }

        public async void DisplayWaiting(object o)
        {
            WaitingContent = new Context.Waiting();
            await Task.Delay(TimeSpan.FromSeconds(2));
            WaitingContent = null;
        }

        public async void Worflow(object o)
        {
            RootAdornerContent = new Context.Waiting();
            await Task.Delay(TimeSpan.FromSeconds(2));
            RootAdornerContent = new Context.MessageBox { Text = "The workflow is ended.", ButtonText = "Ok", ButtonCommand = new RelayCommand((ob) => { RootAdornerContent = null; }) };
        }

        public void DisplayMessageBox(object o)
        {
            MessageContent = new Context.MessageBox { Text = "An error has occurred... No more information.", ButtonText = "Really?", ButtonCommand = new RelayCommand((ob) => { MessageContent = null; }) };
        }

        #region ViewModel Base
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion ViewModel Base
    }
}
