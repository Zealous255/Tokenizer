using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer.TokenExtraction
{
    public interface ITokenExtractor<TokenTypeT> where TokenTypeT : Enum
    {
        public int Position { get; set; }
        public int LineNumber { get; set; }
    }
}
