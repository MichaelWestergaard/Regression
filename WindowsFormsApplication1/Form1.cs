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
        public string trendline;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //Regression Beregning
        {
            label3.Text = "";
            X.Clear();
            Y.Clear();

            double number;
            //Check if valid input, and save in X and Y lists if valid.
            if (x1.Text != "" && y1.Text != "" && Double.TryParse(x1.Text, out number) == true && Double.TryParse(y1.Text, out number) == true)
            {
                X.Add(double.Parse(x1.Text)); //Parse input as double
                Y.Add(double.Parse(y1.Text));
            }
            if (x2.Text != "" && y2.Text != "" && Double.TryParse(x2.Text, out number) == true && Double.TryParse(y2.Text, out number) == true)
            {
                X.Add(double.Parse(x2.Text));
                Y.Add(double.Parse(y2.Text));
            }
            if (x3.Text != "" && y3.Text != "" && Double.TryParse(x3.Text, out number) == true && Double.TryParse(y3.Text, out number) == true)
            {
                X.Add(double.Parse(x3.Text));
                Y.Add(double.Parse(y3.Text));
            }
            if (x4.Text != "" && y4.Text != "" && Double.TryParse(x4.Text, out number) == true && Double.TryParse(y4.Text, out number) == true)
            {
                X.Add(double.Parse(x4.Text));
                Y.Add(double.Parse(y4.Text));
            }
            if (x5.Text != "" && y5.Text != "" && Double.TryParse(x5.Text, out number) == true && Double.TryParse(y5.Text, out number) == true)
            {
                X.Add(double.Parse(x5.Text));
                Y.Add(double.Parse(y5.Text));
            }
            
            //Check for type of regression to be used
            if (radioButton1.Checked)
            {
                trendline = "Linear"; //Set Chart line to correct form.
                Linear();
            }
            else if (radioButton2.Checked)
            {
                trendline = "Exponential";
                Exponential();
            }
           
            else if (radioButton4.Checked)
            {
                trendline = "3";
                poly();
            }
            
            //Setting up chart
            chart1.Series.Clear();
            chart1.Series.Add("Punktserie");
            chart1.Series.Add("Tendenslinje");
            chart1.Series["Tendenslinje"].ChartType = SeriesChartType.Line;
            chart1.Series["Punktserie"].ChartType = SeriesChartType.Point;
            
            for (int i = 0; i < X.Count; i++)
            {
                chart1.Series["Punktserie"].Points.AddXY(X[i], Y[i]);
            }

            chart1.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, trendline + ", 1, false, false", chart1.Series[0], chart1.Series["Tendenslinje"]);
            
        }
        
        public void Linear() //Regression for linear function, f(x) = ax + b
        {
            double sumX = 0.0, sumY = 0.0, sumX2 = 0.0;
            for (int i = 0; i < X.Count; i++)
            {
                // Sum of X and Y coords                
                sumX += X[i];
                sumY += Y[i];
                sumX2 += X[i] * X[i];
            }

            // Average X,Y-coord
            double x = sumX / X.Count; 
            double y = sumY / X.Count;

            double xx = 0.0, yy = 0.0, xy = 0.0;

            for(int i = 0; i < X.Count; i++)
            {
                // Used to find slope
                xx += (X[i] - x) * (X[i] - x); 
                xy += (X[i] - x) * (Y[i] - y);
            }
                        
            slope = xy / xx;
            intercept = y - slope * x;
            
            double r = r2(slope, intercept); // Find the R^2 value from slope and intercept

            label3.Text = "y = " + (float)intercept + " + " + (float)slope + " * x"; // Show the function to the user
            lblR2.Text = "R^2 = " +(float) r; // Show the R^2 value to the user


        }

        public void Exponential() //f(x) = b * a^x
        {

            List<double> logY = new List<double>();

            foreach (double y in Y) //Take the Log10 values of all Y input
            {
                logY.Add(Math.Log10(y));
            }


            double c11 = 0;
            double c12 = 0;
            double c22 = 0;
            double d1 = 0;
            double d2 = 0;

            foreach (double x in X)
            {
                c11 = c11 + Math.Pow(x, 2); //Calculate c11 value for use in formula (Se Sætning 2 Regression Forberedelses Materiale)
            }

            foreach (double x in X)
            {
                c12 = c12 + (x * 1); //Calculate c12 value for use in formula (Se Sætning 2 Regression Forberedelses Materiale)
            }

            foreach (double x in X)
            {
                c22 = c22 + Math.Pow(1, 2); //Calculate c22 value for use in formula (Se Sætning 2 Regression Forberedelses Materiale)
            }

            for (int i = 0; i < X.Count; i++)
            {
                d1 = d1 + logY[i] * X[i]; //Calculate d1 value for use in formula (Se Sætning 2 Regression Forberedelses Materiale)
            }

            for (int i = 0; i < X.Count; i++)
            {
                d2 = d2 + logY[i] * 1; //Calculate d2 value for use in formula (Se Sætning 2 Regression Forberedelses Materiale)
            }
            //Console.WriteLine("c11= " + c11);  For debugging
            //Console.WriteLine("c12= " + c12);
            //Console.WriteLine("c22= " + c22);
            //Console.WriteLine("d1= " + d1);
            //Console.WriteLine("d2= " + d2);
            double c21 = c12; //Set c21 to be equal to c12

            double a2 = ((c11 * d2) - (c21 * d1)) / ((c11 * c22) - (c12 * c21)); //Calculate A2 via Sætning 2
            double a1 = (d1 - (c12 * a2)) / c11; //Calculate A1 when we know A2

            a2 = Math.Pow(10, a2); //calculate b, by doing reverse log10
            a1 = Math.Pow(10, a1); //calculate a, by doing reverse log10

            //Console.WriteLine("a1= " + a1);
            //Console.WriteLine("a2= " + a2);
            double r = r2(a1, a2); //Calculate R^2 

            label3.Text = "f(x)=" + (float)a2 + "*" + (float)a1 + "^x";
            lblR2.Text = "R^2 = " + (float)r;


        }

        public void poly() //f(x) = ax^2 + bx
        {
            double c11 = 0;
            double c12 = 0;
            double c22 = 0;
            double d1  = 0;
            double d2  = 0;

            //Using Sætning 2 again, this time without Log10 values
            foreach (double x in X)
            {
                c11 = c11 + Math.Pow(Math.Pow(x, 2), 2);  
            }

            foreach (double x in X)
            {
                c12 = c12 + (Math.Pow(x, 2) * x);
            }

            foreach (double x in X)
            {
                c22 = c22 + Math.Pow(x,2);
            }

            for (int i = 0; i < X.Count; i++)
            {
                d1 = d1 + Y[i] * Math.Pow(X[i], 2);
            }

            for (int i = 0; i < X.Count; i++)
            {
                d2 = d2 + Y[i] * X[i];
            }
            //Console.WriteLine("c11= " + c11); For debugging
            //Console.WriteLine("c12= " + c12);
            //Console.WriteLine("c22= " + c22);
            //Console.WriteLine("d1= " + d1);
            //Console.WriteLine("d2= " + d2);
            double c21 = c12;

            double a2 = ((c11 * d2) - (c21 * d1)) /( (c11 * c22) - (c12 * c21));
            double a1 = (d1-(c12*a2))/c11;

            //Console.WriteLine("a1= " + a1);
            //Console.WriteLine("a2= " + a2);
            double r = r2(a1, a2);

            label3.Text = "f(x)=" + (float)a1 +"x^2"+ " + " +(float) a2 + "x";
            lblR2.Text = "R^2 = " + (float)r;

        }

        public double r2(double a, double b)
        {
            //Calculating R2 using method from: https://mat1a-2012.wikispaces.com/Line%C3%A6r+regression+og+korrelationskoefficienten?responseToken=b4bf5760c9de52b3f4ce8a6ae1732a84 
            double upperval = 0;
            double lowerval = 0;
            double aveB = 0;
            double aveM = 0;

            
            foreach (double y in Y)
                //Calculate average of input Y values
            {
                aveM = aveM + y; 
            }
            aveM = aveM / Y.Count;

            if (radioButton1.Checked)
            {
                foreach (double x in X)
                {
                    aveB = aveB + (a * x + b); //Calculate average of values from model

                }
                aveB = aveB / X.Count;
                foreach (double x in X)
                {
                    upperval = upperval + Math.Pow(((a * x + b) - aveB), 2); //Calculate the values of the number above the line
                }
            }
            else if (radioButton2.Checked) //Is bugged
            {
                foreach (double x in X)
                {
                    aveB = aveB + (b * Math.Pow(a, x));
                }
                aveB = aveB / X.Count;
                foreach (double x in X)
                {
                    upperval = upperval + Math.Pow(((b * Math.Pow(a,x)) - aveB), 2);
                }
            }


            else if (radioButton4.Checked) //Is bugged
            {
                foreach (double x in X)
                {
                    aveB = aveB + (a * Math.Pow(x,2) + (b*x));

                }
                aveB = aveB / X.Count;
                foreach (double x in X)
                {
                    upperval = upperval + Math.Pow(((a * Math.Pow(x, 2) + (b * x)) - aveB), 2);
                }

            }
            
            foreach (double y in Y)
            {
                lowerval = lowerval + Math.Pow(((y) - aveM), 2); //Calculate values below line
            }

            double r2 = Math.Pow(Math.Pow(upperval / lowerval, 0.5),2); //Squareroot and ^2
           
            return r2;

        }

    }
}
