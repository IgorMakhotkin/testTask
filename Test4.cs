using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using ExcelDataReader;

namespace TestTask;

public class Test4
{


    public async Task FromExcelFileAsync()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var filePath = @"ФайлСИсходнымиДанными.xlsx";
        var responseModel = new XmlDataModel();
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var table = reader.AsDataSet();
                var result = table.Tables[0].Rows.Cast<DataRow>().ToList();
                responseModel.SchemaVersion.Number = "2";
                responseModel.SchemaVersion.Period.Date = DateTime.Now.ToShortDateString();
                responseModel.SchemaVersion.Period.Source.ClassCode = "ДМС";
                responseModel.SchemaVersion.Period.Source.Code = "819";
                responseModel.SchemaVersion.Period.Source.Form.Code = "178";
                responseModel.SchemaVersion.Period.Source.Form.Name = "Счета в кредитных организациях";
                responseModel.SchemaVersion.Period.Source.Form.Status = "0";
                var columnList = new List<Column>();
                int num = 1;
                for (int i = 0; i < result[0].ItemArray.Length; i++)
                {
                    if (!(result[0].ItemArray[i].ToString()).Contains("Код счета бюджетного учета") && result[0].ItemArray[i].ToString() != "")
                    {
                        if (result[1].ItemArray[i].ToString() != "")
                        {
                            var column = new Column();
                            column.Num = num.ToString();
                            column.Name = (result[1].ItemArray[i].ToString()).Capitalize() + " " + result[0].ItemArray[i].ToString().ToLower();
                            columnList.Add(column);
                            num++;
                        }
                        else
                        {
                            var column = new Column();
                            column.Num = num.ToString();
                            column.Name = result[0].ItemArray[i].ToString();
                            columnList.Add(column);
                            num++;
                        }

                    }
                }
                responseModel.SchemaVersion.Period.Source.Form.ColumnList = columnList;
                var documentList = new List<Document>();
                for (int i = 3; i < result.Count; i++)
                {
                    documentList.Add(CreateDocument(result[i].ItemArray!));
                }
                responseModel.SchemaVersion.Period.Source.Form.Document = documentList;
            }
        }
        var response = await XmlDataModel.SerializationToString(responseModel);
        using (StreamWriter w = new StreamWriter("ФайлРезультат.xml", false, Encoding.GetEncoding(1251)))
        {
            await w.WriteLineAsync(response);
        }

    }

    public Document CreateDocument(Object[] ItemArray)
    {
        var document = new Document();
        document.ПлСч11 = (ItemArray[1].ToString()).Code();
        var dataList = new List<Data>();
        dataList.Add(CreateData(1, ItemArray));
        document.Data = dataList;
        return document;
    }
    public Document UpdateDocument(Document document, Object[] ItemArray)
    {
        var dataList = new List<Data>();
        dataList.Add(CreateData(1, ItemArray));
        document.Data = dataList;
        return document;
    }
    public Data CreateData(int rowNum, Object[] ItemArray)
    {
        var data = new Data();
        data.СТРОКА = "00" + rowNum.ToString();
        var pxList = new List<Px>();
        for (int j = 0; j < ItemArray.Length; j++)
        {
            int pxNum = 1;
            if (j != 1)
            {
                var px = new Px();
                px.Num = pxNum.ToString();
                px.Value = ItemArray[j].ToString();
                pxList.Add(px);
                pxNum++;
            }
        }
        data.Px = pxList;
        return data;
    }
}
