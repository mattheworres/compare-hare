namespace CompareHare.Api.Features.Shared.Validation
{
    public class CustomValidationFailure
    {
        public CustomValidationFailure(
            string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public string PropertyName { get; }
        public string ErrorMessage { get; }
    }
}
