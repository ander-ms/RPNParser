using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RPNParser;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        void Test1(List<Token> str, decimal result)
        {
            Assert.AreEqual(result, RPNParser.RPNParser.ParseRPNString(str));
        }
        [TestMethod]
        public void TestComplexExpression()
        {
            Test1(new List<Token> { new Token ("2",0,1,true), new Token("3", 0, 1,true), new Token("*", 0, 1), 
                new Token("5", 0, 1,true),new Token ("+",0,1),new Token ("16",0,2,true),new Token ("-",0,1) }, -5);
            //Equals to "23*5+16-"
        }
        void Test2(string str, int startIndex,string result)
        {
            var token = RPNParser.RPNParser.ReadNumbers(str, startIndex);
            Assert.AreEqual(result, token.Value);
        }
        [TestMethod]
        public void TestRPNParserReadnumbers()
        {
            Test2("0.5689+468",0, "0.5689");
        }
        [TestMethod]
        public void TestRPNParserReadnumbers2()
        {
            Test2("0.5689+468", 7, "468");
        }
        void Test3(string str, int startIndex, string result)
        {
            var token = RPNParser.RPNParser.ReadComplexOperator(str, startIndex);
            Assert.AreEqual(result, token.Value);
        }
        [TestMethod]
        public void TestReadComplexOperators()
        {
            Test3("0.56+sin(20)", 5, "sin");

        }
        void Test4(string str,  List<Token> expected)
        {
            var actual = RPNParser.RPNParser.ParseInputString(str);
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Value, actual[i].Value);
                Assert.AreEqual(expected[i].Length, actual[i].Length);
                Assert.AreEqual(expected[i].Position, actual[i].Position);
            }
        }
        [TestMethod]
        public void TestParseLine()
        {
            var expected = new List<Token> { new Token ("0.56",0,4), new Token("+",4,1), new Token("sin",5,3), new Token("(",8,1),
                new Token("20", 9, 2), new Token(")", 11, 1) };
            Test4("0.56+sin(20)", expected);

        }
        void Test5(string str, decimal result)
        {
            Assert.AreEqual(result, RPNParser.RPNParser.Compute(str));
        }
        [TestMethod]
        public void TestComputing1()
        {
            Test5("2+3*(3+2)-2",15);            
        }
        [TestMethod]
        public void TestComputing2()
        {
            Test5("(3 + 5) * 10 - 17 * 2", 46);            
        }
        [TestMethod]
        public void TestComputingWithUnaryCos()
        {
            Test5("(12.25+cos(2*4 -  32/4)) * 10 - 17 * 2", 98.5M);            
        }
        [TestMethod]
        public void TestComputingWithComma()
        {
            Test5("12,25 * 2", 24.5M);            
        }
        [TestMethod]
        public void TestComputingWithDot()
        {
            Test5("12.25 * 2", 24.5M);            
        }
        [TestMethod]
        public void TestComputingUnarySub()
        {
            Test5("(- 2+4)", 2);            
        }
         [TestMethod]
        public void Test42Pow2()
        {
            Test5("42^2", 1764);            
        }
    }
}
