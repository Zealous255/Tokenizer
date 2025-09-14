using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tokenizer.TokenExtraction;
using Tokenizer.TokenExtraction.Generic;

namespace Tokenizer.EscapeSequenceImplementation
{
    public class NewlineDiscardHandler : IDiscardSequence
    {
        public NewlineDiscardHandler()
        {
        }

        public bool ProcessedEscapeSequence(Stack<char> characterStack)
        {
            char current = characterStack.Peek();

            if (Regex.IsMatch(current.ToString(), @"\r"))
            {
                characterStack.Pop();

                current = characterStack.Peek();

                if (Regex.IsMatch(current.ToString(), @"\n"))
                {
                    characterStack.Pop();

                    Curser.NewLine();

                    return true;
                }
            }
            else if (Regex.IsMatch(current.ToString(), @"\n"))
            {
                characterStack.Pop();

                Curser.NewLine();

                return true;
            }

            return false;
        }
    }
}
