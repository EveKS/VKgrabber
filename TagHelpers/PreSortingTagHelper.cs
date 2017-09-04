using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkGroupManager.Models;

namespace VkGroups.TagHelpers
{
    public class PreSortingTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        public Filter Filter { get; set; }

        public string Atribute { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "<pre>";
            var content = (await output.GetChildContentAsync());

            var decodeContent = WebUtility.HtmlDecode(content.GetContent());

            if (decodeContent.Contains('#'))
            {
                string pattern = @"(?<hashtag>(#+\w+(@\w+)?))";
                string replacePattern = string.Empty;

                var regex = new Regex(pattern, RegexOptions.Compiled);
                decodeContent = regex.Replace(decodeContent, replacePattern).Trim();
            }

            decodeContent = $"{Atribute}\n{decodeContent}";

            output.Content.SetHtmlContent(decodeContent);
        }
    }
}
