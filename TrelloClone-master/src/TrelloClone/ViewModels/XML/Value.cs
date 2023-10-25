using System.Xml.Serialization;

namespace TrelloClone.ViewModels.XML
{
    public class Value
    {
        [XmlText]
        public string json { get; set; }
    }
}
