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
        private string _beginningSequence { get;  } = string.Empty;
        private string _endingSequence { get; } = string.Empty;
        private string _buffer = string.Empty;



        public CommentEscapeHandler(string beginningSequence, string endingSequence)
        {
            _beginningSequence = beginningSequence;
            _endingSequence = endingSequence;
        }


        public bool ProcessedEscapeSequence(Stack<char> characterStack)
        {
            char current = characterStack.Peek();

            _buffer = string.Empty;

            if (!PopulateBufferByMatchConstraint(current, characterStack))
            {
                characterStack = RewindStack(characterStack);

                return false;
            }
           

            _buffer = string.Empty;

            current = characterStack.Peek();

            Regex? endingConstraint = TryConstructMatchConstraint(_endingSequence);

            while (!(IsMatchCharacterset(current, _endingSequence) || IsMatchRegexConstraint(current, endingConstraint))) {

                if (current != '\r')
                    Curser.position++;

                current = characterStack.Pop();

                if (current == '\n')
                    Curser.NewLine();
            }


            FinalizeMatch(current, characterStack);

            return true;
        }

        private void FinalizeMatch(char current, Stack<char> characterStack)
        {
            Regex? endingConstraint = TryConstructMatchConstraint(_endingSequence);

            while (IsMatchCharacterset(current, _endingSequence) || IsMatchRegexConstraint(current, endingConstraint))
            {
                _buffer = string.Concat(_buffer, current);

                if (!characterStack.Any() || IsExactMatch(_buffer, _endingSequence))
                    break;

                current = characterStack.Pop();

                Curser.position++;
            }

            _buffer = string.Empty;
        }

        private bool PopulateBufferByMatchConstraint(char current, Stack<char> characterStack)
        {
            Regex? beginningConstraint = TryConstructMatchConstraint(_beginningSequence);

            current = characterStack.Pop();
            _buffer = current.ToString();

            while (IsMatchCharacterset(current, _beginningSequence) || IsMatchRegexConstraint(current, beginningConstraint))
            {
                Curser.position++;

                _buffer = string.Concat(_buffer, current);

                current = characterStack.Pop();

                if (IsExactMatch(_buffer, _beginningSequence))
                    return true;
            }

            return false;
        }

        private bool IsExactMatch(string buffer, string predicate)
        {
            Regex predicateRegex = new Regex(predicate);

            if (predicate.Equals(buffer) || predicateRegex.IsMatch(buffer))
            {
                return true;
            }

            return false;
        }

        private bool IsMatchCharacterset(char current, string predicate)
        {
            char[] predicateChars = predicate.Replace(@"\", "").Replace(@"(","").Replace(@")", "").ToCharArray();

            foreach (char c in predicateChars)
            {
                if (current == c)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsMatchRegexConstraint(char current, Regex? constraint)
        {
            if (constraint == null) return false;

            if (constraint.IsMatch(current.ToString()))
            {
                return true;
            }

            return false;
        }

        private Regex? TryConstructMatchConstraint(string targetSequence)
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

        /// <summary>
        /// Rewinds the character stack if the multi character extractor cannot match 
        /// the regex
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual Stack<char> RewindStack(Stack<char> input)
        {
            for (int i = _buffer.Length-1; i >= 0; i--)
            {
                input.Push(_buffer[i]);
            }

            return input;
        }

    }
}
