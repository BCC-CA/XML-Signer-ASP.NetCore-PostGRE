using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace XmlSigner.Data.Models
{
    public class DownloadUploadToken : BaseModel
    {
        public DownloadUploadToken() { }
        public DownloadUploadToken(string SignReason, TableName TableName, long DbEntryId)
        {
            this.TableName = TableName;
            this.DbEntryId = DbEntryId;
            this.SignReason = SignReason;
        }
        public void MarkAsUsed()
        {
            IsUsed = true;
            UploadTime = DateTime.UtcNow;
        }

        public string Token { get; set; } = Guid.NewGuid().ToString();
        public DateTime ExpirityTime { get; set; } = DateTime.UtcNow.AddMinutes(2);
        public DateTime? UploadTime { get; set; }
        public TableName TableName { get; set; } = TableName.XmlFile;
        public bool IsUsed { get; set; } = false;
        public long DbEntryId { get; set; }
        public string SignReason { get; set; }
        [IgnoreDataMember, Column("SignerId")]
        public long? SignerId { get; set; }
        [ForeignKey("SignerId")]
        public virtual User Signer { get; set; }
    }
}
