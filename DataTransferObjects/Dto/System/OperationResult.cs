using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjects.Dto.System
{
    [ExcludeFromCodeCoverage]
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
