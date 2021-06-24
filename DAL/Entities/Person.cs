using DAL.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace DAL.Entities
{
    public partial class Person
    {
        public Person()
        {
            PersonInOrganization = new HashSet<PersonInOrganization>();
        }

        public int PersonId { get; set; }
        public string Picture { get; set; }
        [Required]
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PersonalId { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        [CheckDateAttribute]
        public DateTime? BirthDate { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string FullName { get; set; }

        public virtual ICollection<PersonInOrganization> PersonInOrganization { get; set; }
    }
}
