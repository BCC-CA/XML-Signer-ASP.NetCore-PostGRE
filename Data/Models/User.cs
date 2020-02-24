using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlSigner.Data.Models
{
    public class User : IdentityUser<long>
    {
        //Basic Tables - Start
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        //Basic Tables - End

        //Not configured yet
        [Column("LastLoginIp", TypeName = "inet"), Required(ErrorMessage = "Last Login Ip be given"), Display(Name = "Last Login Ip", Prompt = "Please Give Last Login IP")]
        public string LastLoginIp { get; set; }
        [Column("LastLoginMac", TypeName = "macaddr"), Required(ErrorMessage = "Last Login Mac should be given"), Display(Name = "Last Login Mac", Prompt = "Please Give Last Login Mac")]
        public string LastLoginMac { get; set; }
    }
}
