using System;
using System.Collections.Generic;

#nullable disable

namespace DevAssessementWebsite.Models
{
    public partial class UserInformation
    {
        public int PersonId { get; set; }
        public string TellNo { get; set; }
        public string CellNo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public int? AddressCode { get; set; }
        public string PostalAddress1 { get; set; }
        public string PostalAddress2 { get; set; }
        public int? PostalCode { get; set; }

        public virtual User Person { get; set; }
    }

   public class ExtendedInformation
    {
        public UserInformation userInformation;
        public User user;

    }
    public class EditResponse
    {
        public string Id { get; set; }
        public string TellNo { get; set; }
        public string CellNo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressCode { get; set; }
        public string PostalAddress1 { get; set; }
        public string PostalAddress2 { get; set; }
        public string PostalCode { get; set; }
        public string Password { get; set; }
        public string LastLogin { get; set; }

    }
}
