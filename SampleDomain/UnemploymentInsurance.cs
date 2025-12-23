using Dapper.Contrib.Extensions;
using Sample.Common.Database;
using Sample.Common.Domain;
using Sample.Common.FilterList;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
using TableAttribute = System.ComponentModel.DataAnnotations.Schema.TableAttribute;

namespace Sample.Domain
{
    [Table("UnemploymentInsurance")]
    [DatabaseType(DatabaseTypes.Default)]
    public partial class UnemploymentInsurance : AuditingEntity
    {
        [Key]
        [Dapper.Contrib.Extensions.ExplicitKey]
        public long UnemploymentInsuranceID { get; set; }
        public string? Code { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? AppointmentResultsDate { get; set; }
        [Keyword]
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }
        public int? NationID { get; set; }
        public int? ReligionID { get; set; }
        [Keyword]
        public string? IdentityNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public byte? IssueBy { get; set; }
        [Keyword]
        public string? SocialSecurityNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? BankAccountNumber { get; set; }
        public long? BankID { get; set; }
        public string? BankAccountHolder { get; set; }
        public int? TechnicalQualification { get; set; }
        public short? EducationLevel { get; set; }
        public long? FieldOfEducationID { get; set; }
        public long? CurrentCityID { get; set; }
        public long? CurrentDistrictID { get; set; }
        public long? CurrentWardID { get; set; }
        public string? CurrentAddress { get; set; }
        public string? GeneralNote { get; set; }
        public string? OtherNote { get; set; }
        public bool? ReceiveFromElsewhere { get; set; }

        public long? FromCityID { get; set; }
        public string? MonthlyCalendar { get; set; }
        public int? ReceivedMonths { get; set; }
        public int? RemainingMonths { get; set; }


        public DateTime ResignationDate { get; set; }
        public string? CompanyName { get; set; }
        public long CompanyCityID { get; set; }
        public long? CompanyDistrictID { get; set; }
        public long? CompanyWardID { get; set; }

        public short? Reason { get; set; }
        public string? PreviousOccupation { get; set; }
        public short? PreviousFieldOfWork { get; set; }
        public short? TypeOfEconomic { get; set; }
        public short? IsIndustrialZone { get; set; }
        public int? TypeOfContract { get; set; }
        public int TotalMonth { get; set; }
        public int PaymentMethodSuggestion { get; set; }
        public short? AttachedDocuments { get; set; }
        public string? LastInsuranceAddress { get; set; }
        public long? LastMonthInsuranceCityID { get; set; }
        public long? LastMonthInsuranceWardID { get; set; }

        public DateTime? FirstMonth { get; set; }
        public decimal? FirstMonthMoney { get; set; }
        public DateTime? SecondMonth { get; set; }
        public decimal? SecondMonthMoney { get; set; }
        public DateTime? ThirdMonth { get; set; }
        public decimal? ThirdMonthMoney { get; set; }
        public DateTime? FourthMonth { get; set; }
        public decimal? FourthMonthMoney { get; set; }
        public DateTime? FifthMonth { get; set; }
        public decimal? FifthMonthMoney { get; set; }
        public DateTime? SixthMonth { get; set; }
        public decimal? SixthMonthMoney { get; set; }

        //BenefitInfomation thông tin tính hưởng
        public DateTime? BenefitDate { get; set; }
        public long? SalaryID { get; set; }
        public decimal? MaximumBenefitMoney { get; set; }
        public decimal? AverageContribution { get; set; }
        public decimal? Benefit { get; set; }
        public int? TotalMonthBenefit { get; set; }
        public int? TotalMonthReserve { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? BenefitUserID { get; set; }
        public string? BenefitNote { get; set; }

        //AppraisalInfomation - thông tin thẩm định
        public DateTime? AppraisalDate { get; set; }
        public string? AppraisalContent { get; set; }
        public string? AppraisalUserID { get; set; }
        public string? AppraisalNote { get; set; }

        //DecisionInfomation - đánh số quyết định
        [Keyword]
        public int? DecisionNumber { get; set; }
        public string? DecisionUserID { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? DecisionNote { get; set; }

        //ApprovedInfomation - thông tin trình ký
        public string? SenderBy { get; set; }
        public DateTime? PostedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedNote { get; set; }

        //ReturnResultInfomation - thông tin trả kết quả
        public string? ReturnResultUserID { get; set; }
        public DateTime? ReturnResultDate { get; set; }
        public string? ReturnResultNote { get; set; }
        public int Step { get; set; }
        public string? AttachmentFile { get; set; }
        public long ManagementUnitID { get; set; }
        public Guid? SubordinateUnitID { get; set; }


        public string? DataEntryStaff { get; set; }
        public int? StepGiveBack { get; set; }

        public short? TypeProposedCancellation { get; set; }
        public int? StepBack { get; set; }
    }
    public enum StepModel : int
    {
        HSThemMoi = 1,
        DaTinhHuong = 2,
        DaThamDinh = 3,
        DaDanhSoQD = 4,
        DaTrinhKy = 5,
        DaChuyenBPTraKQ = 6,
        DaTraKQ = 7,
        DangChoPheDuyet = 8,
    }
}
