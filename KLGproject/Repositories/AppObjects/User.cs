using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLGproject.Repositories.AppObjects
{
    public class User
    {

        private List<int> _library = new List<int>();
        public event EventHandler LibraryChanged;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public List<int> Library { get { return _library; } set { _library=value; OnLibraryChanged(); } }

        protected virtual void OnLibraryChanged()
        {
            LibraryChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
