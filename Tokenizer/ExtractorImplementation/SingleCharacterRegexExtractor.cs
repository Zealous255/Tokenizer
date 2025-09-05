using System.Text.RegularExpressions;
using Tokenizer.TokenExtraction;

namespace Tokenizer.Extractors
{
    public class SingleCharacterRegexExtractor<TokenTypeT> : TokenExtractorBase<TokenTypeT>
        where TokenTypeT : Enum
    {
        private char _buffer;

        public Regex Match { get; }

        public SingleCharacterRegexExtractor(string match)
        {
            Match = new Regex(match);
        }

        public override bool IsMatch(Stack<char> input)
        {
            _buffer = input.Peek();

            if (Match.IsMatch(_buffer.ToString()))
            {
                input.Pop();

                return true;
            }

            return false;
        }

        public override Token<TokenTypeT> TryExtractToken(TokenTypeT tokenType)
        {
            Token<TokenTypeT> token = new Token<TokenTypeT>(Curser.position, Curser.line, 1, _buffer.ToString(), tokenType);

            Curser.position++;

            return token;
        }
    }
}
