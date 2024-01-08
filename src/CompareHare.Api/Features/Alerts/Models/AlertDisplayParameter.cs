namespace CompareHare.Api.Features.Alerts.Models
{
    public class AlertDisplayParameter
    {
        public AlertDisplayParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
