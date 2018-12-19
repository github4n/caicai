using Lottery.Modes.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Lottery.Services.Abstractions
{
    public interface IXML_DataService
    {
        void AddGdklsfAsync(XmlNodeList xmlNodeList);
    }
}
