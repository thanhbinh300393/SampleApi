using System.ComponentModel.DataAnnotations;

namespace Sample.Domain.Collective.Positions.Dto
{
    public class PositionInput
    {
        [Required(ErrorMessage = "FieldIsRequired")]
        public string Code { get; set; }
        [Required(ErrorMessage = "FieldIsRequired")]
        public string Name { get; set; }
        public long? ManagementUnitID { get; set; }
        public string Description { get; set; }        
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public bool? IsSystem { get; set; }
        public byte? Type { get; set; }
        public long? ModuleID { get; set; }
    }
}