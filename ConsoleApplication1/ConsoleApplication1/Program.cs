using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expr = MathNet.Symbolics.Expression;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // declare variables
            var x = Expr.Symbol("a1");
            var y = Expr.Symbol("a2");

            // Parse left and right side of both equations
            Expr aleft = Infix.ParseOrThrow("1361800");
            Expr aright = Infix.ParseOrThrow("260040000*a1+2724000*a2");
            Expr bleft = Infix.ParseOrThrow("14540");
            Expr bright = Infix.ParseOrThrow("2724000*a1+30000*a2");

            // Solve both equations to x
            Expr ax = SolveSimpleRoot(x, aleft - aright); // "8 - 5*y"
            Expr bx = SolveSimpleRoot(x, bleft - bright); // "1 + 2*y"

            // Equate both terms of x, solve to y
            Expr cy = SolveSimpleRoot(y, ax - bx); // "1"

            // Substitute term of y into one of the terms of x
            Expr cx = Algebraic.Expand(Structure.Substitute(y, cy, ax)); // "3"

            // Print expression in Infix notation
            Console.WriteLine(Infix.Print(cx)); // x=3
            Console.WriteLine(Infix.Print(cy)); // y=1
            Console.ReadLine();
        }

        public static Expr SolveSimpleRoot(Expr variable, Expr expr)
        {
            // try to bring expression into polynomial form
            Expr simple = Algebraic.Expand(Rational.Numerator(Rational.Simplify(variable, expr)));

            // extract coefficients, solve known forms of order up to 1
            Expr[] coeff = Polynomial.Coefficients(variable, simple);
            switch (coeff.Length)
            {
                case 1: return Expr.Zero.Equals(coeff[0]) ? variable : Expr.Undefined;
                case 2: return Rational.Simplify(variable, Algebraic.Expand(-coeff[0] / coeff[1]));
                default: return Expr.Undefined;
            }
        }
    }
}
