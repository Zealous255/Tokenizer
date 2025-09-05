namespace Tokenizer.TokenExtraction
{
    public interface ITokenExtractor<TokenTypeT> where TokenTypeT : Enum
    {
        public int Position { get; set; }
        public int LineNumber { get; set; }
    }
}
