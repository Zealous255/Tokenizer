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

        protected override bool BufferMatch()
        {
            return _matchPredicate.Equals(Buffer);
        }

        protected override bool IsMatchExtension(char current)
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
