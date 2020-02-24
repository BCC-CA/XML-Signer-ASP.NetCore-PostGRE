using Microsoft.AspNetCore.Identity;

namespace XmlSigner.Data.Models
{
    public class UserRole : IdentityUserRole<long>
    {
        //Basic Tables - Start
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        //Basic Tables - End
    }
}
