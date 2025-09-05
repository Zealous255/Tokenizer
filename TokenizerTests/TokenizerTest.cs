using System.Text.RegularExpressions;
using Tokenizer;
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
        Whitespace,
        Tab
    }


    public class FooTokenizer : TokenizerBase<FooTokenType> { 
    
        public FooTokenizer()
        {

        }

        public override void ConifgureTokenDefinitions()
        {
            RegisterTokenDefinition(new TokenDefinition<FooTokenType>(FooTokenType.Joe,new MultiCharacterRegexExtractor<FooTokenType>(@"^Joe$")));
            RegisterTokenDefinition(new TokenDefinition<FooTokenType>(FooTokenType.Mamma, new MultiCharacterExtractor<FooTokenType>(@"++")));
            RegisterTokenDefinition(new TokenDefinition<FooTokenType>(FooTokenType.Dugg, new SingleCharacterRegexExtractor<FooTokenType>(@"\+")));
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
            List<Token<FooTokenType>> tokens = tokenizer.Tokenize("Joe");

            // assert
            Assert.IsTrue(tokens[0].TokenType == FooTokenType.Joe);
        }

        [TestMethod]
        public void Tokenizer_should_tokenize_strings_of_multiple_characters_when_configured_with_REGEX_into_a_single_token_and_tokenize_single_characters()
        {
            // arrange
            FooTokenizer tokenizer = new FooTokenizer();

            // act
            List<Token<FooTokenType>> tokens = tokenizer.Tokenize("Joe+");

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
    }
}
