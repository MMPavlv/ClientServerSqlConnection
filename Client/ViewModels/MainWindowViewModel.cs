using Client.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using static System.Collections.Specialized.BitVector32;

namespace Client.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private const string DefaultAddress = "localhost";
        private const int DefaultServerPort = 5241;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenConnectionCommand))]
        [NotifyCanExecuteChangedFor(nameof(RequestVersionCommand))]
        [NotifyCanExecuteChangedFor(nameof(CloseConnectionCommand))]
        private bool _isRequestInProgress;

        private bool CanRunRequest() => !IsRequestInProgress;

        public NotificationService Notifications { get; } = new();

        public string ServerIp
        {
            get;
            set => SetProperty(ref field, value);
        }

        public string Version
        {
            get;
            set => SetProperty(ref field, value);
        }

        public string Log
        {
            get;
            set => SetProperty(ref field, value);
        }

        public MainWindowViewModel()
        {
            ServerIp = $"{DefaultAddress}:{DefaultServerPort}";
            Log = string.Empty;
            Version = string.Empty;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRunRequest))]
        private async Task OpenConnectionAsync()
        {
            await RunRequestAsync(async () =>
                {
                    try
                    {
                        await ConnectionService.SendRequestAsync(ServerIp, ConnectionService.ConnectAction);
                        Notifications.Show("Success", "Connection was established", NotificationKind.Success);
                        WriteLog("Connection was established");
                    }
                    catch (Exception e)
                    {
                        WriteLog(e.Message);
                        Notifications.Show("Error", e.Message.ToShortString(), NotificationKind.Error);
                    }
                });
        }


        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRunRequest))]
        private async Task RequestVersionAsync()
        {
            await RunRequestAsync(async () =>
                {
                    try
                    {
                        var responseBody = await ConnectionService.SendRequestAsync(ServerIp, ConnectionService.GetVersionAction);

                        Version = ConnectionService.ReadStringPayload(responseBody);

                        Notifications.Show("Version", Version, NotificationKind.Info);

                        WriteLog(Version);
                    }
                    catch (Exception e)
                    {
                        WriteLog(e.Message);
                        Notifications.Show("Error", e.Message.ToShortString(), NotificationKind.Error);
                    }
                });
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRunRequest))]
        private async Task CloseConnectionAsync()
        {
            await RunRequestAsync(async () =>
                {
                    try
                    {
                        await ConnectionService.SendRequestAsync(ServerIp, ConnectionService.CloseAction);
                        Notifications.Show("Success", "Connection was closed", NotificationKind.Success);
                        Version = string.Empty;
                        WriteLog("Connection was closed");
                    }
                    catch (Exception e)
                    {
                        WriteLog(e.Message);
                        Notifications.Show("Error", e.Message.ToShortString(), NotificationKind.Error);
                    }
                });
        }

        private async Task RunRequestAsync(Func<Task> action)
        {
            IsRequestInProgress = true;

            try
            {
                await action();
            }
            finally
            {
                IsRequestInProgress = false;
            }
        }

        private void WriteLog(string message)
        {
            Log += $"{message}\n=======================================\n\n\n";
        }
    }
}
