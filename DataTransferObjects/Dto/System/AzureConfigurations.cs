using System.Diagnostics.CodeAnalysis;

namespace DataTransferObjects.Dto.System
{
    [ExcludeFromCodeCoverage]
    public class AzureConfigurations
    {
        public string Instance { get; set; }
        public string ClientId { get; set; }
        public string Domain { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        public string ChangePassword { get; set; }
    }
}
