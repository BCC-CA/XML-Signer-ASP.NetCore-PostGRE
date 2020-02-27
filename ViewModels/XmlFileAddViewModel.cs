using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace XmlSigner.ViewModels
{
    public class XmlFileAddViewModel
    {
        [Required(ErrorMessage = "File Content should be given"), Display(Name = "File Content", Prompt = "Please Give File Content")]
        public IFormFile XmlFile { get; set; }

        [Display(Name = "Previous Signed/Unsigned File", Prompt = "Please select Previous File")]
        public long? PreviousFileId { get; set; }
    }
}
