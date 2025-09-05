using System.Text.RegularExpressions;
using Tokenizer.TokenExtraction;

namespace Tokenizer.Extractors
{
    public class NewlineExtractor<TokenTypeT> : MultiCharacterExtractorBase<TokenTypeT>
        where TokenTypeT : Enum
    {

        private Regex _regexMatch { get; }

        public NewlineExtractor(string matchPredicate) : base(matchPredicate) {
            _regexMatch = new Regex(_matchPredicate);
        }

        protected override bool BufferMatch()
        {
            return _regexMatch.IsMatch(Buffer);
        }

        protected override bool IsMatchExtension(char current)
        {
            if (Regex.IsMatch(current.ToString(), @"(\r\n|\r|\n)"))
            {
                return true;
            }

            return false;
        }

        public override Token<TokenTypeT> TryExtractToken(TokenTypeT tokenType)
        {
            Token<TokenTypeT> token = base.TryExtractToken(tokenType);

            Curser.NewLine();

            return token;
        }
    }
}
