using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNParser
{
    public class Operator
    {
        public virtual decimal ComputeBinary(decimal a, decimal b)
        {
            throw new NotImplementedException();
        }
        public virtual decimal ComputeUnary(decimal a)
        {
            throw new NotImplementedException();
        }
        public static int GetOperationProirity(string operationName)
        {
            int result = -1;
            switch (operationName)
            {
                case "+":
                case "-": result = 2; break;
                case "minus": result = 3;break;
                case "*":
                case "/": result = 4; break;
                case "^": result = 6; break;
                case "cos":
                case "sin": result = 8; break;
                case "(":
                case ")":
                default:
                    result = -1;
                    break;
            }
            return result;

        }

    }
    public class UnaryOperator : Operator
    {
        public UnaryOperator(string name)
        {
            Name = name;
        }

        public UnaryOperator(bool leftassociativity)
        {
            IsLeftassociativity = leftassociativity;
        }
        public UnaryOperator(Func<decimal, decimal> func)
        {
            Execute = func;
        }
        public UnaryOperator(string name, bool leftassociativity, Func<decimal, decimal> func)
        {
            Name = name;

            IsLeftassociativity = leftassociativity;
            Execute = func;
        }
        private Func<decimal, decimal> Execute;
        public bool IsLeftassociativity;
        public readonly string Name;

        public override decimal ComputeUnary(decimal a)
        {
            return Execute(a);
        }
        

    }
    public class BinaryOperator : Operator
    {
        public BinaryOperator(string name)
        {
            Name = name;
        }

        public BinaryOperator(bool leftassociativity)
        {
            IsLeftassociativity = leftassociativity;
        }
        public BinaryOperator(Func<decimal, decimal, decimal> func)
        {
            Execute = func;
        }
        public BinaryOperator(string name, bool leftassociativity, Func<decimal, decimal, decimal> func)
        {
            Name = name;

            IsLeftassociativity = leftassociativity;
            Execute = func;
        }
        private Func<decimal, decimal, decimal> Execute;
        public bool IsLeftassociativity;
        public readonly string Name;

        public override decimal ComputeBinary(decimal a, decimal b)
        {
            return Execute(a, b);
        }
        
    }

}
