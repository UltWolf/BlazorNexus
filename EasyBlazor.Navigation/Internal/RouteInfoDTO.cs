﻿using System.Text.RegularExpressions;

namespace EasyBlazor.Navigation.Internal;

public class RouteInfoDTO
{
    public string Route { get; set; }
    public IList<string> RouteSegments { get; set; }

    public bool RouteMatchesUri(string UriSegments)
    {
        var getOnlySegmentsRegExpression = new Regex(@"(?<!\?.+)(?<=\/)[\w-]+(?=[/\r\n?]|$)");

        //get only segments between / after the domain name
        var incomingSegments = getOnlySegmentsRegExpression.Matches(UriSegments).Select(x => x.Value).ToList();

        //trivial case - length doesn't match
        if (RouteSegments.Count != incomingSegments.Count)
        {
            return false;
        }

        //check segment to segment
        for(var i = 0; i < RouteSegments.Count; i++)
        {
            var textBetweenCurlyBraces = new Regex(@"{(.*?)}");
            if (textBetweenCurlyBraces.Matches(RouteSegments[i]).Any())
            {
                //its route template, not static value, skip
            }
            else
            if (incomingSegments[i] != RouteSegments[i])
            {
                //static route segment unmatched
                return false;
            }
        }
  
        //if all sequences equals then return true
        return true;
    }
}