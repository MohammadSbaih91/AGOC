namespace AGOC.ViewModels
{
    public class AuthValidationResult
    {
        public bool IsValid { get; set; }
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
        public string ErrorMessage { get; set; } = string.Empty;
    }

}
