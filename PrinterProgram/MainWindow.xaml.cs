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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //  FIELDS
        PrinterUserControl CurrentPrinter;
        Queue<PrinterUserControl> queue;
        static Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();

            queue = new Queue<PrinterUserControl>();
            foreach(Control item in printersGrid.Children)
            {
                if(item is PrinterUserControl)
                {
                    PrinterUserControl printer = item as PrinterUserControl;
                    printer.InkEmpty += Event;
                    printer.PageMissing += Event;
                    queue.Enqueue(printer);
                }
            }
            CurrentPrinter = queue.Dequeue();
        }



        /// <summary>
        /// On clicking on button "Print", apply the event
        /// </summary>
        /// <param name="sender">The object sender</param>
        /// <param name="e">The argument sent</param>
        private void OnClick(object sender, RoutedEventArgs e)
        {
            double ink = rand.NextDouble() + rand.Next(0, (int)PrinterUserControl.MAX_PRINT_INK);
            int pages = rand.Next(0, PrinterUserControl.MAX_PRINT_PAGES);

            CurrentPrinter.InkCount = CurrentPrinter.InkCount - ink;
            CurrentPrinter.PageCount = CurrentPrinter.PageCount - pages;
        }

        public void AddInk(PrinterUserControl p)
        {
            CurrentPrinter.InkCount += 
                rand.NextDouble() + rand.Next((int)PrinterUserControl.MIN_ADD_INK, (int)PrinterUserControl.MAX_INK);
        }

        public void AddPages(PrinterUserControl p)
        {
            CurrentPrinter.PageCount += 
                rand.Next(PrinterUserControl.MIN_ADD_PAGES, PrinterUserControl.MAX_PAGES);
        }

        /// <summary>
        /// Get event from a printer, it could be ink issue or page issue
        /// If the event is critical, change printer
        /// </summary>
        /// <param name="sender">The object sender</param>
        /// <param name="e">The argument sent</param>
        public void Event(object sender, EventArgs e)
        {
            if (!(e is PrinterEventArgs))
                return;

            PrinterEventArgs args = (PrinterEventArgs)e;
            MessageBoxPrint(args);

            if(args.Critical)
            {
                if (args.ErrorMessage == "Ink quantity less than 1%")
                    AddInk(CurrentPrinter);

                else
                    AddPages(CurrentPrinter);

                if(CurrentPrinter == (PrinterUserControl)sender)
                {
                    queue.Enqueue(CurrentPrinter);
                    CurrentPrinter = queue.Dequeue();
                }
                
            }

        }

        /// <summary>
        /// Print message box according to the argument sent
        /// </summary>
        /// <param name="e">Argument sent</param>
        public void MessageBoxPrint(PrinterEventArgs e)
        {
            //  about the image
            MessageBoxImage i;
            if (e.Critical)
                i = MessageBoxImage.Error;
            else
                i = MessageBoxImage.Exclamation;

            //  about the message
            string msg = e.Date + "\n" + e.Name + " alerts:\n " + e.ErrorMessage;

            MessageBox.Show(
                msg,
                "WARNING!!",
                MessageBoxButton.OK,
                i);

        }
    }
}


