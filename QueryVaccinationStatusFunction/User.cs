using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryVaccinationStatusFunction
{
    public class User
    {
        // Properties
        public string Id { get; set; }
        public bool IsVaccinated { get; set; }

        // Constructors
        public User()
        {
        }

        public User(string id, bool isVaccinated)
        {
            Id = id;
            IsVaccinated = isVaccinated;
        }
    }
}
