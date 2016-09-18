using System.Windows.Input;

namespace Holo.Overlay.Context
{
    public class MessageBox
    {
        public string Text { get; set; }
        public string ButtonText { get; set; }
        public ICommand ButtonCommand { get; set; }
    }
}
