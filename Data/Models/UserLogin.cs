using Microsoft.AspNetCore.Identity;

namespace XmlSigner.Data.Models
{
    public class UserLogin : IdentityUserLogin<long>
    {
        //Basic Tables - Start
        public virtual User User { get; set; }
        //Basic Tables - End
    }
}
