using Tokenizer;
using Tokenizer.EscapeSequenceImplementation;
using Tokenizer.Extractors;
using Tokenizer.TokenExtraction;

namespace TokenizerTests
{
    public enum FooTokenType { 
        Joe,
        Mamma,
        Dugg,
        Ditches,
        FizzBuzz,
        BuzzFizz,
        Newline,
        Whitespace,
        Tab
    }


    public class FooTokenizer : TokenizerBase<FooTokenType> { 
    
        public FooTokenizer()
        {

        }

        public override void ConfigureTokenDefinitions()
        {
            RegisterTokenDefinition(new TokenDefinition<FooTokenType>(FooTokenType.Joe, new MultiCharacterRegexExtractor<FooTokenType>(@"^Joe$")));
            RegisterTokenDefinition(new TokenDefinition<FooTokenType>(FooTokenType.Mamma, new MultiCharacterExtractor<FooTokenType>(@"++")));
            RegisterTokenDefinition(new TokenDefinition<FooTokenType>(FooTokenType.Dugg, new SingleCharacterRegexExtractor<FooTokenType>(@"\+")));

            RegisterEscapeSequence(new EscapeSquenceDefinition(new NewlineDiscardHandler()));
            RegisterEscapeSequence(new EscapeSquenceDefinition(new CommentEscapeHandler(@"(\/\*)", @"(\*\/)")));
            RegisterEscapeSequence(new EscapeSquenceDefinition(new CommentEscapeHandler(@"(\/\/)", @"\n")));
        }
    }



    [TestClass]
    public sealed class TokenizerTest
    {
        [TestMethod]
        public void Tokenizer_should_tokenize_strings_of_multiple_characters_into_a_single_token()
        {
            // arrange
            FooTokenizer tokenizer = new FooTokenizer();

            // act
            List<Token<FooTokenType>> tokens = tokenizer.Tokenize("Joe ");

            // assert
            Assert.IsTrue(tokens[0].TokenType == FooTokenType.Joe);
        }

        [TestMethod]
        public void Tokenizer_should_tokenize_strings_of_multiple_characters_when_configured_with_REGEX_into_a_single_token_and_tokenize_single_characters()
        {
            // arrange
            FooTokenizer tokenizer = new FooTokenizer();

            // act
            List<Token<FooTokenType>> tokens = tokenizer.Tokenize("Joe +");

            // assert
            Assert.IsTrue(tokens[0].TokenType == FooTokenType.Joe);
            Assert.IsTrue(tokens[1].TokenType == FooTokenType.Dugg);
        }

        [TestMethod]
        public void Tokenizer_should_tokenize_strings_of_multiple_characters_into_a_single_token_and_tokenize_single_characters()
        {
            // arrange
            FooTokenizer tokenizer = new FooTokenizer();

            // act
            List<Token<FooTokenType>> tokens = tokenizer.Tokenize("++");

            // assert
            Assert.IsTrue(tokens[0].TokenType == FooTokenType.Mamma);
        }

        [TestMethod]
        public void Tokenizer_should_tokenize_strings_of_multiple_characters_into_a_single_token_and_tokenize_single_characters_when_appropriate()
        {
            // arrange
            FooTokenizer tokenizer = new FooTokenizer();

            // act
            List<Token<FooTokenType>> tokens = tokenizer.Tokenize("+++");

            // assert
            Assert.IsTrue(tokens[0].TokenType == FooTokenType.Mamma);
            Assert.IsTrue(tokens[1].TokenType == FooTokenType.Dugg);
        }

        [TestMethod]
        public void Tokenizer_should_extract_newline_characters_and_track_line_number_of_detected_token()
        {
            // arrange
            FooTokenizer tokenizer = new FooTokenizer();

            // act
            List<Token<FooTokenType>> tokens = tokenizer.Tokenize("Joe+++\r\n+Joe+\r\nJoe");
            List<Token<FooTokenType>> joeTokens = tokens.Where(x => x.Text.Equals("Joe")).ToList();

            // assert
            Assert.IsTrue(joeTokens[0].Line == 1);
            Assert.IsTrue(joeTokens[1].Line == 2);
            Assert.IsTrue(joeTokens[2].Line == 3);
        }

        [TestMethod]
        public void Tokenizer_should_extract_ignore_the_contents_of_block_comments_while_advancing_the_curser()
        {
            // arrange
            FooTokenizer tokenizer = new FooTokenizer();

            // act
            List<Token<FooTokenType>> tokens = tokenizer.Tokenize("/* Joe+++\r\n+Joe+\r\nJoe */");

            // assert
            Assert.IsTrue(tokens.Count() == 0);
        }

        [TestMethod]
        public void Tokenizer_should_extract_ignore_the_contents_of_line_comments_while_advancing_the_curser()
        {
            // arrange
            FooTokenizer tokenizer = new FooTokenizer();

            // act
            List<Token<FooTokenType>> tokens = tokenizer.Tokenize("// Joe+++\r\n//+Joe+\r\n//Joe \n");

            // assert
            Assert.IsTrue(tokens.Count() == 0);
        }
    }
}
