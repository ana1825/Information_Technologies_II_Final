using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class PersonInOrganization
    {
        public int PersonInOrganizationId { get; set; }
        public int? OrganizationId { get; set; }
        public int? PersonId { get; set; }
        public int? PositionId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Person Person { get; set; }
        public virtual Positions Position { get; set; }
    }
}
