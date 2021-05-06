using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UpcomingMovies.Resources;

namespace UpcomingMovies.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox()
        {
            InitializeComponent();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        void AddButtons(MessageBoxButton buttons)
        {
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    AddButton(AllResources.OkButtonText, MessageBoxResult.OK);
                    break;
                case MessageBoxButton.OKCancel:
                    AddButton(AllResources.OkButtonText, MessageBoxResult.OK);
                    AddButton(AllResources.CancelButtonText, MessageBoxResult.Cancel, isCancel: true);
                    break;
                case MessageBoxButton.YesNo:
                    AddButton(AllResources.YesButtonText, MessageBoxResult.Yes);
                    AddButton(AllResources.NoButtonText, MessageBoxResult.No);
                    break;
                case MessageBoxButton.YesNoCancel:
                    AddButton(AllResources.YesButtonText, MessageBoxResult.Yes);
                    AddButton(AllResources.NoButtonText, MessageBoxResult.No);
                    AddButton(AllResources.CancelButtonText, MessageBoxResult.Cancel, isCancel: true);
                    break;
                default:
                    throw new ArgumentException(AllResources.UnknownButtonText, AllResources.ButtonsText);
            }
        }
        void AddButton(string text, MessageBoxResult result, bool isCancel = false)
        {
            var button = new Button() { Content = text, IsCancel = isCancel };
            button.Click += (o, args) => { Result = result; DialogResult = true; };
            ButtonContainer.Children.Add(button);
        }
        MessageBoxResult Result = MessageBoxResult.None;
        public static MessageBoxResult Show(string message,
                                            MessageBoxButton buttons)
        {
            var dialog = new CustomMessageBox();
            dialog.MessageContainer.Text = message;
            dialog.AddButtons(buttons);
            dialog.ShowDialog();
            return dialog.Result;
        }
    }
}
