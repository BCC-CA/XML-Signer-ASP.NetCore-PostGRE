using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlSigner.Data.Models
{
    public class XmlFile : BaseModel
    {
        [ConcurrencyCheck]  //2 file should not be same
        [Column("FileContent", TypeName = "text"), Required(ErrorMessage = "File Content should be given"), MinLength(5), Display(Name = "File Content", Prompt = "Please Give File Content")]
        public string FileContent { get; set; }
        [Column("FileRealName"), Required(ErrorMessage = "Real Name should be given"), MinLength(5), MaxLength(32767), Display(Name = "File Real Name", Prompt = "Please Give File Real Name")]
        public string FileRealName { get; set; }

        [Column("SignerId"), Display(Name = "Signer Id", Prompt = "Please Give Signer Id")]
        public long? SignerId { get; set; }
        /*
        [Column("IsFilePreviouslySigned"), Required(ErrorMessage = "File Previous Sign Status should be given"), Display(Name = "File Previous Sign Status", Prompt = "Please Give File Previous Sign Status")]
        public bool IsSigned { get; set; } = true; //False for first file sign
        */
        //public long? PreviousFileId { get; set; }

        [Column("PreviousFileId"), Display(Name = "Previous Signed/Unsigned File", Prompt = "Please select Previous File")]
        public long? PreviousFileId { get; set; }
        [ForeignKey("PreviousFileId"), Display(Name = "Previous Signed/Unsigned File", Prompt = "Please Select Previous File")]
        public virtual XmlFile PreviousSignedFile { get; set; }
    }
}
