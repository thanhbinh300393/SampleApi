using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class MigrationException : ExceptionBase
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.InternalServerError;

        public string TableName { get; set; } = string.Empty;

        public object ErrorData { get; set; }

        public int? SchoolID { get; set; }
        public int? ManagementUnitID { get; set; }

        public long? SchoolYearID { get; set; }

        public MigrationException(
            string message,
            object data,
            string tableName = "",
            int? schoolId = null,
            long? schoolYearId = null)
            : base("Lỗi kỹ thuật, vui lòng liên hệ quản trị viên!", message, data)
        {
            TableName = tableName;
            ErrorData = data;
            SchoolID = schoolId;
            SchoolYearID = schoolYearId;
        }
    }
}