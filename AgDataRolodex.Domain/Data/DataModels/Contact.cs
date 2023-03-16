using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgDataRolodex.Domain.Data.DataModels
{
    public class Contact
    {
        public Guid Id { get; set; }
        public Guid ContactNameId { get; set; }
        public Guid ContactAddressId { get; set; }
    }
}
