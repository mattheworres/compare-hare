using FluentValidation;
using FluentValidation.Validators;

public class MustNotContainNulllsValidator<T,TProperty> : PropertyValidator<T,TProperty> {
    public override string Name => "MustNotContainNulllsValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "{PropertyName} cannot contain empty values.";

    public override bool IsValid(ValidationContext<T> context, TProperty value) {
        var list = value as IList<T>;
        return list.All(li => li != null);
    }
}
