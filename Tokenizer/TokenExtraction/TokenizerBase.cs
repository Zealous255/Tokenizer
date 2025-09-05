namespace Tokenizer.TokenExtraction
{
    internal static class Curser
    {
        public static int position { get; set; }
        public static int line { get; set; }
    }

    public abstract class TokenizerBase<TokenTypeT> where TokenTypeT : Enum
    {
        private Stack<char> _tokenStack = new Stack<char>();
        private List<TokenDefinition<TokenTypeT>> _tokenDefinitions = new List<TokenDefinition<TokenTypeT>>();

        private string _originalInputString = string.Empty;
        private int _inputLength;

        public TokenizerBase()
        {
            ConifgureTokenDefinitions();
        }

        public abstract void ConifgureTokenDefinitions();

        public void RegisterTokenDefinition(TokenDefinition<TokenTypeT> tokenDefinition)
        {
            _tokenDefinitions.Add(tokenDefinition);
        }

        public List<Token<TokenTypeT>> Tokenize(string inputString)
        {
            List<Token<TokenTypeT>> tokens = new List<Token<TokenTypeT>>();

            Curser.line = 1;

            PrepareCharacterStack(inputString);

            _originalInputString = inputString;
            _inputLength = _originalInputString.Length;

            while (_tokenStack.Any())
            {
                tokens.Add(FindTokenMatch(_tokenStack));
            }

            return tokens;
        }

        private Token<TokenTypeT> FindTokenMatch(Stack<char> tokenStack)
        {
            Token<TokenTypeT> returnToken = null;

            for (int i = 0; i < _tokenDefinitions.Count; i++)
            {
                if (_tokenDefinitions[i].Extractor.IsMatch(tokenStack))
                {
                    returnToken = _tokenDefinitions[i].Extractor.TryExtractToken(_tokenDefinitions[i].TokenType);
                    break;
                }

            }

            return returnToken;
        }

        public void AdvanceCurserPosition(int amount)
        {
            Curser.position += amount;
        }

        public void CurserNewline()
        {
            Curser.position = 0;
            Curser.line++;
        }

        private void PrepareCharacterStack(string inputString)
        {
            for (int i = inputString.Length - 1; i >= 0; i--)
            {
                _tokenStack.Push(inputString[i]);
            }
        }

    }
}
