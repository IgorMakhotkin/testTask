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
                    if (!(result[0].ItemArray[i].ToString()).Contains("Код счета бюджетного учета"))
                    {
                        if (result[1].ItemArray[i].ToString() != "")
                        {
                            var column = new Column();
                            if (result[1].ItemArray[i].ToString() != "" && result[0].ItemArray[i].ToString() != "")
                            {
                                column.Num = num.ToString();
                                column.Name = (result[1].ItemArray[i].ToString()).Capitalize() + " " + result[0].ItemArray[i].ToString().ToLower();
                            }
                            else
                            {
                                column.Num = num.ToString();
                                column.Name = (result[1].ItemArray[i].ToString()).Capitalize() + " " + result[0].ItemArray[i-1].ToString().ToLower();
                            }
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
                foreach (var doc in CreateDocumentList(result))
                {
                    var document = new Document();
                    document.ПлСч11 = doc.Key;
                    document.Data = doc.Value;
                    document.Data.Add(Row960(doc.Value));
                    documentList.Add(document);
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

    public Dictionary<string, List<Data>> CreateDocumentList(List<DataRow> result)
    {
        var documentDictionary = new Dictionary<string, List<Data>>();
        for (int i = 3; i < result.Count; i++)
        {
            if (documentDictionary.TryGetValue(result[i].ItemArray[1].ToString().Code(), out var docValue))
            {
                docValue.Add(CreateData(docValue.Count + 1, result[i].ItemArray));
                documentDictionary.Remove(result[i].ItemArray[1].ToString().Code());
                documentDictionary.Add(result[i].ItemArray[1].ToString().Code(), docValue);
            }
            else
            {
                var documentKey = (result[i].ItemArray[1].ToString()).Code();
                var dataList = new List<Data>();
                dataList.Add(CreateData(1, result[i].ItemArray));
                documentDictionary.Add(documentKey, dataList);
            }
        }
        return documentDictionary;
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
        int pxNum = 1;
        for (int j = 0; j < ItemArray.Length; j++)
        {
            if (j != 1)
            {
                var px = new Px();
                px.Num = pxNum.ToString();
                if(Double.TryParse(ItemArray[j].ToString(),out var value))
                {
                    px.Value = (value.ToString("0.00")).Replace(',','.');
                }
                else
                {
                    px.Value = ItemArray[j].ToString();
                }
                pxList.Add(px);
                pxNum++;
            }
        }
        data.Px = pxList;
        return data;
    }
    public Data Row960(List<Data> dataList)
    {
        var data = new Data();
        data.СТРОКА = "960";
        var dataSum = new Dictionary<string, decimal>();
        foreach (var pxList in dataList)
        {
            foreach (var px in pxList.Px)
            {
                if (!px.Num.Contains("1"))
                {
                    if (dataSum.TryGetValue(px.Num, out var pxValue))
                    {
                        Decimal.TryParse(px.Value.Replace('.',','), out var decimallPxValue);
                        pxValue += decimallPxValue;
                        dataSum.Remove(px.Num);
                        dataSum.Add(px.Num, pxValue);
                    }
                    else
                    {
                        Decimal.TryParse(px.Value.Replace('.',','), out var decimallPxValue);
                        dataSum.Add(px.Num, decimallPxValue);
                    }
                }

            }
        }
        var sumPxValue = new List<Px>();
        foreach (var pxValue in dataSum)
        {
            var px = new Px();
            px.Num = pxValue.Key;
            px.Value = pxValue.Value.ToString("0.00").Replace(',','.');
            sumPxValue.Add(px);
        }
        data.Px = sumPxValue;
        return data;
    }
}
