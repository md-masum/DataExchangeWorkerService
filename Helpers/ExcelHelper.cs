using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataExchangeWorkerService.Configuration;
using OfficeOpenXml;

namespace DataExchangeWorkerService.Helpers
{
    public static class ExcelHelper
    {
        public static List<T> ReadFile<T>(string filePath, int startRow)
        {
            var cols = new ClientAColumnConfig().Columns();
            List<T> clientModelList = new List<T>();
            using var pck = new ExcelPackage();
            using (var stream = File.OpenRead(filePath))
            {
                pck.Load(stream);
            }
            var worksheet = pck.Workbook.Worksheets.First();



            for (var row = startRow; row <= worksheet.Dimension.End.Row; row++)
            {
                T clientModel = (T)Activator.CreateInstance(typeof(T));
                for (var j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    var col = cols.First(x => x.Index == j);
                    if (clientModel != null)
                    {
                        var type = clientModel.GetType().GetProperty(col.Model)?.PropertyType;
                        var val = ConvertColumnValue(type, worksheet.Cells[row, j].Value);
                        clientModel.GetType().GetProperty(col.Model)?.SetValue(clientModel, val);
                    }
                }
                clientModelList.Add(clientModel);
            }
            return clientModelList;
        }
        public static object ConvertColumnValue(Type type, object val)
        {
            if (type == typeof(int))
            {
                val = val != null ? Convert.ToInt32(val) : null;
            }
            else if (type == typeof(string))
            {
                val = val != null ? Convert.ToString(val) : (object)null;
            }
            else if (type == typeof(double))
            {
                val = val != null ? Convert.ToDouble(val) : null;
            }
            else if (type == typeof(decimal))
            {
                val = val != null ? Convert.ToDecimal(val) : null;
            }
            else if (type.UnderlyingSystemType == typeof(DateTime))
            {
                val = val != null ? Convert.ToDateTime(val) : null;
            }
            else if (type.UnderlyingSystemType == typeof(DateTime?))
            {
                val = val != null ? Convert.ToDateTime(val) : null;
            }
            else
            {
                val = val != null ? Convert.ToString(val) : (object)null;
            }

            return val;
        }
    }
}
