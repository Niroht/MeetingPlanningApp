using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common.Model
{
    public class Attendant
    {
        public string Name { get; }

        public string Email { get; }

        public Attendant(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
