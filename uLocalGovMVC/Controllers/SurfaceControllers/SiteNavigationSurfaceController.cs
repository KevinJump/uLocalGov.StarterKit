using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using System.Web.Configuration;
using uLocalGovMVC.Models.ViewModels;

namespace uLocalGovMVC.Controllers.SurfaceControllers
{
      public class SiteNavigationSurfaceController : SurfaceController
    {
        //
        // GET: /SiteNavigationSurface/

        public ActionResult DisplayAZEntries(string letter)
        {
        // Lets Get all Pages and put their 'title' and 'url' in a sorted dictionary Then for each page, go through it's alternative A-Z entries and add them too Then cache the final SortedDictionary dependant on the umbraco.config cache file on disk
		
		
		//Try to read from cache first
		SortedDictionary<string,string> azPages = HttpRuntime.Cache["AZPages"] as SortedDictionary<string,string>;
		
		//If cache is null, build the a-z from scratch
		if (azPages == null){
	
			azPages =  new SortedDictionary<string,string>();
            //perhaps read these from configuration
            string[] docTypeAliasToIncludeInAZ = new string[] { "ulgContentPage", "ulgLandingPage" };
 
       IPublishedContent root = Umbraco.TypedContentAtRoot().First();
            var sitePages = root.Descendants().Where(x=>x.IsVisible() && docTypeAliasToIncludeInAZ.Contains(x.DocumentTypeAlias) && !x.GetPropertyValue<bool>("hideFromAZ"));
	
		foreach (IPublishedContent page in sitePages){
				
			if (!azPages.ContainsKey(page.GetPropertyValue<string>("title"))){			
			azPages.Add(page.GetPropertyValue<string>("title"), page.Url);
			}
		
            // check if page is listed under alternative entries
			if (page.HasProperty("alternativeAZEntries")){
				
			string[] otherAZEntries = page.GetPropertyValue<string>("alternativeAZEntries").Split( new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
			
				foreach (var entry in otherAZEntries){
					

					if (!azPages.ContainsKey(entry)){						
					
                        // add page under alternative entry
                        azPages.Add(entry,page.Url);	
			
					}
					
				}
			}
			
		}
			//Insert rebuilt AZ into cache based on Umbraco.config file
			HttpRuntime.Cache.Insert("AZPages",azPages, new CacheDependency(Server.MapPath(WebConfigurationManager.AppSettings["umbracoContentXML"])));
		}
		else {
			// AZ is CACHED !!
			
		}	
		
		//read in letter from querystring, filter results and send to view to display a-z

	var currentLetter = (String.IsNullOrEmpty(Request.QueryString["letter"])) ? "a" : Request.QueryString["letter"].Substring(0,1).ToLower();
	           
            // populate view model
            AZEntriesViewModel model = new AZEntriesViewModel();    
            
       
            if (!String.IsNullOrEmpty(letter)){
                currentLetter = letter;
            }

            // filter az by current letter
            model.CurrentLetter = currentLetter;
            model.HasEntries = false;
            SortedDictionary<string,string> filteredPages = new SortedDictionary<string,string>();
            foreach (var entry in azPages.Where(x=>x.Key.ToLower().StartsWith(currentLetter.ToLower()))){
                filteredPages.Add(entry.Key,entry.Value);
            }
               model.AZPages = filteredPages;
                model.HasEntries = filteredPages.Any(); 

            return PartialView("DisplayAZEntries", model);



		
        }

    }
}
