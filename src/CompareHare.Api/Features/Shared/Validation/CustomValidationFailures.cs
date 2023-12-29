using System.Collections;

namespace CompareHare.Api.Features.Shared.Validation
{
    public class CustomValidationFailures : IEnumerable<CustomValidationFailure>
    {
        public static readonly CustomValidationFailures Empty = new CustomValidationFailures();

        private readonly List<CustomValidationFailure> _validationFailures;

        public CustomValidationFailures()
        {
            _validationFailures = new List<CustomValidationFailure>();
        }

        public CustomValidationFailures(params CustomValidationFailure[] validationFailures)
            : this()
        {
            _validationFailures.AddRange(validationFailures);
        }

        public CustomValidationFailures(string propertyName, string errorMessage)
            : this(new CustomValidationFailure(propertyName, errorMessage)) { }

        public void Add(string propertyName, string errorMessage)
        {
            _validationFailures.Add(new CustomValidationFailure(propertyName, errorMessage));
        }

        public void AddRange(IEnumerable<CustomValidationFailure> validationFailures)
        {
            _validationFailures.AddRange(validationFailures);
        }

        public IEnumerator<CustomValidationFailure> GetEnumerator()
        {
            return _validationFailures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return _validationFailures.Count; } }
    }
}
