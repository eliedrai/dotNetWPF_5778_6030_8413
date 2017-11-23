using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterProgram
{
    public class PrinterEventArgs : EventArgs
    {
        public readonly bool Critical;
        public DateTime Date = DateTime.Now;
        public readonly string ErrorMessage;
        public readonly string Name;

        public PrinterEventArgs(bool c, string msg, string n)
        {
            Critical = c;
            ErrorMessage = msg;
            Name = n;
        }
    }

}
