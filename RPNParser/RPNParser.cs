using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNParser
{
    public static class RPNParser
    {
        public static int SkipSpaces(string line, int startIndex)
        {
            int countOfSpaces = 0;
            while (line.Length > countOfSpaces + startIndex && char.IsWhiteSpace(line[countOfSpaces + startIndex])) countOfSpaces++;
            return startIndex + countOfSpaces;
        }
        public static Token ReadNumbers(string line, int startIndex)
        {
            int fieldLength = 0;

            var strBuilder = new StringBuilder();
            while (line.Length > fieldLength + startIndex && (char.IsDigit(line[fieldLength + startIndex]) || line[fieldLength + startIndex] == '.' || line[fieldLength + startIndex] == ','))
            {
                strBuilder.Append(line[fieldLength + startIndex]);
                fieldLength++;
            }
            return new Token(strBuilder.ToString(), startIndex, fieldLength,true);
        }
        public static Token ReadComplexOperator(string line, int startIndex)
        {
            int fieldLength = 0;

            var strBuilder = new StringBuilder();
            while (line.Length > fieldLength + startIndex && char.IsLetter(line[fieldLength + startIndex]) )
            {
                strBuilder.Append(line[fieldLength + startIndex]);
                fieldLength++;
            }
            return new Token(strBuilder.ToString(), startIndex, fieldLength);
        }
        public static List<Token> ParseInputString(string line)
        {
            line = line.ToLower();
            char[] oneSymbolOperators = new char[] { '+', '-', '*', '/', '^','(', ')' };
            int startIndex = 0;
            int length = line.Length;
            var result = new List<Token>();
            if (line.Length == 0) return result;
            while (length > startIndex)
            {
                if (char.IsWhiteSpace(line[startIndex])) startIndex = SkipSpaces(line, startIndex);
                if (length <= startIndex) break;
                var token = new Token("", startIndex, 0);
                if (char.IsDigit(line[startIndex])) token = ReadNumbers(line, startIndex);
                else if (oneSymbolOperators.Contains(line[startIndex]))
                {
                    if (line[startIndex].ToString()=="-" && (result.Count==0 || (result.Count!=0 && result.Last().Value == "(")))
                    {
                        token = new Token("minus", startIndex, 1);
                    }
                    else
                    {
                        token = new Token(line[startIndex].ToString(), startIndex, 1);
                    }
                    
                }
                else if (char.IsLetter(line[startIndex])) token = ReadComplexOperator(line, startIndex);
                if (token.Length != 0) result.Add(token);
                startIndex = token.GetIndexNextToToken();
            }
            return result;
        }
        public static decimal Compute (string str)
        {
            decimal result = 0.0M;
            var commandStringInRPN = ConvertToRPN(ParseInputString(str));
            result = ParseRPNString(commandStringInRPN);
            return result;

        }
        /*• Операнды идут прямо в выходную очередь.
        • Если элемент -оператор, то
            • Если стек не пуст, и его верхний элемент – более приоритетный
                 оператор, то он перекладывается из стека в выходную очередь;
            • Оператор из выражения идёт в очередь.
        • Открывающая скобка идёт в стек
        • Если элемент – закрывающая скобка, то
            • Пока на вершине стека не покажется открывающая скобка,
                перекладывать операторы из стека в выходную очередь
            • Удалить открыващую скобку из стека
        • Если в выражении закончились элементы, перекладывать
        операторы из стека в выходную очередь, пока стек не опустеет.*/
        public static List<Token> ConvertToRPN(List<Token> tokens)
        {
            var result = new List<Token>();
           
            var stack = new Stack<Token>();
            foreach(var token in tokens)
            {
                if (token.IsDigit) { result.Add(token); ; }
                else if (token.Value == "(") { stack.Push(token); ; }
                else if (token.Value== ")")
                {
                    while(stack.Count!=0) 
                    {
                        var peek = stack.Pop();
                        if (peek.Value == "(") break;
                        result.Add(peek);
                                      
                    }
                                        
                }
                else
                {
                    if (stack.Count != 0 && BinaryOperator.GetOperationProirity(stack.Peek().Value) > BinaryOperator.GetOperationProirity(token.Value)) result.Add(stack.Pop());
                    //result.Add(token);
                    stack.Push(token);
                }

            }
            while (stack.Count != 0) result.Add(stack.Pop());
            return result;
        }

        public static decimal ParseRPNString(List<Token> str)
        {
            var operations = new Dictionary<string, Operator>();
            operations.Add("+", new BinaryOperator("+",  true, (a, b) => a + b));
            operations.Add("-", new BinaryOperator("-", true, (b, a) => a - b));
            operations.Add("*", new BinaryOperator("*",  true, (a, b) => a * b));
            operations.Add("/", new BinaryOperator("/",  true, (b, a) => a / b));
            operations.Add("^", new BinaryOperator("^",  true, (b, a) => (decimal) Math.Pow((double)a, (double)b)));
            operations.Add("sin", new UnaryOperator("sin", true, (a) => (decimal) Math.Sin((double)a)));
            operations.Add("cos", new UnaryOperator("cos", true, (a) => (decimal)Math.Cos((double)a)));
            operations.Add("minus", new UnaryOperator("minus", true, (a) => -a));
            var stack = new Stack<decimal>();
            foreach (var e in str)
            {
                if (e.IsDigit)
                    stack.Push(decimal.Parse(e.Value.Replace(',', '.'), CultureInfo.InvariantCulture));
                else if (operations.ContainsKey(e.Value))
                {
                    if (operations[e.Value] is BinaryOperator)
                    {
                        stack.Push(operations[e.Value].ComputeBinary(stack.Pop(), stack.Pop()));
                    }
                    else
                    {
                        stack.Push(operations[e.Value].ComputeUnary(stack.Pop()));
                    }
                }
                else
                    throw new ArgumentException();
            }
            return stack.Pop();
        }
    }
}
