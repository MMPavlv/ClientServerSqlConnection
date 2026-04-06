using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public partial class NotificationService : ObservableObject
    {
        private const int FadeDurationMs = 250;

        public ObservableCollection<NotificationViewModel> Notifications { get; } = new();

        public void Show(
            string title,
            string message,
            NotificationKind kind = NotificationKind.Info,
            int durationMs = 3000)
        {
            var notification = new NotificationViewModel(title, message, kind);
            Notifications.Add(notification);
            Dispatcher.UIThread.Post(() => notification.Opacity = 1);

            if (durationMs <= 0)
            {
                return;
            }

            _ = CloseAfterDelayAsync(notification, durationMs);
        }

        private async Task CloseAfterDelayAsync(NotificationViewModel notification, int durationMs)
        {
            await Task.Delay(durationMs);
            await Dispatcher.UIThread.InvokeAsync(() => notification.Opacity = 0);
            await Task.Delay(FadeDurationMs);
            await Dispatcher.UIThread.InvokeAsync(() => Notifications.Remove(notification));
        }
    }
}
