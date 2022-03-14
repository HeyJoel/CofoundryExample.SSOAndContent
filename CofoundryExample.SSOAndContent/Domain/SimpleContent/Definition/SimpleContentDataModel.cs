using Cofoundry.Domain;
using System.ComponentModel.DataAnnotations;

namespace CofoundryExample.SSOAndContent
{
    public class SimpleContentDataModel : ICustomEntityDataModel
    {
        [Required]
        [Html(HtmlToolbarPreset.BasicFormatting, HtmlToolbarPreset.Headings, HtmlToolbarPreset.Source, HtmlToolbarPreset.Media)]
        public string Html { get; set; }
    }
}