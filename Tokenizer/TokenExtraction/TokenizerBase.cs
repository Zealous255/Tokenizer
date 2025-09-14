using System.Text.RegularExpressions;

namespace Tokenizer.TokenExtraction
{
    internal static class Curser
    {
        public static int position { get; set; } = 1;
        public static int line { get; set; } = 1;

        public static void NewLine()
        {
            position = 1;
            line++;
        }

        public static void ResetCurser()
        {
            line = 1;
            position = 1;
        }
    }

    public abstract class TokenizerBase<TokenTypeT> where TokenTypeT : Enum
    {
        private Stack<char> _tokenStack = new Stack<char>();
        private List<TokenDefinition<TokenTypeT>> _tokenDefinitions = new List<TokenDefinition<TokenTypeT>>();
        private List<EscapeSquenceDefinition> _escapeSequenceDefintiions = new List<EscapeSquenceDefinition>();

        private string _originalInputString = string.Empty;

        public int InputLength { get {  return _originalInputString.Length; } }
        public string OriginalInputString { get; }

        public TokenizerBase()
        {
            ConfigureTokenDefinitions();
        }

        public abstract void ConfigureTokenDefinitions();

        public void RegisterTokenDefinition(TokenDefinition<TokenTypeT> tokenDefinition)
        {
            _tokenDefinitions.Add(tokenDefinition);
        }

        public void RegisterEscapeSequence(EscapeSquenceDefinition escapeSequenceDefinition)
        {
            _escapeSequenceDefintiions.Add(escapeSequenceDefinition);
        }

        public List<Token<TokenTypeT>> Tokenize(string inputString)
        {
            Curser.ResetCurser();

            List<Token<TokenTypeT>> tokens = new List<Token<TokenTypeT>>();

            PrepareCharacterStack(inputString);

            _originalInputString = inputString;

            while (_tokenStack.Any())
            {
                if (IsEmptySpace(_tokenStack.Peek()))
                {
                    _tokenStack.Pop();
                } else
                {
                    bool matchedEscapeSequence = ProcessedEscapeSequence();

                    // Continue until we either run out of tokens or have processed all the 
                    // escape sequences that still stand in our way
                    while (matchedEscapeSequence)
                    {
                        matchedEscapeSequence = ProcessedEscapeSequence();
                    } 

                    if (_tokenStack.Any())
                        tokens.Add(FindTokenMatch(_tokenStack));
                }

            }

            return tokens;
        }

        protected virtual bool IsEmptySpace(char current)
        {
            return (Regex.Match(current.ToString(), @"([ ]|\t)").Success);
        }

        private bool ProcessedEscapeSequence()
        {
            foreach (EscapeSquenceDefinition escapeSquenceDefinition in _escapeSequenceDefintiions)
            {
                if (_tokenStack.Any())
                {
                    if (escapeSquenceDefinition.EscapeSequence.ProcessedEscapeSequence(_tokenStack))
                    {
                        return true;
                    }
                    
                } else
                {
                    break;
                }
            }

            return false;
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

        private void PrepareCharacterStack(string inputString)
        {
            for (int i = inputString.Length - 1; i >= 0; i--)
            {
                _tokenStack.Push(inputString[i]);
            }
        }

    }
}
