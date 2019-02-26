using System.Collections.Generic;
using System.Linq;
using FluentValidation.Validators;

public class MustNotContainDuplicatesValidator<T> : PropertyValidator {

	public MustNotContainDuplicatesValidator() : base("{PropertyName} must not contain duplicates.") {
	}

	protected override bool IsValid(PropertyValidatorContext context) {
		var list = context.PropertyValue as IList<T>;
        return list.Count == list.Distinct().Count();
	}
}
