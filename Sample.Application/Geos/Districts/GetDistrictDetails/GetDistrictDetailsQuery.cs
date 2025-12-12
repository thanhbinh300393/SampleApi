using Sample.Common.CQRS.Queries;
using Sample.Domain.Geos.Districts;

namespace Sample.Application.Geos.Districts.GetDistrictDetails
{
    public class GetDistrictDetailsQuery : QueryBase<District>
    {
        public string DistrictCode { get; set; }

        public GetDistrictDetailsQuery(string districtCode)
        {
            DistrictCode = districtCode;
        }
    }
}
