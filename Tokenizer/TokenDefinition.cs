using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tokenizer.TokenExtraction;

namespace Tokenizer
{
    public class TokenDefinition<TokenTypeT> where TokenTypeT : Enum
    {
        public TokenExtractorBase<TokenTypeT> Extractor { get; }
        public TokenTypeT TokenType {  get; }

        public TokenDefinition(TokenTypeT tokenType, TokenExtractorBase<TokenTypeT> extractor)
        {
            TokenType = tokenType;
            Extractor = extractor;
        }
    }
}
