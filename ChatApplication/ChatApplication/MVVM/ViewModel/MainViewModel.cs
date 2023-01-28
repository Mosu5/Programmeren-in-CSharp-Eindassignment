using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ChatApplication.Core;
using ChatApplication.FileIO;
using ChatApplication.MVVM.Model;
using ChatApplication.Net;

namespace ChatApplication.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    /* Commands */
    public RelayCommand ConnectToServerCommand { get; set; }
    public RelayCommand SendMessageCommand { get; set; }
    public RelayCommand SaveChatlogCommand { get; set; }
    public RelayCommand LoadChatlogCommand { get; set; }

    /* Properties */
    public ObservableCollection<UserModel> Users { get; set; }
    public ObservableCollection<string> Messages { get; set; }
    public string Username { get; set; }

    private string _message;

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    private Server _server;

    public MainViewModel()
    {
        Users = new ObservableCollection<UserModel>();
        Messages = new ObservableCollection<string>();

        _server = new Server();
        _server.ConnectedEvent += UserConnected;
        _server.MessageReceivedEvent += MessageReceived;
        _server.UserDisconnectedEvent += RemoveUser;

        ConnectToServerCommand =
            new RelayCommand(
                o =>
                {
                    try
                    {
                        _server.ConnectToServer(Username);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Could not connect to server. Try restarting the application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                },
                o => !string.IsNullOrEmpty(Username)
            );

        SendMessageCommand =
            new RelayCommand(
                o =>
                {
                    try
                    {
                        _server.SendMessage(Message);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(
                            "You are not connected to the server",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                    Message = String.Empty;
                },
                o => !string.IsNullOrEmpty(Message)
            );

        SaveChatlogCommand =
            new RelayCommand(
                o =>
                {
                    var fileHandler = new FileHandler();
                    fileHandler.WriteToFileAsync(Messages);
                },
                o => Messages.Count > 0
            );
        LoadChatlogCommand = new RelayCommand(
            // displays a messagebox to warn the user, if they cancel do nothing, if they confirm load the chatlog
            async o =>
            {
                var result = WarnUserChatlogOverwrite();
                if (result == MessageBoxResult.Yes)
                {
                    Disconnect();
                    var fileHandler = new FileHandler();
                    var chatlog = await fileHandler.ReadFromFileAsync();
                    if (chatlog.Count == 0)
                    {
                        return;
                    }
                    Messages.Clear();
                    foreach (var line in chatlog)
                    {
                        Application.Current.Dispatcher.Invoke(() => Messages.Add(line));
                    }
                }
            },
            o => true
        );
    }

    private MessageBoxResult WarnUserChatlogOverwrite()
    {
        return MessageBox.Show(
            "This will overwrite your current chatlog and disconnect u from the server, are you sure you want to continue?",
            "Warning",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning
        );
    }
    
    private void Disconnect()
    {
        _server.Disconnect();
        Users.Clear();
        Username = String.Empty;
        MessageBox.Show(
            "You have been disconnected from the server",
            "Disconnected",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
    }

    private void RemoveUser()
    {
        var uid = _server.PacketReader.ReadMessage();
        var user = Users.FirstOrDefault(x => x.UID == uid);
        Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
    }

    private void MessageReceived()
    {
        var msg = _server.PacketReader.ReadMessage();
        Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
    }

    private void UserConnected()
    {
        var user = new UserModel
        {
            Username = _server.PacketReader.ReadMessage(),
            UID = _server.PacketReader.ReadMessage(),
        };
        
        if (Users.All(x => x.UID != user.UID))
        {
            Application.Current.Dispatcher.Invoke(() => Users.Add(user));
        }
    }
}