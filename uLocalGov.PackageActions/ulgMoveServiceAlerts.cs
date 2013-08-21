using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using umbraco.interfaces;
using umbraco.cms.businesslogic.packager.standardPackageActions;

using Umbraco.Core; 
using Umbraco.Core.Services;
using Umbraco.Core.Models;
using Umbraco.Core.Logging; 

namespace uLocalGov.PackageActions
{
    /// <summary>
    ///  moves services alerts out of the resident node, 
    ///  the packager can only package up one content node from the root
    /// </summary>
    public class ulgMoveServiceAlerts : IPackageAction
    {
        public string Alias()
        {
            return "ulgMoveServiceAlerts"; 
        }

        public bool Execute(string packageName, System.Xml.XmlNode xmlData)
        {
            IContentService _contentService = ApplicationContext.Current.Services.ContentService;

            foreach (var node in _contentService.GetRootContent())
            {
                if (node.Name == "Residents")
                {
                    IContent _alerts = _contentService.GetChildrenByName(node.Id, "Shared Content").FirstOrDefault();

                    if ((_alerts != null) && (_alerts.Name == "Shared Content"))
                    {
                        LogHelper.Info<ulgMoveServiceAlerts>("Moving Shared Content to the root"); 
                        _alerts.ParentId = -1; // move to root
                        _contentService.Save(_alerts, 0, true); 
                    }
                }
            }

            return true; 
        }

        public System.Xml.XmlNode SampleXml()
        {
            string sample = "<Action runat=\"install\" undo=\"true/false\" alias=\"ulgMoveServiceAlerts\"></Action>";
            return helper.parseStringToXmlNode(sample);

        }

        public bool Undo(string packageName, System.Xml.XmlNode xmlData)
        {
            return true;
        }
    }
}
