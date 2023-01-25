using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TomatoLib
{
    public class TextContainerContainer<T> : TextContainer where T : TextContainer
    {
        public ObservableCollection<T> TextContainerList { get; set; } = new ObservableCollection<T>();
    }
}