namespace TomatoLib
{
    /**
     * Base class for textComponents (such as TextChapter and TextSection).
     */
    public class TextContainer
    {
        public ushort Identification { get; set; } 
        public string Name { get; set; }
        public string Text { get; set; }
    }
}