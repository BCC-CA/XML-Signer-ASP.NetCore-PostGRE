using Microsoft.AspNetCore.Identity;

namespace XmlSigner.Data.Models
{
    public class UserClaim : IdentityUserClaim<long>
    {
        //Basic Tables - Start
        public virtual User User { get; set; }
        //Basic Tables - End
    }
}
