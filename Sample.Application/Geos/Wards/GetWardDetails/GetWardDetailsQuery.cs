using Sample.Common.CQRS.Queries;
using Sample.Domain.Geos.Wards;

namespace Sample.Application.Geos.Wards.GetWardDetails
{
    public class GetWardDetailsQuery : QueryBase<Ward>
    {
        public string WardCode { get; set; }

        public GetWardDetailsQuery(string wardCode)
        {
            WardCode = wardCode;
        }
    }
}