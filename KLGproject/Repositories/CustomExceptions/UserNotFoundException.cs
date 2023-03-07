using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLGproject.Repositories.CustomExceptions
{
    /// <summary>
    /// UserNotFoundException
    /// </summary>
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(string message):base(message) {}

        public UserNotFoundException(string message, Exception innerException): base(message, innerException){}
    }
}
