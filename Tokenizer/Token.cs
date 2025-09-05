namespace Tokenizer
{
    public class Token<TokenTypeT> where TokenTypeT : Enum
    {
        public int Pos { get; }
        public int Line { get; }
        public int Length { get; }
        public string Text { get; }
        public TokenTypeT TokenType { get; }

        public Token(int pos, int line, int length, string text, TokenTypeT tokenType)
        {
            Pos = pos;
            Line = line;
            Length = length;
            Text = text;
            TokenType = tokenType;
        }


        public override string ToString()
        {
            return $"Type: {TokenType.ToString()} Value: {Text} Line: {Line} Pos: {Pos} Length: {Length}";
        }
    }
}
