using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace XmlSigner.Data.Models
{
    public class Role : IdentityRole<long>
    {
        //Basic Tables - Start
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
        //Basic Tables - End
    }
}
