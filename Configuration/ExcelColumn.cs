namespace DataExchangeWorkerService.Configuration
{
    public class ExcelColumn
    {
        public ExcelColumn(int index, string model, bool isMandatory)
        {
            Index = index;
            Model = model;
            IsMandatory = isMandatory;
        }
        public int Index { get; set; }
        public string Model { get; set; }
        public bool IsMandatory { get; set; }
    }
}
