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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
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
            label3.Text = "";
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
            double sumX = 0.0, sumY = 0.0, sumX2 = 0.0;
            for (int i = 0; i < X.Count; i++)
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

            double r = r2(slope, intercept);
            label3.Text = "y = " + (float) intercept + " + " + (float) slope + " * x; R^2=" + r;
            
        }

        public void Exponential()
        {
            double c11 = 0.0, c12 = 0.0, c22 = 0.0, d1 = 0.0, d2 = 0.0;

            //f1(x)=x
            //f2(x)=1

            for(int i = 0; i < X.Count; i++)
            {
                c11  += Math.Pow(X[i],2);
                c12  += X[i] * 1;
                c22  += Math.Pow(1, 2);
                d1   += Y[i] * X[i];
                d2   += Y[i] * 1;
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

        public double r2(double a, double b)
        {
            double upperval = 0;
            double lowerval = 0;
            double aveB = 0;
            double aveM = 0;

            foreach (double x in X)
            {
                aveB = aveB + (a * x + b);
          
            }
            foreach (double y in Y)
            {
                aveM = aveM + y;
            }
            aveB = aveB / X.Count;
            aveM = aveM / Y.Count;

            foreach (double x in X)
            {
                upperval = upperval + Math.Pow(((a * x + b) - aveB), 2);
            }
            foreach (double y in Y)
            {
                lowerval = lowerval + Math.Pow(((y) - aveM), 2);
            }

            double r2 = Math.Pow(Math.Pow(upperval / lowerval, 0.5),2);
            return r2;

        }

    }
}
