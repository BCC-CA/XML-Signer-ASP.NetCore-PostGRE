using System;

namespace XmlSigner.Data.Models
{
    public enum TableName
    {
        XmlFile,
        Others
    }

    public class DownloadUploadToken : BaseModel
    {
        public DownloadUploadToken(string SignReason, long VerificationStep, TableName TableName, long DbEntryId)
        {
            this.TableName = TableName;
            this.DbEntryId = DbEntryId;
            this.SignReason = SignReason;
            this.VerificationStep = VerificationStep;
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
        public long VerificationStep { get; set; }
    }
}
