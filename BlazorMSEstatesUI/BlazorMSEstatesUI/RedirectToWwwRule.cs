using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace BlazorMSEstatesUI;

public class RedirectToWwwRule : IRule
{
    public virtual void ApplyRule(RewriteContext context)
    {
        var req = context.HttpContext.Request;

        // Skip this rule when testing locally
        if (req.Host.Value.Contains("localhost", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = RuleResult.ContinueRules;
            return;
        }

        // Skip this rule when page already includes the www. prefix
        if (req.Host.Value.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = RuleResult.ContinueRules;
            return;
        }

        // Create the url you want to redirect to
        var wwwHost = new HostString($"www.{req.Host.Value}");
        var newUrl = UriHelper.BuildAbsolute(req.Scheme, wwwHost, req.PathBase, req.Path, req.QueryString);

        // Set the statuscode of the response to 301 (Moved Permanently)
        context.HttpContext.Response.StatusCode = 301;

        // Set the new url in the response location header
        context.HttpContext.Response.Headers[HeaderNames.Location] = newUrl;
        context.Result = RuleResult.EndResponse;
    }
}