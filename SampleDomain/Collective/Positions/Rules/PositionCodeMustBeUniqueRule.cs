using Sample.Common.Exceptions;

namespace Sample.Domain.Collective.Positions.Rules
{
    public class PositionCodeMustBeUniqueRule : IBusinessRule
    {
        private readonly IPositionChecker _checker;
        public readonly string _code;
        private readonly long? _positionID;
        public string Message { get => "Mã chức vụ đã tồn tại trong hệ thống"; set { } }

        public PositionCodeMustBeUniqueRule(IPositionChecker checker, string code, long? positionID = null)
        {
            _checker = checker;
            _code = code;
            _positionID = positionID;
        }
        public bool IsBroken()
        {
            return _checker.IsExistCode(_code, _positionID);
        }
    }
}
