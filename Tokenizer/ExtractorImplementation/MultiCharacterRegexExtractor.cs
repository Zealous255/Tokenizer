using System.Text.RegularExpressions;
using Tokenizer.TokenExtraction;

namespace Tokenizer.Extractors
{
    public class MultiCharacterRegexExtractor<TokenTypeT> : MultiCharacterExtractorBase<TokenTypeT>
        where TokenTypeT : Enum
    {
        private Regex _matchCharacterConstraint { get; set; }
        private Regex _regexMatch { get; }

        public MultiCharacterRegexExtractor(string matchPredicate) : base(matchPredicate)
        {
            _regexMatch = new Regex(_matchPredicate);
            _matchCharacterConstraint = SetMatchConstraint(matchPredicate);
        }

        public override Token<TokenTypeT> TryExtractToken(TokenTypeT tokenType)
        {
            Token<TokenTypeT> token = base.TryExtractToken(tokenType);

            Curser.position += token.Text.Length;

            return token;
        }

        protected override bool BufferMatch()
        {
            return _regexMatch.IsMatch(Buffer);
        }

        protected override bool IsMatchCharset(char current)
        {
            return _matchCharacterConstraint.IsMatch(current.ToString());
        }

        /// <summary>
        /// Sets up a 'Match Constraint' that includes any characters used in a 
        /// string literal predicate
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        private Regex SetMatchConstraint(string regex)
        { 
            if (regex.Trim().StartsWith("^") && regex.Trim().EndsWith("$"))
            {
                return new Regex(ConstructMatchConstraint(regex));
            }

            return new Regex(@"\w");
        }

        /// <summary>
        /// Deconstructs the provided Regex and rebuilds it to include any characters 
        /// used in the provided string
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        private string ConstructMatchConstraint(string regex)
        {
            return regex.Replace("^", "[").Replace("$", "]");
        }

    }
}
