using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Expr = MathNet.Symbolics.Expression;
using MathNet.Symbolics;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        double sumX = 0.0, sumY = 0.0, sumX2 = 0.0;
        List<double> X = new List<double>();
        List<double> Y = new List<double>();

        public double intercept, slope;
        public string tendensline;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            X.Clear();
            Y.Clear();
            if (x1.Text != "" || y1.Text != "")
            {
                X.Add(double.Parse(x1.Text));
                Y.Add(double.Parse(y1.Text));
            }
            if (x2.Text != "" || y2.Text != "")
            {
                X.Add(double.Parse(x2.Text));
                Y.Add(double.Parse(y2.Text));
            }
            if (x3.Text != "" || y3.Text != "")
            {
                X.Add(double.Parse(x3.Text));
                Y.Add(double.Parse(y3.Text));
            }
            if (x4.Text != "" || y4.Text != "")
            {
                X.Add(double.Parse(x4.Text));
                Y.Add(double.Parse(y4.Text));
            }
            if (x5.Text != "" || y5.Text != "")
            {
                X.Add(double.Parse(x5.Text));
                Y.Add(double.Parse(y5.Text));
            }

            if (radioButton1.Checked)
            {
                tendensline = "Linear";
            }
            else if (radioButton2.Checked)
            {
                tendensline = "Exponential";
            }
            else if (radioButton3.Checked)
            {
                tendensline = "Logarithmic";
            }
            else if (radioButton4.Checked)
            {
                tendensline = "Power";
            }

            if(tendensline == "Linear")
            {
                Linear();
            }
            else if(tendensline == "Exponential")
            {
                Exponential();
            }

            chart1.Series.Clear();
            chart1.Series.Add("Series");
            chart1.Series.Add("Tendenslinje");
            chart1.Series["Tendenslinje"].ChartType = SeriesChartType.Line;
            chart1.Series["Series"].ChartType = SeriesChartType.Point;
            
            for (int i = 0; i < X.Count; i++)
            {
                chart1.Series["Series"].Points.AddXY(X[i], Y[i]);
            }

            chart1.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, tendensline + ", 1, false, false", chart1.Series[0], chart1.Series["Tendenslinje"]);
            
        }
        
        public void Linear()
        {
            for(int i = 0; i < X.Count; i++)
            {
                sumX += X[i];
                sumY += Y[i];
                sumX2 += X[i] * X[i];
            }

            double x = sumX / X.Count;
            double y = sumY / X.Count;

            double xx = 0.0, yy = 0.0, xy = 0.0;

            for(int i = 0; i < X.Count; i++)
            {
                xx += (X[i] - x) * (X[i] - x);
                yy += (Y[i] - y) * (Y[i] - y);
                xy += (X[i] - x) * (Y[i] - y);
            }
                        
            slope = xy / xx;
            intercept = y - slope * x;

            double rss = 0.0;
            double ssr = 0.0;
            for (int i = 0; i < X.Count; i++)
            {
                double fit = slope * X[i] + intercept;
                rss += (fit - X[i]) * (fit - X[i]);
                ssr += (fit - y) * (fit - y);
            }
            
            float r2 = (float) (ssr / yy);
            label3.Text = "y = " + (float) intercept + " + " + (float) slope + " * x; R^2=" + r2;
            
        }

        public void Exponential()
        {
            double c11 = 0.0, c12 = 0.0, c22 = 0.0, d1 = 0.0, d2 = 0.0;
            for(int i = 0; i <X.Count; i++)
            {
                c11  += Math.Pow(Math.Pow(X[i], 2), 2);
                c12  += Math.Pow(X[i], 2) * X[i];
                c22  += Math.Pow(X[i], 2);
                d1   += Math.Pow(X[i], 2) * Y[i];
                d2   += X[i] * Y[i];
            }
            
            var x = Expr.Symbol("a1");
            var y = Expr.Symbol("a2");
            
            Expr aleft = Infix.ParseOrThrow(d1.ToString());
            Expr aright = Infix.ParseOrThrow(c11 + "*a1+" + c12 + "*a2");
            Expr bleft = Infix.ParseOrThrow(d2.ToString());
            Expr bright = Infix.ParseOrThrow(c12+"*a1+" + c22 + "*a2");
            
            Expr ax = SolveSimpleRoot(x, aleft - aright);
            Expr bx = SolveSimpleRoot(x, bleft - bright);

            Expr cy = SolveSimpleRoot(y, ax - bx);

            Expr cx = Algebraic.Expand(Structure.Substitute(y, cy, ax));

            label3.Text = "y =  " + Infix.Print(cx) + "^x * " + Infix.Print(cy);

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

        public void pot()
        {
            double c11;
            double c12;
            double c22;
            double d1;
            double d2;
            foreach (double x in X)
            {

            }

        }

    }
}
