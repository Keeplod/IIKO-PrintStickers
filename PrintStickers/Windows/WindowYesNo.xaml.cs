using System.Windows;

namespace PrintStickers.Windows
{
    public partial class WindowYesNo : Window
    {
        public WindowYesNo(string Title, string Message)
        {
            InitializeComponent();

            LabelTitle.Content = Title;
            LabelMessage.Text = Message;
        }

        private void Yes(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void No(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
