using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace FakePersonApi.Routes
{
    public class PersonController : ApiController
    {
        // GET: api/Person
        public string Get()
        {
            var fetcher = new HtmlAgilityPack.HtmlWeb();
            var reader = fetcher.Load("http://www.fakenamegenerator.com/gen-random-sw-sw.php");
            var root = reader.DocumentNode;
            Dictionary<string, string> p = new Dictionary<string, string>();
            
            foreach (var node in root.Descendants())
            {
                if (node.Attributes.Contains("class"))
                {
                    if (node.Attributes["class"].Value == "adr")
                    {
                        //address
                        p.Add("address", node.InnerText);
                    }
                    else if(node.Attributes["class"].Value == "address")
                    {
                        foreach (var item in node.ChildNodes)
                        {
                            if(item.OriginalName == "h3")
                            {
                                p.Add("namn", item.InnerText);
                                break;
                            }
                        }
                            //$('.address > h3').text(); = namn
                    }
                }
                if (node.OriginalName == "dl" && node.InnerHtml.Contains("<dt>Personnummer</dt><dd>"))
                {
                    p.Add("personnummer", node.InnerHtml.Substring(node.InnerHtml.LastIndexOf("<dd>") + 4, 11));
                }
                if (node.OriginalName == "dl" && node.InnerHtml.Contains("<dt>Phone</dt>"))
                {
                    p.Add("telefonnummer", node.InnerHtml.Substring(node.InnerHtml.LastIndexOf("<dd>") + 4, 12));
                }

            }
            var retStr = "";
            foreach (var item in p)
            {
                retStr += $"{item.Key}: {item.Value}, ";
            }
            return retStr;
        }

        // GET: api/Person/{ID}
        public string Get(int id)
        {
            var ppls = "";
            for(var i = 0; i < id; i++)
            {
                if(i > 0)
                {
                    ppls += "|\r\n";
                }
                ppls += Get();
            }
            return ppls;
        }
    }
}