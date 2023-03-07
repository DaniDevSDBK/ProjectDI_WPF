using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLGproject.Repositories.CustomExceptions
{
    /// <summary>
    /// GameNotFoundException
    /// </summary>
    public class GameNotFoundException:Exception
    {
        public GameNotFoundException(string message) : base(message) { }

        public GameNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
