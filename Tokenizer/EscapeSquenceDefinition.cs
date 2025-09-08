using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokenizer.TokenExtraction.Generic;

namespace Tokenizer
{
    public class EscapeSquenceDefinition
    {

        public IDiscardSequence EscapeSequence { get; }

        public EscapeSquenceDefinition(IDiscardSequence escapeSequence)
        {
            EscapeSequence = escapeSequence;
        }

    }
}
