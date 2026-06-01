namespace EntryPointAPI.Validators
{
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; set; } = new();
    }
    public interface IValidator<T> where T : class
    {
        public ValidationResult Validate(T value);
    }
}
