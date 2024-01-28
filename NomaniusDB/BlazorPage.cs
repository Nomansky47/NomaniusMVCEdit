using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NomaniusMVC
{
    public static class BlazorPage
    {
        public static Task<IHtmlContent> Render<T>(IHtmlHelper html) where T : IComponent
        {
            return html.RenderComponentAsync<T>(RenderMode.ServerPrerendered);
        }
        public static Task<IHtmlContent> Render<T>(IHtmlHelper html, object obj) where T : IComponent
        {
            return html.RenderComponentAsync<T>(RenderMode.ServerPrerendered, obj);
        }
    }
}
