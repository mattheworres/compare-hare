namespace CompareHare.Api.Features.Shared.Models
{
    public class SelectListOptionModel
    {
        public SelectListOptionModel() { }

        public SelectListOptionModel(string label, object value)
        {
            Label = label;
            Value = value;
        }

        public string Label { get; set; }
        public object Value { get; set; }
    }
}
