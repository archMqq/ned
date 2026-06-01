using EntryPointAPI.Models;

namespace EntryPointAPI.Validators
{
    public class NotificationDataValidator : IValidator<NotificationData>
    {  
        public ValidationResult Validate(NotificationData value)
        {
            var result = new ValidationResult();
            if (value.UserId <= 0)
            {
                result.Errors.Add("UserId must be greater than 0.");
            }
            if (value.Channels == null || value.Channels.Length == 0)
            {
                result.Errors.Add("At least one channel must be provided.");
            }
            else
            {
                foreach (var channel in value.Channels)
                {
                    if (string.IsNullOrWhiteSpace(channel.Address))
                    {
                        result.Errors.Add($"Channel of type {channel.Type} has an empty address.");
                    }
                }
            }
            return result;
        }
    }
}
