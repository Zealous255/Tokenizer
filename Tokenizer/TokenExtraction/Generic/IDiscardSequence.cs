using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer.TokenExtraction.Generic
{
    public interface IDiscardSequence
    {

        public bool ProcessedEscapeSequence(Stack<char> characterStack);
    }
}
