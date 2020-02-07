using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace XmlSigner.ViewModels
{
    public class XmlFile
    {
        [IgnoreDataMember]
        [ConcurrencyCheck]  //2 file should not be same
        [Required(ErrorMessage = "File Content should be given"), MinLength(5), Display(Name = "File Content", Prompt = "Please Give File Content")]
        public IFormFile FileContent { get; set; }
        [Required(ErrorMessage = "Real Name should be given"), MinLength(5), MaxLength(32767), Display(Name = "File Real Name", Prompt = "Please Give File Real Name")]
        public string FileRealName { get; set; }
        [IgnoreDataMember]
        [Display(Name = "Signer Id", Prompt = "Please Give Signer Id")]
        public long? SignerId { get; set; }
        [ForeignKey("SignerId"), Display(Name = "Previous Signed/Unsigned File", Prompt = "Please Select Previous File")]
        public virtual IdentityUser<long> Signer { get; set; }
        
        [Display(Name = "Previous Signed/Unsigned File", Prompt = "Please select Previous File")]
        public long? PreviousFileId { get; set; }
        [ForeignKey("PreviousFileId"), Display(Name = "Previous Signed/Unsigned File", Prompt = "Please Select Previous File")]
        public virtual XmlFile PreviousSignedFile { get; set; }
    }
}
