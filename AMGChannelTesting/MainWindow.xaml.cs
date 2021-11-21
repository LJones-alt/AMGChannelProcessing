// Laura Jones 21/11/21 Channel Processing
//This holds the main window function
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AMGChannelTesting
{

    public partial class MainWindow : Window
    {
        public string FilePath = "filepath";
        public MainWindow()
        {
            InitializeComponent();
        }
        //opens folder browser to select text file for channels
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            FilePath = UtilFunctions.ShowFolderBrowser();
        }

        private void BtCalc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] Xvals = UtilFunctions.LoadInput(FilePath);
                Int32 ArrayLength = Xvals.Length;
                Console.WriteLine("Loaded file, found {0} values", ArrayLength);

                //Now process this information - see supporting docs for breakdown
                // B = (1/X) + mX + c 
                //m = 2.0, unless user input
                //c=0.5, unless user input
                double m = Convert.ToDouble(TbGetM.Text);
                double c = Convert.ToDouble(TbGetC.Text);
                // create B array from calulation
                double[] BVals = UtilFunctions.ProcessB(Xvals, ArrayLength, m, c);
                //calculate b as mean of B
                double b = UtilFunctions.CalcB(BVals, ArrayLength);
               // b = Math.Round(b,1); //round to 1dp (from input parameter values)
                TbCalcb.Text = b.ToString();
                double[] CVals = UtilFunctions.GetCVals(Xvals, b, ArrayLength);
               
                //plot the calculated values
                double[] Xdata = Xvals.Skip(1).ToArray();
                double[] Cdata = CVals.Skip(1).ToArray();
                GraphPlot.Plot.Clear(); //remove existing plot
                GraphPlot.Plot.AddScatter(Xdata, BVals, markerSize: 5, lineWidth: 0, label: "Calculated B Values");
                GraphPlot.Plot.AddScatter(Xdata, Cdata, markerSize: 5, lineWidth: 0, label: "Calculated C Values"); //should be a flat line
                GraphPlot.Plot.XLabel("Loaded X Values");
                GraphPlot.Plot.YLabel("Calculated Values");
                GraphPlot.Plot.Legend(location: ScottPlot.Alignment.UpperRight);
                GraphPlot.Refresh(); //display
                
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message.ToString(), "Error");
            }
        }
    }
}
