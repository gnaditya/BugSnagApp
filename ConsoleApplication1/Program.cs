using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Griffin.Connectors.Providers.Zendesk.Web.Import;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ZendeskTrigger trigger = ZendeskTrigger.GetTriggerByType(ZendeskTriggerType.TicketReopened, 20208205);
            ZendeskTriggerBase targetBase = new ZendeskTriggerBase();
            targetBase.Trigger = trigger;
            string jsonString = JsonConvert.SerializeObject(targetBase, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            StringContent requestContent = new StringContent(JsonConvert.SerializeObject(targetBase, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), Encoding.UTF8, "application/json");
        }

    }
}
