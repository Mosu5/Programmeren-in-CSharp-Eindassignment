using System;
using System.Collections.ObjectModel;
using ChatApplication.MVVM.Model;

namespace ChatApplication.MVVM.ViewModel;

public class MainViewModel
{
    public ObservableCollection<MessageModel> Messages { get; set; }
    public ObservableCollection<ContactModel> Contacts { get; set; }

    public MainViewModel()
    {
        Messages = new ObservableCollection<MessageModel>();
        Contacts = new ObservableCollection<ContactModel>();


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