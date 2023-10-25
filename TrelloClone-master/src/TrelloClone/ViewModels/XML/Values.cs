using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrelloClone.ViewModels.XML
{
    [XmlRoot(ElementName = "values")]
    public class Values
    {
        [XmlElement(ElementName = "value")]
        public List<Value> values { get; set; } = new List<Value>();
    }
}
