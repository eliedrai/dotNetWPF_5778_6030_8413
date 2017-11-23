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

namespace PrinterProgram
{

    /// <summary>
    /// Interaction logic for PrinterUserControl.xaml
    /// </summary>

    public partial class PrinterUserControl : UserControl
    {
        //  General fields and method for the partial class
        private static int printerCount = 0;

        private string printerName;
        public string PrinterName
        {
            get { return printerName; }
            set
            {
                printerName = value;
                printerNameLabel.Content = PrinterName;
            }
        }

        /// <summary>
        /// Constructor of the PrinterUserControl
        /// </summary>
        public PrinterUserControl()
        {
            printerCount += 1; //  a counter of printers

            InitializeComponent();
            PrinterName = "Printer " + Convert.ToString(printerCount); //  give a name to the printer

            InkCount = MAX_INK;
            inkCountProgressBar.Maximum = MAX_INK;
            inkCountProgressBar.Minimum = 0;

            PageCount = MAX_PAGES;
            pageCountSlider.Maximum = MAX_PAGES;
            pageCountSlider.Minimum = 0;
        }


        //  Events

        /// <summary>
        /// Change fontsize of the nameLabel if cursor on the label
        /// </summary>
        /// <param name="sender">The object which sends the event</param>
        /// <param name="e">The event sent</param>
        private void EnterMouse(object sender, MouseEventArgs e)
        {
            printerNameLabel.FontSize = 30;
        }

        /// <summary>
        /// Change fontsize when the cursor leave the label
        /// </summary>
        /// <param name="sender">The object which sends the event</param>
        /// <param name="e">The event sent</param>
        private void LeaveMouse(object sender, MouseEventArgs e)
        {
            printerNameLabel.FontSize = 16;
        }


        //---------------------
        //  To manage the INK
        //---------------------

        //  CONSTANTS
        public static double MAX_INK = 100;
        public static double MIN_ADD_INK = MAX_INK / 2;
        public static double MAX_PRINT_INK = 20;

        //  Field and property
        private double inkCount;
        public double InkCount
        {
            get { return inkCount; }
            set
            {
                inkCount = value;
                inkCountProgressBar.Value = value;

                if (inkCount <= 15 && inkCount > 10)
                {
                    inkLabel.Foreground = Brushes.Yellow;
                    PrinterEventArgs arg_ink = 
                        new PrinterEventArgs(false, "Ink quantity less than 15%", PrinterName);
                    InkEmpty?.Invoke(this, arg_ink);
                }

                else if (inkCount <= 10 && inkCount > 1)
                {
                    inkLabel.Foreground = Brushes.Orange;
                    PrinterEventArgs arg_ink = 
                        new PrinterEventArgs(false, "Ink quantity less than 10%", PrinterName);
                    InkEmpty?.Invoke(this, arg_ink);
                }

                else if (inkCount <= 1)
                {
                    inkLabel.Foreground = Brushes.Red;
                    PrinterEventArgs arg_ink = 
                        new PrinterEventArgs(true, "Ink quantity less than 1%", PrinterName);
                    InkEmpty?.Invoke(this, arg_ink);
                }

                else
                {
                    inkLabel.Foreground = Brushes.Black;
                }
            }
        }

        public event EventHandler<PrinterEventArgs> InkEmpty = null;
        //===============================================================

        //-----------------------
        //  To manage the PAGES
        //-----------------------

        //  CONSTANTS
        public static int MAX_PAGES = 400;
        public static int MIN_ADD_PAGES = (int)MAX_PAGES / 8;
        public static int MAX_PRINT_PAGES = 50;

        public static readonly double MaxPages = MAX_PAGES;

        private int pageCount;
        public int PageCount
        {
            get { return pageCount; }

            set
            {
                pageCount = value;
                
                if(pageCount <= 0)
                {
                    pageLabel.Foreground = Brushes.Red;
                    string strPages = Convert.ToString(-pageCount);
                    string msg = "Miss " + strPages + " pages to perform a print.";
                    PrinterEventArgs arg_pages =
                        new PrinterEventArgs(true, msg, printerName);
                    PageMissing(this, arg_pages);
                    pageLabel.Foreground = Brushes.Black;
                }

                pageCountSlider.Value = pageCount;
            }
        }



        public event EventHandler<PrinterEventArgs> PageMissing = null;


        /// <summary>
        /// To show a tool tip with amount of ink in the printer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowInk(object sender, MouseEventArgs e)
        {
            string str;
            str = "Ink quantity: " + String.Format("{0:F1}", InkCount) + "%";
            inkCountProgressBar.ToolTip = str;
            
        }

        /// <summary>
        /// Function to manually add pages in the printer 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePages(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PageCount = (int)pageCountSlider.Value;
        }
        //===============================================================




    }

}

