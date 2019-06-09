namespace QuickspatchWeb.Models
{
    public class ControlSharedViewModelBase
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public int Length { get; set; }
        public bool ReadOnly { get; set; }
        public int Col { get; set; }

        public string ReadOnlyAttr
        {
            get
            {
                return ReadOnly ? "readonly" : "";
            }
        }
        public string Enabled { get; set; }
        public string DataBindingValue { get; set; }
    }
}