using Microsoft.AspNetCore.Identity;

namespace XmlSigner.Data.Models
{
    public class RoleClaim : IdentityRoleClaim<long>
    {
        //Basic Tables - Start
        public virtual Role Role { get; set; }
        //Basic Tables - End
    }
}
