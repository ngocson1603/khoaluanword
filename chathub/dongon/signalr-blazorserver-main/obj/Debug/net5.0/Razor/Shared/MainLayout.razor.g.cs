#pragma checksum "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\Shared\MainLayout.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "766919fe97fd7c0565d5ac4d0be0d94d0fe1db95"
// <auto-generated/>
#pragma warning disable 1591
namespace Chatty.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using Chatty;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\_Imports.razor"
using Chatty.Shared;

#line default
#line hidden
#nullable disable
    public partial class MainLayout : LayoutComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "page");
            __builder.AddAttribute(2, "b-llgovumdkq");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "sidebar");
            __builder.AddAttribute(5, "b-llgovumdkq");
            __builder.OpenComponent<Chatty.Shared.NavMenu>(6);
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.AddMarkupContent(7, "\n\n    ");
            __builder.OpenElement(8, "div");
            __builder.AddAttribute(9, "class", "main");
            __builder.AddAttribute(10, "b-llgovumdkq");
            __builder.AddMarkupContent(11, "<div class=\"top-row px-4\" b-llgovumdkq><a href=\"https://docs.microsoft.com/aspnet/\" target=\"_blank\" b-llgovumdkq>About</a></div>\n\n        ");
            __builder.OpenElement(12, "div");
            __builder.AddAttribute(13, "class", "content px-4");
            __builder.AddAttribute(14, "b-llgovumdkq");
            __builder.AddContent(15, 
#nullable restore
#line 14 "D:\DULIEUHOCTAP\KhoaLuan\khoaluanword\chathub\dongon\signalr-blazorserver-main\Shared\MainLayout.razor"
             Body

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
