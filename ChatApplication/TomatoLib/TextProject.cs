using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TomatoLib
{
    public class TextProject
    {
        public string Name { get; set; }

        public ObservableCollection<TextChapter> ChaptersList { get; set; } = new ObservableCollection<TextChapter>();

        public string FilePath { get; set; }

        //TODO: Replace this with UniqueIdentifier, since we can reuse identifiers once sections have been deleted.
        public ushort HighestIdentifier
        {
            get
            {
                ushort highestUshort = 0;
                foreach (var textChapter in ChaptersList)
                {
                    if (textChapter.Identification > highestUshort)
                    {
                        highestUshort = textChapter.Identification;
                    }

                    foreach (TextContainer textSection in textChapter.TextContainerList)
                    {
                        if (textSection.Identification > highestUshort)
                        {
                            highestUshort = textSection.Identification;
                        }
                    }
                }

                return highestUshort;
            }
        }
    }
}