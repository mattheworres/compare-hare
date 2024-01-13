using FluentValidation;
using FluentValidation.Validators;

public class MustNotContainDuplicatesValidator<T,TProperty> : PropertyValidator<T,TProperty> {

    public override string Name => "MustNotContainDuplicatesValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "{PropertyName} must not contain duplicates.";

    public override bool IsValid(ValidationContext<T> context, TProperty value) {
        var list = value as IList<T>;
        return list != null ? list.Count == list.Distinct().Count() : false;
    }
}
