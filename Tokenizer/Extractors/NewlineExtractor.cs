using Tokenizer.TokenExtraction;

namespace Tokenizer.Extractors
{
    public class NewlineExtractor<TokenTypeT> : TokenExtractorBase<TokenTypeT>
        where TokenTypeT : Enum
    {

        public NewlineExtractor()
        {

        }

        public override bool IsMatch(Stack<char> input)
        {
            throw new NotImplementedException();
        }

        public override Token<TokenTypeT> TryExtractToken(TokenTypeT tokenType)
        {
            Curser.line++;
            throw new NotImplementedException();
        }
    }
}
