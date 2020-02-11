using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlSigner.Data.Models
{
    public class User : IdentityUser<long>
    {
        //Not configured yet
        [Column("LastLoginIp", TypeName = "inet"), Required(ErrorMessage = "Last Login Ip be given"), Display(Name = "Last Login Ip", Prompt = "Please Give Last Login IP")]
        public string LastLoginIp { get; set; }
        [Column("LastLoginMac", TypeName = "macaddr"), Required(ErrorMessage = "Last Login Mac should be given"), Display(Name = "Last Login Mac", Prompt = "Please Give Last Login Mac")]
        public string LastLoginMac { get; set; }
    }
}
