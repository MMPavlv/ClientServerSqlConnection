using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels
{
    public enum NotificationKind
    {
        Info,
        Success,
        Error,
        Warning
    }

    public partial class NotificationViewModel : ObservableObject
    {
        public NotificationViewModel(string title, string message, NotificationKind kind)
        {
            Title = title;
            Message = message;
            Kind = kind;
            Opacity = 0;
        }

        public string Title { get; }

        public string Message { get; }

        public NotificationKind Kind { get; }

        public double Opacity
        {
            get;
            set => SetProperty(ref field, value);
        }

        public IBrush Background =>
            Kind switch
            {
                NotificationKind.Success => new SolidColorBrush(Color.Parse("#1F6F43")),
                NotificationKind.Error => new SolidColorBrush(Color.Parse("#8B1E3F")),
                NotificationKind.Warning => new SolidColorBrush(Color.Parse("#8A5A00")),
                _ => new SolidColorBrush(Color.Parse("#1E5AA8"))
            };

        public IBrush BorderBrush =>
            Kind switch
            {
                NotificationKind.Success => new SolidColorBrush(Color.Parse("#69D39B")),
                NotificationKind.Error => new SolidColorBrush(Color.Parse("#FF8DA1")),
                NotificationKind.Warning => new SolidColorBrush(Color.Parse("#F4C15D")),
                _ => new SolidColorBrush(Color.Parse("#86BAFF"))
            };
    }
}
