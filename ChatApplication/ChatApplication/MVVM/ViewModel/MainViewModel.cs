using System;
using System.Collections.ObjectModel;
using ChatApplication.Core;
using ChatApplication.MVVM.Model;

namespace ChatApplication.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public ObservableCollection<MessageModel> Messages { get; set; }
    public ObservableCollection<ContactModel> Contacts { get; set; }
    
    /* Commands */
    public RelayCommand SendCommand { get; set; }

    private ContactModel _selectedContact;
    public ContactModel SelectedContact
    {
        get => _selectedContact;
        set
        {
            _selectedContact = value;
            OnPropertyChanged();
        }
    }
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
    

    public MainViewModel()
    {
        Messages = new ObservableCollection<MessageModel>();
        Contacts = new ObservableCollection<ContactModel>();

        SendCommand = new RelayCommand(o =>
        {
            Messages.Add(new MessageModel
            {
                Message = Message,
                FirstMessage = false
            });
            
            Message = "";
            
        });

        Messages.Add(new MessageModel
        {
            Username = "Allison",
            UsernameColor = "#409aff",
            ImageSource = "https://i.imgur.com/yMWvLXd.png",
            Message = "Hello, how are you?",
            Time = DateTime.Now,
            IsNativeOrigin = false,
            FirstMessage = true
        });

        for (int i = 0; i < 4; i++)
        {
            Messages.Add(new MessageModel
            {
                Username = "Bunny",
                UsernameColor = "#409aff",
                ImageSource = "https://i.imgur.com/yMWvLXd.png",
                Message = "Test",
                Time = DateTime.Now,
                IsNativeOrigin = true,
            });


            Messages.Add(new MessageModel
            {
                Username = "Allison",
                UsernameColor = "#409aff",
                ImageSource = "https://i.imgur.com/yMWvLXd.png",
                Message = "Last",
                Time = DateTime.Now,
                IsNativeOrigin = true,
            });
        }

        for (int j = 0; j < 5; j++)
        {
            Contacts.Add(new ContactModel
            {
                Username = $"Allison {j}",
                ImageSource = "https://i.imgur.com/yMWvLXd.png",
                Messages = Messages
            });
        }
    }
}