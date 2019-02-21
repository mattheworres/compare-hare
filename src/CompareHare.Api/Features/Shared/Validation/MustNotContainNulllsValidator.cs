using System.Collections.Generic;
using System.Linq;
using FluentValidation.Validators;

public class MustNotContainNulllsValidator<T> : PropertyValidator {

	public MustNotContainNulllsValidator() : base("{PropertyName} cannot contain empty values.") {
	}

	protected override bool IsValid(PropertyValidatorContext context) {
		var list = context.PropertyValue as IList<T>;
        return list.All(li => li != null);
	}
}