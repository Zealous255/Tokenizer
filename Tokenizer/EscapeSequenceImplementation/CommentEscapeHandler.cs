using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tokenizer.TokenExtraction;
using Tokenizer.TokenExtraction.Generic;

namespace Tokenizer.EscapeSequenceImplementation
{
    public class CommentEscapeHandler : IDiscardSequence
    {

        private string _beginningSequence { get; } = string.Empty;

        private string _endingSequence { get; } = string.Empty;

        private Regex? _startConstraint;

        private Regex? _endConstraint;

        private string _buffer = string.Empty;



        public CommentEscapeHandler(string beginningSequence, string endingSequence)
        {
            _beginningSequence = beginningSequence;
            _endingSequence = endingSequence;
        }


        public void ProcessDiscardSequence(Stack<char> characterStack)
        {
            Regex beginRegex = new Regex(_beginningSequence);
            Regex endRegex = new Regex(_endingSequence);

            _startConstraint = TryConstractMatchConstraint(_beginningSequence) ?? null;
            _endConstraint = TryConstractMatchConstraint(_endingSequence) ?? null;

            char current = characterStack.Peek();


            // Check for start constraint
            if (_startConstraint != null)
            {
                while (IsMatchConstraint(current, _startConstraint) && characterStack.Any())
                {
                    current = characterStack.Pop();
                    _buffer = string.Concat(_buffer, current);
                    Curser.position++;

                    if (beginRegex.IsMatch(_buffer))
                    {
                        ConsumeIgnoredText(characterStack, characterStack.Pop(), endRegex);
                        break;
                    }
                }
            } else
            {
                if (beginRegex.IsMatch(current.ToString()) && characterStack.Any()) {

                    characterStack.Pop();

                    ConsumeIgnoredText(characterStack, current, endRegex);
                }
            }
        }


        private void ConsumeIgnoredText(Stack<char> characterStack, char current, Regex endRegex)
        {
            if (_endConstraint != null)
            {
                while (characterStack.Any() && !IsMatchConstraint(current, _endConstraint))
                {
                    current = characterStack.Pop();

                    Curser.position++;

                    if (Regex.IsMatch(current.ToString(), @"\n"))
                        Curser.NewLine();
                    
                }

                // After the loop as finished, we should have hit our match constraint. At this point 
                // we can match the ending comment and go back to our original parsing
                _buffer = string.Empty;

                while (characterStack.Any() && IsMatchConstraint(current, _endConstraint) && !(Regex.IsMatch(current.ToString(), @"\s")))
                {
                    _buffer = string.Concat(_buffer, current);

                    Curser.position++;

                    if (endRegex.IsMatch(_buffer))
                    {
                        _buffer = string.Empty;
                        break;
                    }

                    current = characterStack.Pop();
                }
            }
            else
            {
                while (characterStack.Any() && !endRegex.IsMatch(current.ToString()))
                {
                    current = characterStack.Pop();

                    Curser.position++;

                    if (Regex.IsMatch(current.ToString(), @"\n"))
                        Curser.NewLine();

                    if (current.Equals(_endingSequence))
                        break;
                }
            }
        }

        private bool IsMatchConstraint(char current, Regex constraint)
        {
            if (constraint.IsMatch(current.ToString()))
            {
                return true;
            }

            return false;
        }

        private Regex? TryConstractMatchConstraint(string targetSequence)
        {
            if (targetSequence.StartsWith('(') && targetSequence.EndsWith(')')) {
                return new Regex(targetSequence.Replace('(', '[').Replace(')', ']'));
            }
            if (targetSequence.StartsWith('^') && targetSequence.EndsWith('$'))
            {
                return new Regex(targetSequence.Replace('^', '[').Replace('$', ']'));
            }

            return null;
        }

    }
}
