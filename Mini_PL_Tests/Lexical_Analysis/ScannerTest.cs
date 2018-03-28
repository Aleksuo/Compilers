using System;
using Mini_PL.Lexical_Analysis;
using Mini_PL.Utils.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mini_PL_Tests
{
    [TestClass]
    public class ScannerTest
    {
        
        public void tokenRecognizedHelper(string input, Token expected)
        {
            StringSource source = new StringSource(input);
            Scanner scanner = new Scanner(source);
            Token result = scanner.nextToken();
            Assert.AreEqual(expected.getLexeme(), result.getLexeme());
            Assert.AreEqual(expected.getType(), result.getType());
        }

        //Operators

        [TestMethod]
        public void plusTokenIsRecognized()
        {
            tokenRecognizedHelper("+", new Token(TokenType.PLUS, "+"));
        }

        [TestMethod]
        public void minusTokenIsRecognized()
        {
            tokenRecognizedHelper("-", new Token(TokenType.MINUS, "-"));
        }

        [TestMethod]
        public void divTokenIsRecognized()
        {
            tokenRecognizedHelper("/", new Token(TokenType.DIV, "/"));
        }

        [TestMethod]
        public void multTokenIsRecognized()
        {
            tokenRecognizedHelper("*", new Token(TokenType.MULT, "*"));
        }

        [TestMethod]
        public void andTokenIsRecognized()
        {
            tokenRecognizedHelper("&", new Token(TokenType.AND, "&"));
        }

        [TestMethod]
        public void notTokenIsRecognized()
        {
            tokenRecognizedHelper("!", new Token(TokenType.NOT, "!"));
        }

        [TestMethod]
        public void equalsTokenIsRecognized()
        {
            tokenRecognizedHelper("=", new Token(TokenType.EQUALS, "="));
        }

        [TestMethod]
        public void lessthanTokenIsRecognized()
        {
            tokenRecognizedHelper("<", new Token(TokenType.LESSTHAN, "<"));
        }

        [TestMethod]
        public void assignTokenIsRecognized()
        {
            tokenRecognizedHelper(":=", new Token(TokenType.ASSIGN, ":="));
        }

        //Keywords

        [TestMethod]
        public void varTokenIsRecognized()
        {
            tokenRecognizedHelper("var", new Token(TokenType.VAR, "var"));
        }

        [TestMethod]
        public void forTokenIsRecognized()
        {
            tokenRecognizedHelper("for", new Token(TokenType.FOR, "for"));
        }

        [TestMethod]
        public void endTokenIsRecognized()
        {
            tokenRecognizedHelper("end", new Token(TokenType.END, "end"));
        }

        [TestMethod]
        public void inTokenIsRecognized()
        {
            tokenRecognizedHelper("in", new Token(TokenType.IN, "in"));
        }

        [TestMethod]
        public void doTokenIsRecognized()
        {
            tokenRecognizedHelper("do", new Token(TokenType.DO, "do"));
        }

        [TestMethod]
        public void readTokenIsRecognized()
        {
            tokenRecognizedHelper("read", new Token(TokenType.READ, "read"));
        }

        [TestMethod]
        public void printTokenIsRecognized()
        {
            tokenRecognizedHelper("print", new Token(TokenType.PRINT, "print"));
        }

        [TestMethod]
        public void intTypeTokenIsRecognized()
        {
            tokenRecognizedHelper("int", new Token(TokenType.TYPE, "int"));
        }

        [TestMethod]
        public void stringTypeTokenIsRecognized()
        {
            tokenRecognizedHelper("string", new Token(TokenType.TYPE, "string"));
        }

        [TestMethod]
        public void boolTypeTokenIsRecognized()
        {
            tokenRecognizedHelper("bool", new Token(TokenType.TYPE, "bool"));
        }

        [TestMethod]
        public void assertTokenIsRecognized()
        {
            tokenRecognizedHelper("assert", new Token(TokenType.ASSERT, "assert"));
        }

        //Other tokens

        [TestMethod]
        public void leftParenthesisTokenIsRecognized()
        {
            tokenRecognizedHelper("(", new Token(TokenType.LEFTPAREN, "("));
        }

        [TestMethod]
        public void rightParenthesisTokenIsRecognized()
        {
            tokenRecognizedHelper(")", new Token(TokenType.RIGHTPAREN, ")"));
        }

        [TestMethod]
        public void colonTokenIsRecognized()
        {
            tokenRecognizedHelper(":", new Token(TokenType.COLON, ":"));
        }

        [TestMethod]
        public void semicolonTokenIsRecognized()
        {
            tokenRecognizedHelper(";", new Token(TokenType.SEMICOLON, ";"));
        }

        [TestMethod]
        public void rangeTokenIsRecognized()
        {
            tokenRecognizedHelper("..", new Token(TokenType.RANGE, ".."));
        }

        //Numbers

        [TestMethod]
        public void numberTokenIsRecognized()
        {
            tokenRecognizedHelper("8", new Token(TokenType.INTEGER, "8"));
        }

        [TestMethod]
        public void numberTokenIsRecognized2()
        {
            tokenRecognizedHelper("87685902", new Token(TokenType.INTEGER, "87685902"));
        }

        [TestMethod]
        public void zerosAtTheBeginningProduceErrorToken()
        {
            tokenRecognizedHelper("01", new Token(TokenType.ERROR, "01"));
        }

        [TestMethod]
        public void zerosAtTheBeginningProduceErrorToken2()
        {
            tokenRecognizedHelper("000001345", new Token(TokenType.ERROR, "000001345"));
        }

        [TestMethod]
        public void zeroIsRecognized()
        {
            tokenRecognizedHelper("0", new Token(TokenType.INTEGER, "0"));
        }

        //identifiers
        [TestMethod]
        public void identifierTokenIsRecognized()
        {
            tokenRecognizedHelper("test", new Token(TokenType.ID, "test"));
        }

        [TestMethod]
        public void identifierTokenWithNumbersAtEndIsRecognized()
        {
            tokenRecognizedHelper("test123", new Token(TokenType.ID, "test123"));
        }

        
        [TestMethod]
        public void numbersInPrefixProduceErrorToken()
        {
            tokenRecognizedHelper("123test", new Token(TokenType.ERROR, "123test"));
        }
        

        //strings
        [TestMethod]
        public void stringTokenIsRecognized()
        {
            tokenRecognizedHelper("\"this is a test string\"", new Token(TokenType.STRING, "this is a test string"));
        }

        //whitespace and  new lines
        [TestMethod]
        public void whiteSpacesAreIgnored()
        {
            tokenRecognizedHelper("    1  ", new Token(TokenType.INTEGER, "1"));
        }

        [TestMethod]
        public void newLinesAreIgnored()
        {
            tokenRecognizedHelper("\n\n\n1\n", new Token(TokenType.INTEGER, "1"));
        }

        [TestMethod]
        public void whiteSpacesAndNewLinesAreIgnored()
        {
            tokenRecognizedHelper("\n  \n   1 \n      \n", new Token(TokenType.INTEGER, "1"));
        }

        [TestMethod]
        public void newLinesIncreaseLineCount()
        {
            StringSource source = new StringSource("\n\n\n1");
            Scanner scanner = new Scanner(source);
            Token result = scanner.nextToken();
            Assert.AreEqual(result.getLexeme(), "1");
            Assert.AreEqual(result.getType(), TokenType.INTEGER);
            Assert.AreEqual(scanner.getLineCount(), 4);
        }

        //comments
        [TestMethod]
        public void singleLineCommentsAreIgnored()
        {
            tokenRecognizedHelper("// this is a single line comment\n 1", new Token(TokenType.INTEGER, "1"));
        }

        [TestMethod]
        public void singleLineCommentsAreIgnored2()
        {
            tokenRecognizedHelper("1// this is a single line comment after a token ", new Token(TokenType.INTEGER, "1"));
        }

        [TestMethod]
        public void singleLineCommentsAreIgnored3()
        {
            tokenRecognizedHelper("// this is a 145234 &/ comment before a token\n 1 //this is a comment after the token\n", new Token(TokenType.INTEGER, "1"));
        }

        [TestMethod]
        public void multiLineCommentsAreIgnored()
        {
            tokenRecognizedHelper("/* this is a \n multi\n line comment */ 1", new Token(TokenType.INTEGER, "1"));
        }

        [TestMethod]
        public void multiLineCommentsAreIgnored2()
        {
            tokenRecognizedHelper("1 /* this is a \n multi\n line comment */", new Token(TokenType.INTEGER, "1"));
        }

        [TestMethod]
        public void nestedMultiLineCommentsAreIgnored()
        {
            tokenRecognizedHelper("1 /* level 1 /* level 2*/ back in level 1*/", new Token(TokenType.INTEGER, "1"));
        }
        

    }
}
