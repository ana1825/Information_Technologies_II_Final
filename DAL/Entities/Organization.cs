using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Organization
    {
        public Organization()
        {
            PersonInOrganization = new HashSet<PersonInOrganization>();
        }

        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Activities { get; set; }

        public virtual ICollection<PersonInOrganization> PersonInOrganization { get; set; }
    }
}
