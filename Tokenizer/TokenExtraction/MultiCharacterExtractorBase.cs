namespace Tokenizer.TokenExtraction
{
    public abstract class MultiCharacterExtractorBase<TokenTypeT> : TokenExtractorBase<TokenTypeT> where TokenTypeT : Enum
    {
        protected string Buffer { get; set; } = string.Empty;

        protected string _matchPredicate { get; } = string.Empty;

        public MultiCharacterExtractorBase(string matchPredicate) : base() { 
            _matchPredicate = matchPredicate;
        } 

        public override bool IsMatch(Stack<char> input)
        {
            char current = input.Peek();

            if (!IsMatchExtension(current))
            {
                return false;
            }

            Buffer = input.Pop().ToString();

            while (input.Any() && IsMatchExtension(current))
            {
                Buffer = string.Concat(Buffer, input.Pop());

                if (BufferMatch())
                {
                    return true;
                }

            }

            input = RewindStack(input);

            return false;
        }

        public override Token<TokenTypeT> TryExtractToken(TokenTypeT tokenType)
        {
            Token<TokenTypeT> returnToken = new Token<TokenTypeT>(Curser.position, Curser.line, Buffer.Length, Buffer, tokenType);

            Curser.position += Buffer.Length;

            Buffer = string.Empty;

            return returnToken;
        }

        /// <summary>
        /// Checks that the character buffer matches the predicate
        /// </summary>
        /// <returns></returns>
        protected abstract bool BufferMatch();

        /// <summary>
        /// Extended matching for checking other factors besides a 1x1 match with 
        /// a REGEX or with an exact string or char
        /// </summary>
        /// <param name="current">Current character from the stack</param>
        /// <returns></returns>
        protected abstract bool IsMatchExtension(char current);

        /// <summary>
        /// Rewinds the character stack if the multi character extractor cannot match 
        /// the regex
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual Stack<char> RewindStack(Stack<char> input)
        {
            for (int i = Buffer.Length - 1; i >= 0; i--)
            {
                input.Push(Buffer[i]);
            }

            return input;
        }
    }
}
