using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ChatApplication.Core;
using ChatApplication.MVVM.Model;
using ChatApplication.Net;

namespace ChatApplication.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    /* Commands */
    public RelayCommand ConnectToServerCommand { get; set; }
    public RelayCommand SendMessageCommand { get; set; }

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
                o => _server.ConnectToServer(Username),
                o => !string.IsNullOrEmpty(Username)
            );

        SendMessageCommand =
            new RelayCommand(
                o =>
                {
                    _server.SendMessage(Message);
                    Message = String.Empty;

                },
                o => !string.IsNullOrEmpty(Message)
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