using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NomaniusMVC
{
    public enum RenderMode
    {
        /// <summary>
        /// Renders the component into static HTML.
        /// </summary>
        Static = 1,

        /// <summary>
        /// Renders a marker for a Blazor server-side application. This doesn't include any output from the component.
        /// When the user-agent starts, it uses this marker to bootstrap a blazor application.
        /// </summary>
        Server = 2,

        /// <summary>
        /// Renders the component into static HTML and includes a marker for a Blazor server-side application.
        /// When the user-agent starts, it uses this marker to bootstrap a blazor application.
        /// </summary>
        ServerPrerendered = 3,

        /// <summary>
        /// Renders a marker for a Blazor webassembly application. This doesn't include any output from the component.
        /// When the user-agent starts, it uses this marker to bootstrap a blazor client-side application.
        /// </summary>
        WebAssembly = 4,

        /// <summary>
        /// Renders the component into static HTML and includes a marker for a Blazor webassembly application.
        /// When the user-agent starts, it uses this marker to bootstrap a blazor client-side application.
        /// </summary>
        WebAssemblyPrerendered = 5,
    }
    internal interface IComponentRenderer
    {
        Task<IHtmlContent> RenderComponentAsync(
            ViewContext viewContext,
            Type componentType,
            Enum renderMode,
            object parameters);
    }
    public static class HtmlHelperComponentExtensions
    {
        /// <summary>
        /// Renders the <typeparamref name="TComponent"/>.
        /// </summary>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/>.</param>
        /// <param name="renderMode">The <see cref="RenderMode"/> for the component.</param>
        /// <returns>The HTML produced by the rendered <typeparamref name="TComponent"/>.</returns>
        public static Task<IHtmlContent> RenderComponentAsync<TComponent>(this IHtmlHelper htmlHelper, RenderMode renderMode) where TComponent : IComponent
            => RenderComponentAsync<TComponent>(htmlHelper, renderMode, parameters: null);

        /// <summary>
        /// Renders the <typeparamref name="TComponent"/>.
        /// </summary>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/>.</param>
        /// <param name="parameters">An <see cref="object"/> containing the parameters to pass
        /// to the component.</param>
        /// <param name="renderMode">The <see cref="RenderMode"/> for the component.</param>
        /// <returns>The HTML produced by the rendered <typeparamref name="TComponent"/>.</returns>
        public static Task<IHtmlContent> RenderComponentAsync<TComponent>(
            this IHtmlHelper htmlHelper,
            RenderMode renderMode,
            object parameters) where TComponent : IComponent
            => RenderComponentAsync(htmlHelper, typeof(TComponent), renderMode, parameters);

        /// <summary>
        /// Renders the specified <paramref name="componentType"/>.
        /// </summary>
        /// <param name="htmlHelper">The <see cref="IHtmlHelper"/>.</param>
        /// <param name="componentType">The component type.</param>
        /// <param name="parameters">An <see cref="object"/> containing the parameters to pass
        /// to the component.</param>
        /// <param name="renderMode">The <see cref="RenderMode"/> for the component.</param>
        public static Task<IHtmlContent> RenderComponentAsync(
            this IHtmlHelper htmlHelper,
            Type componentType,
            RenderMode renderMode,
            object parameters)
        {
            if (htmlHelper is null)
            {
                throw new ArgumentNullException(nameof(htmlHelper));
            }

            if (componentType is null)
            {
                throw new ArgumentNullException(nameof(componentType));
            }
            var viewContext = htmlHelper.ViewContext;
            var componentRenderer = viewContext.HttpContext.RequestServices.GetRequiredService<IComponentRenderer>();
            return componentRenderer.RenderComponentAsync(viewContext, componentType, renderMode, parameters);
        }
    }
}
