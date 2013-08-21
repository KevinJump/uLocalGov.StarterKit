using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Core.Models;

using Umbraco.Core.Logging; 

using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces; 
/// <summary>
///  the bits we need to tidy up at the end of the package installation
/// </summary>

namespace uLocalGov.PackageActions
{
    /// <summary>
    ///  fix the templates. when a doctype has multiple templates, an imported content item
    ///  will always take the default one : ( 
    /// </summary>
    public class ulgFixTempates : IPackageAction
    {
        public string Alias()
        {
            return "ulgFixTemplates"; 
        }

        private Dictionary<string, string> types;

        private void Init()
        {
            types = new Dictionary<string, string>();

            // add things here...

            // we could put these in the package xml ? 
            types.Add("A-Z", "ulgAZEntriesPage");
            types.Add("Site Map", "ulgSitemapPage");

            LogHelper.Info<ulgFixTempates>("Fix Template Package Action Starting"); 
            

        }

        /// <summary>
        ///  do it.
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public bool Execute(string packageName, System.Xml.XmlNode xmlData)
        {
            Init(); 

            IContentService _contentService = ApplicationContext.Current.Services.ContentService;
            IFileService _fileServices = ApplicationContext.Current.Services.FileService;

            foreach (var item in _contentService.GetRootContent())
            {
                // this will only return resident ? 
                if (item.Name == "Residents")
                {
                    //
                    // for now - we are assuming all the misplaced templates are at the root level 
                    // (they are for now) -- if they ever go deep, we would have to recurse all content
                    // 
                    foreach (var child in _contentService.GetChildren(item.Id))
                    {
                        if (types.ContainsKey(child.Name))
                        {
                            LogHelper.Info<ulgFixTempates>("Fixing template for {0} - {1}", ()=> child.Name, ()=> types[child.Name]);
                            // fix the type
                            ITemplate template = _fileServices.GetTemplate(types[child.Name]);

                            if (template != null)
                            {
                                child.Template = template;
                                _contentService.Save(child, 0, true);
                            }
                        }
                    }
                }

            }
            
            return true; 
        }

        public System.Xml.XmlNode SampleXml()
        {
            string sample = "<Action runat=\"install\" undo=\"true/false\" alias=\"ulgFixTemplates\"></Action>";
            return helper.parseStringToXmlNode(sample);
        }

        public bool Undo(string packageName, System.Xml.XmlNode xmlData)
        {
            return true; 
        }
    }
}
