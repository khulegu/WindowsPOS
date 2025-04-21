using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSLib.Exceptions
{
    public class BarcodeAlreadyExistsException : Exception
    {
        public BarcodeAlreadyExistsException() { }

        public BarcodeAlreadyExistsException(string message) : base(message) { }

        public BarcodeAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
    }

}
