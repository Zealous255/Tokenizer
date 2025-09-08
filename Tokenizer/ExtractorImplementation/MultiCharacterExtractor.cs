using Tokenizer.TokenExtraction;

namespace Tokenizer.Extractors
{
    public class MultiCharacterExtractor<TokenTypeT> : MultiCharacterExtractorBase<TokenTypeT>
        where TokenTypeT : Enum
    {
        private char[] _charSet;

        public MultiCharacterExtractor(string matchPredicate) : base(matchPredicate)
        {
            _charSet = _matchPredicate.ToCharArray();
        }

        public override Token<TokenTypeT> TryExtractToken(TokenTypeT tokenType)
        {
            Token<TokenTypeT> token = base.TryExtractToken(tokenType);

            Curser.position += token.Text.Length;

            return token;
        }

        protected override bool BufferMatch()
        {
            return _matchPredicate.Equals(Buffer);
        }

        protected override bool IsMatchCharset(char current)
        {
            foreach (char c in _charSet)
            {
                if (c == current)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
