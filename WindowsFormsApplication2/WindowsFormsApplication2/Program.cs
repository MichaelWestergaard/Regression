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

namespace WindowsFormsApplication2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            double[] x = new[] { 1.0, 2.0, 3.0 };
            double[] y = new[] { 2.0, 4.1, 7.9 };
            double[] p = Exponential(x, y); // a=1.017, r=0.687
            double[] yh = Generate.Map(x, k => p[0] * Math.Exp(p[1] * k)); // 2.02, 4.02, 7.98

        }
        static double[] Exponential(double[] x, double[] y, DirectRegressionMethod method = DirectRegressionMethod.QR)
        {
            double[] y_hat = Generate.Map(y, Math.Log);
            double[] p_hat = Fit.LinearCombination(x, y_hat, method, t => 1.0, t => t);
            return new[] { Math.Exp(p_hat[0]), p_hat[1] };
        }
    }
}
