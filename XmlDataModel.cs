

using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TestTask;

[XmlRoot("RootXml"), Serializable]
public class XmlDataModel
{
    [XmlElement("SchemaVersion")]
    public SchemaVersion SchemaVersion { get; set; } = new SchemaVersion();
    public static Task<string> SerializationToString(XmlDataModel request)
    {
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");
        var xmlWriterSettings = new XmlWriterSettings
        {
            Indent = true,
            OmitXmlDeclaration = false,
            Encoding = Encoding.GetEncoding(1251)
        };
        var win1251 = Encoding.GetEncoding(1251);
        XmlSerializer serializer = new XmlSerializer(typeof(XmlDataModel));
        string requestString = " ";
        MemoryStream memStream = new MemoryStream();
        serializer.Serialize(XmlWriter.Create(memStream, xmlWriterSettings), request, ns);
        memStream.Flush();
        memStream.Seek(0, SeekOrigin.Begin);
        requestString = win1251.GetString(memStream.ToArray());
        return Task.FromResult(requestString);
    }
}

public class SchemaVersion
{
    [XmlAttribute("Number", DataType = "string")]
    public string Number { get; set; }
    [XmlElement("Period")]
    public Period Period { get; set; } = new Period();

}

public class Period
{
    [XmlAttribute("Date")]
    public string Date { get; set; }
    [XmlElement("Source")]
    public Source Source { get; set; } = new Source();
}
public class Source
{
    [XmlAttribute("ClassCode", DataType = "string")]
    public string ClassCode { get; set; }
    [XmlAttribute("Code", DataType = "string")]
    public string Code { get; set; }
    [XmlElement("Form")]
    public Form Form { get; set; } = new Form();

}
public class Form
{
    [XmlAttribute("Code", DataType = "string")]
    public string Code { get; set; }
    [XmlAttribute("Name", DataType = "string")]
    public string Name { get; set; }
    [XmlAttribute("Status", DataType = "string")]
    public string Status { get; set; }
    [XmlElement("Column")]
    public List<Column> ColumnList { get; set; } = new List<Column>();
    [XmlElement("Document")]
    public List<Document> Document { get; set; } = new List<Document>();
}
public class Column
{
    [XmlAttribute("Num", DataType = "string")]
    public string Num { get; set; }
    [XmlAttribute("Name", DataType = "string")]
    public string Name { get; set; }
}
public class Document
{
    [XmlAttribute("ПлСч11", DataType = "string")]
    public string ПлСч11 { get; set; }
    [XmlElement("Data")]
    public List<Data> Data { get; set; } = new List<Data>();

}
public class Data
{
    [XmlAttribute("СТРОКА", DataType = "string")]
    public string СТРОКА { get; set; }
    [XmlElement("Px")]
    public List<Px> Px { get; set; } = new List<Px>();

}
public class Px
{
    [XmlAttribute("Num", DataType = "string")]
    public string Num { get; set; }
    [XmlAttribute("Value", DataType = "string")]
    public string Value { get; set; }

}



