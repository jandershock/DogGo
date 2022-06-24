using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DogGo.TagHelpers
{
    public class CustomLabelTagHelper : TagHelper
    {
        public ModelExpression AspFor { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "label";
            output.Attributes.SetAttribute("for", $"{AspFor.Name}");
            output.Content.SetHtmlContent($"{AspFor.Metadata.GetDisplayName()}:");
        }
    }
}
