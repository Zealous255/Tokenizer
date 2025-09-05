namespace Tokenizer.TokenExtraction
{
    public abstract class TokenExtractorBase<TokenTypeT> : ITokenExtractor<TokenTypeT>
        where TokenTypeT : Enum
    {
        public int Position { get; set; }
        public int LineNumber { get; set; }

        public abstract bool IsMatch(Stack<char> input);

        public abstract Token<TokenTypeT> TryExtractToken(TokenTypeT tokenType);
    }
}
