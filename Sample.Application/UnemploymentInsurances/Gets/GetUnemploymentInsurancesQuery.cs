using Sample.Application.UnemploymentInsurances.Dto;
using Sample.Common.CQRS.Queries;
using Sample.Common.FilterList;

namespace Sample.Application.UnemploymentInsurances.Gets
{
    public class GetUnemploymentInsurancesQuery : QueryBase<FilterResult<UnemploymentInsuranceDto>>
    {
        public FilterRequest DataRequest { get; set; }
        public GetUnemploymentInsurancesQuery(FilterRequest dataRequest)
        {
            DataRequest = dataRequest;
        }
    }
}
