using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace XmlSigner.Data.Models
{
    public enum TableName
    {
        XmlFile,
        LeaveApplication,
        Other
    }
    public class XmlFile : BaseModel
    {
        public XmlFile() { }
        public XmlFile(string xmlString, string fileName, long dbEntryId, TableName tableName)
        {
            FileContent = xmlString;
            DbEntryId = dbEntryId;
            FileRealName = fileName;
            TableName = tableName;
            //DownloadUploadTokens = new List<DownloadUploadToken>();
        }

        [IgnoreDataMember]
        [ConcurrencyCheck]  //2 file should not be same
        [Column("FileContent", TypeName = "text"), Required(ErrorMessage = "File Content should be given"), MinLength(5), Display(Name = "File Content", Prompt = "Please Give File Content")]
        public string FileContent { get; set; }
        [Column("FileRealName"), Required(ErrorMessage = "Real Name should be given"), MinLength(5), MaxLength(32767), Display(Name = "File Real Name", Prompt = "Please Give File Real Name")]
        public string FileRealName { get; set; }

        public TableName TableName { get; set; } = TableName.XmlFile;
        public long DbEntryId { get; set; }
        public bool IsAlreadyUsed { get; set; } = false;

        [IgnoreDataMember]
        [Column("SignerId"), Display(Name = "Signer Id", Prompt = "Please Give Signer Id")]
        public long? SignerId { get; set; }
        [ForeignKey("SignerId"), Display(Name = "Previous Signed/Unsigned File", Prompt = "Please Select Previous File")]
        public virtual IdentityUser<long> Signer { get; set; }
        /*
        [Column("IsFilePreviouslySigned"), Required(ErrorMessage = "File Previous Sign Status should be given"), Display(Name = "File Previous Sign Status", Prompt = "Please Give File Previous Sign Status")]
        public bool IsSigned { get; set; } = true; //False for first file sign
        */
        //public long? PreviousFileId { get; set; }
        [Column("PreviousFileId"), Display(Name = "Previous Signed/Unsigned File", Prompt = "Please select Previous File")]
        public long? PreviousFileId { get; set; }
        [ForeignKey("PreviousFileId"), Display(Name = "Previous Signed/Unsigned File", Prompt = "Please Select Previous File")]
        public virtual XmlFile PreviousSignedFile { get; set; }

        //public ICollection<DownloadUploadToken> DownloadUploadTokens { get; set; }
    }
}