using Cofoundry.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CofoundryExample.SSOAndContent
{
    public class ContentDataModel : ICustomEntityDataModel
    {
        [Required]
        [Html(HtmlToolbarPreset.BasicFormatting, HtmlToolbarPreset.Headings, HtmlToolbarPreset.Source, HtmlToolbarPreset.Media)]
        public string Html { get; set; }
    }
}