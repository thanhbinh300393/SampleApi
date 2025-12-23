using Sample.Application.UnemploymentInsurances.Dto;
using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.FilterList;
using Sample.Domain;

namespace Sample.Application.UnemploymentInsurances.Gets
{
    //public class GetUnemploymentInsurancesQueryHandler : QueryHandlerBase<GetUnemploymentInsurancesQuery, FilterResult<UnemploymentInsuranceDto>>
    //{
    //    private readonly IDapperRepository<UnemploymentInsurance> _unemploymentInsuranceRepository;
    //    public GetUnemploymentInsurancesQueryHandler(
    //        IDapperRepository<UnemploymentInsurance> unemploymentInsuranceRepository,
    //        IServiceProvider serviceProvider) : base(serviceProvider)
    //    {
    //        _unemploymentInsuranceRepository = unemploymentInsuranceRepository;
    //    }

    //    public override async Task<FilterResult<UnemploymentInsuranceDto>> QueryHandle(GetUnemploymentInsurancesQuery request, CancellationToken cancellationToken)
    //    {
    //        var findStep = request.DataRequest.Filters.Find(x => x.PropertyName.Equals("StepFindResult"));
    //        if (findStep != null)
    //        {
    //            request.DataRequest.Filters.Remove(findStep);
    //        }
    //        var unemploymentInsurances = await _unemploymentInsuranceRepository.Filter<UnemploymentInsuranceDto>(request.DataRequest);
          
    //        return unemploymentInsurances;
    //    }
    //}

    public class GetUnemploymentInsurancesQueryHandler : QueryHandlerBase<GetUnemploymentInsurancesQuery, FilterResult<UnemploymentInsuranceDto>>
    {
        private readonly IDapperRepository<UnemploymentInsurance> _unemploymentInsuranceRepository;
        private readonly IEFRepository<UnemploymentInsurance> _cityRepository;
        public GetUnemploymentInsurancesQueryHandler(
            IDapperRepository<UnemploymentInsurance> unemploymentInsuranceRepository,
            IEFRepository<UnemploymentInsurance> cityRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _unemploymentInsuranceRepository = unemploymentInsuranceRepository;
            _cityRepository = cityRepository;
        }

        public override async Task<FilterResult<UnemploymentInsuranceDto>> QueryHandle(GetUnemploymentInsurancesQuery request, CancellationToken cancellationToken)
        {
            var findStep = request.DataRequest.Filters.Find(x => x.PropertyName.Equals("StepFindResult"));
            if (findStep != null)
            {
                request.DataRequest.Filters.Remove(findStep);
            }
            var listData = _cityRepository.GetAll();

            return FilterResult.Pagination<UnemploymentInsurance, UnemploymentInsuranceDto>(listData, request.DataRequest);
            //return unemploymentInsurances;
        }
    }
}
