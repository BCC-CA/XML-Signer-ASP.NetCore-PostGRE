using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XmlSigner.Data.Models
{
    public enum LeaveType
    {
        Annual,
        Home,
        Special,
        Maternity,
        Sick
    }
    public class LeaveApplication : BaseModel
    {
        [Column("ApplicantName"), Required(ErrorMessage = "Applicant Name should be given"), MinLength(3), MaxLength(32767), Display(Name = "Applicant Name", Prompt = "Please Give Applicant Name")]
        public string Name { get; set; }
        [Column("Designation"), Required(ErrorMessage = "Designation should be given"), MinLength(3), MaxLength(32767), Display(Name = "Designation", Prompt = "Please Give Designation")]
        public string Designation { get; set; }

        [Column("LeaveStart"), Required(ErrorMessage = "Leave start day should be given"), Display(Name = "Leave Start Day", Prompt = "Please Give Leave Start Day")]
        public DateTime LeaveStart { get; set; }
        [Column("LeaveEnd"), Required(ErrorMessage = "Leave end day should be given"), Display(Name = "Leave End Day", Prompt = "Please Give Leave End Day")]
        public DateTime LeaveEnd { get; set; }
        [Column("LeaveType"), Required(ErrorMessage = "Leave Type should be chosen"), Display(Name = "Leave Type", Prompt = "Please Choose Leave Type")]
        public LeaveType LeaveType { get; set; }
        [Column("PurposeOfLeave"), Required(ErrorMessage = "Purpose Of Leave should be given"), MinLength(10), MaxLength(32767), Display(Name = "Purpose Of Leave", Prompt = "Please Write Purpose Of Leave")]
        public string PurposeOfLeave { get; set; }
        //(For Annual and Special)
        [Column("AddressDuringLeave"), Required(ErrorMessage = "Address During Leave should be given"), MinLength(10), MaxLength(32767), Display(Name = "Address During Leave", Prompt = "Please Give Address During Leave")]
        public string AddressDuringLeave { get; set; }
        [Column("PhoneNoDuringLeave"), Required(ErrorMessage = "Phone No During Leave should be given"), MinLength(6), MaxLength(20), Display(Name = "Applicant Name", Prompt = "Please Give Phone No During Leave")]
        public string PhoneNoDuringLeave { get; set; }
    }
}