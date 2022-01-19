using System.Collections.Generic;

namespace DataExchangeWorkerService.Configuration
{
    public class ClientAColumnConfig
    {
        public ClientAColumnConfig()
        {
            Columns();
        }
        public List<ExcelColumn> Columns()
        {
            var excelColumn = new List<ExcelColumn>();
            var index = 1;



            excelColumn.Add(new ExcelColumn(index++, "ClaimId", false));
            excelColumn.Add(new ExcelColumn(index++, "BaName", true));
            excelColumn.Add(new ExcelColumn(index++, "MobileNumber", true));



            excelColumn.Add(new ExcelColumn(index++, "ClaimDate", true));
            excelColumn.Add(new ExcelColumn(index++, "Amount", true));
            excelColumn.Add(new ExcelColumn(index++, "PaymentDate", true));
            excelColumn.Add(new ExcelColumn(index, "TransactionId", true));

            return excelColumn;
        }
    }
}
