using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ammeep.GiftRegister.Web
{
    public static class HtmlExtensions
    {
        public static IHtmlString JavaScriptEncode(this HtmlHelper html, string item)
        {
            return new HtmlString(HttpUtility.JavaScriptStringEncode(item));
        }
    }

    public static class PagerExtensions
    {
        public static IHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, null);
        }

        public static IHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount,string actionName)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, null);
        }

        public static IHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount,object values)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, new RouteValueDictionary(values));
        }

        public static IHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount,string actionName, object values)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, new RouteValueDictionary(values));
        }

        public static IHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, valuesDictionary);
        }

        public static IHtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount,string actionName, RouteValueDictionary valuesDictionary)
        {
            if (valuesDictionary == null)
            {
                valuesDictionary = new RouteValueDictionary();
            }
            if (actionName != null)
            {
                if (valuesDictionary.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "actionName");
                }
                valuesDictionary.Add("action", actionName);
            }
            var pager = new Pager(htmlHelper.ViewContext, pageSize, currentPage, totalItemCount, valuesDictionary);
            return pager.BuildHtmlString();
        }
    }

    public class Pager
    {
        private readonly int _currentPage;
        private readonly RouteValueDictionary _linkWithoutPageValuesDictionary;
        private readonly int _pageSize;
        private readonly int _totalItemCount;
        private readonly ViewContext _viewContext;

        public Pager(ViewContext viewContext, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary)
        {
            _viewContext = viewContext;
            _pageSize = pageSize;
            _currentPage = currentPage;
            _totalItemCount = totalItemCount;
            _linkWithoutPageValuesDictionary = valuesDictionary;
        }

        public IHtmlString BuildHtmlString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<span class=\"pager\">");

            var pageCount = (int) Math.Ceiling(_totalItemCount/(double) _pageSize);

            if (pageCount == 0 || pageCount == 1)
            {
                AppendSinglePage(stringBuilder);
            }
            else if (pageCount != 1 && _currentPage > 0 && _currentPage <= pageCount)
            {

                const int numberOfPagesToDisplay = 5;

                stringBuilder.Append("<span class=\"numbers\">");
               
                stringBuilder.Append(@"<ul class=""pageNumbers"">");
                stringBuilder.Append(string.Format("<li> {0}  |  </li>", GenerateViewAllPageLink("View All", 0, "pageValue", false)));
                // Previous
                if (_currentPage > 1)
                {
                    stringBuilder.Append(GeneratePageLink(@"", (_currentPage - 1), "prevLink"));
                }

                int start = 1;
                int end = pageCount;

                if (pageCount > numberOfPagesToDisplay)
                {
                    int middle = (int) Math.Ceiling(numberOfPagesToDisplay/2d) - 1;
                    int below = (_currentPage - middle);
                    int above = (_currentPage + middle);

                    if (below < 4)
                    {
                        above = numberOfPagesToDisplay;
                        below = 1;
                    }
                    else if (above > (pageCount - 4))
                    {
                        above = pageCount;
                        below = (pageCount - numberOfPagesToDisplay);
                    }

                    start = below;
                    end = above;
                }

                if (start > 3)
                {
                    stringBuilder.Append(GeneratePageLink("1", 1));
                    stringBuilder.Append(GeneratePageLink("2", 2, "noline"));
                    stringBuilder.Append("<li>...</li>");
                }

                for (int i = start; i <= end; i++)
                {
                    if (i == _currentPage)
                    {
                        stringBuilder.AppendFormat("<li><span class=\"selected\">{0}</span></li>", i);
                    }
                    else
                    {
                        stringBuilder.Append(GeneratePageLink(i.ToString(), i, (i == end) ? "noline" : null));
                    }
                }

                if (end <= (pageCount - 3))
                {
                    stringBuilder.Append("<li>...</li>");
                    stringBuilder.Append(GeneratePageLink((pageCount - 1).ToString(), pageCount - 1));
                    stringBuilder.Append(GeneratePageLink(pageCount.ToString(), pageCount));
                }

                // Next
                if (_currentPage < pageCount)
                {
                    stringBuilder.Append(GeneratePageLink(@"", (_currentPage + 1), "nextLink"));
                }

                stringBuilder.Append("</ul>");
            }
            stringBuilder.Append("</span>");// close numbers span
            stringBuilder.Append("</span>");//close pager span
            return new HtmlString(stringBuilder.ToString());
        }

        private static void AppendSinglePage(StringBuilder stringBuilder)
        {
            // Total items
            stringBuilder.Append(@"<span class=""pageNumbers"">");
            stringBuilder.Append("Page 1 of 1");
            stringBuilder.Append("</span> ");
        }

        private string GeneratePageLink(string linkText, int pageNumber, string cssClass = "pageValue")
        {
            return GeneratePageLink(linkText, pageNumber, cssClass, true);
        }

        private string GeneratePageLink(string linkText, int pageNumber, string cssClass, bool writeListItem)
        {
            var pageLinkValueDictionary = new RouteValueDictionary(_linkWithoutPageValuesDictionary);
            pageLinkValueDictionary["pageNumber"] = pageNumber;

            VirtualPathData virtualPathData = _viewContext.RouteData.Route.GetVirtualPath(_viewContext.RequestContext,
                                                                                          pageLinkValueDictionary);

            if (virtualPathData != null)
            {
                string s = String.Format("<a href=\"{0}#registry\" class=\"{1}\">{2}</a>",  @"/" + virtualPathData.VirtualPath,cssClass,linkText);

                if (writeListItem)
                    s = "<li>" + s + "</li>";

                return s;
            }
            return null;
        }


        private string GenerateViewAllPageLink(string linkText, int pageNumber, string cssClass, bool writeListItem)
        {
            var pageLinkValueDictionary = new RouteValueDictionary(_linkWithoutPageValuesDictionary);
            pageLinkValueDictionary["pageNumber"] = pageNumber;
            pageLinkValueDictionary["pageSize"] = _totalItemCount;

            VirtualPathData virtualPathData = _viewContext.RouteData.Route.GetVirtualPath(_viewContext.RequestContext,
                                                                                          pageLinkValueDictionary);

            if (virtualPathData != null)
            {
                string s = String.Format("<a href=\"{0}#registry\" class=\"{1}\">{2}</a>",@"/" + virtualPathData.VirtualPath, cssClass, linkText);

                if (writeListItem)
                    s = "<li>" + s + "</li>";

                return s;
            }
            return null;
        }

    }
}