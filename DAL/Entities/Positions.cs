using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public partial class Positions
    {
        public Positions()
        {
            PersonInOrganization = new HashSet<PersonInOrganization>();
        }

        public int PositionId { get; set; }
        [Required]
        public string Position { get; set; }

        public virtual ICollection<PersonInOrganization> PersonInOrganization { get; set; }
    }
}
