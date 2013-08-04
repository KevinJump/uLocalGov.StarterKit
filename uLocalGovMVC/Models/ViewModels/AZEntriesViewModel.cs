using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uLocalGovMVC.Models.ViewModels
{
    public class AZEntriesViewModel
    {
        public string CurrentLetter { get; set; }
        public SortedDictionary<string, string> AZPages { get; set; }
        public bool HasEntries { get; set; }
    }
}