using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLGproject.Repositories.CustomExceptions
{
    /// <summary>
    /// UserGameNotFoundException
    /// </summary>
    public class UserGameNotFoundException:Exception
    {
        public UserGameNotFoundException(string message) : base(message) { }

        public UserGameNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
