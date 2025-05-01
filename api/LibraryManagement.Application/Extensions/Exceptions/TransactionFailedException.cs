using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Extensions.Exceptions
{
    public class TransactionFailedException : Exception
    {
        public TransactionFailedException() : base("The transaction failed to complete")
        {
        }

        public TransactionFailedException(string message) : base(message)
        {
        }

        public TransactionFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
