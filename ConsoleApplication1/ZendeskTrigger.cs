// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZendeskTrigger.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Griffin.Connectors.Providers.Zendesk.Web.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Zendesk Get Targets Base
    /// </summary>
    public class ZendeskTrigger
    {
        /// <summary>
        /// trigger url
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// ZendeskTrigger id
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// trigger title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// trigger active
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }

        /// <summary>
        /// trigger position
        /// </summary>
        [JsonProperty("position")]
        public long Position { get; set; }

        /// <summary>
        /// trigger actions
        /// </summary>
        [JsonProperty("actions")]
        public List<ZendeskActionField> Actions { get; set; }

        /// <summary>
        /// trigger conditions
        /// </summary>
        [JsonProperty("conditions")]
        public ZendeskTriggerConditions Conditions { get; set; }

        /// <summary>
        /// trigger TriggerType
        /// </summary>
        public ZendeskTriggerType TriggerType { get; set; }

        /// <summary>
        /// ZendeskTrigger GetTriggerByType
        /// </summary>
        /// <param name="type">trigger type</param>
        /// <param name="targetId">target id</param>
        /// <returns>Zendesk Trigger</returns>
        public static ZendeskTrigger GetTriggerByType(ZendeskTriggerType type, long targetId)
        {
            ZendeskTrigger output = null;

            switch (type)
            {
                case ZendeskTriggerType.TicketCreated:
                    output = GetTriggerForTicketCreated(targetId);
                    break;
                case ZendeskTriggerType.TicketSolved:
                    output = GetTriggerForTicketSolved(targetId);
                    break;
                case ZendeskTriggerType.TicketReopened:
                    output = GetTriggerForTicketReopened(targetId);
                    break;
                case ZendeskTriggerType.CommentAdded:
                    output = GetTriggerForCommentAdded(targetId);
                    break;
                default:
                    break;
            }

            return output;
        }

        /// <summary>
        /// ZendeskTrigger GetTriggerForTicketCreated
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <returns>Zendesk Trigger</returns>
        private static ZendeskTrigger GetTriggerForTicketCreated(long targetId)
        {
            ZendeskTrigger output = new ZendeskTrigger();
            output.Title = "O365 New Ticket Trigger";
            output.Active = true;

            ZendeskActionField action = new ZendeskActionField();
            action.Field = ZendeskConstants.ZendeskNotificationTarget;
            string actionmessage = GetActionMessage(ZendeskTriggerType.TicketCreated);
            action.Value = new List<string>() { targetId.ToString(), actionmessage };
            output.Actions = new List<ZendeskActionField>() { action };

            output.Conditions = new ZendeskTriggerConditions();
            ZendeskAllField allCondition = new ZendeskAllField();
            allCondition.Field = ZendeskConstants.ZendeskConditionUpdateTypeField;
            allCondition.Value = ZendeskConstants.ZendeskConditionCreateValue;
            output.Conditions.All = new List<ZendeskAllField>() { allCondition };

            return output;
        }

        /// <summary>
        /// ZendeskTrigger GetTriggerForTicketReopened
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <returns>Zendesk Trigger</returns>
        private static ZendeskTrigger GetTriggerForTicketReopened(long targetId)
        {
            ZendeskTrigger output = new ZendeskTrigger();
            output.Title = "O365 Ticket Reopened Trigger";
            output.Active = true;

            ZendeskActionField action = new ZendeskActionField();
            action.Field = ZendeskConstants.ZendeskNotificationTarget;
            string actionmessage = GetActionMessage(ZendeskTriggerType.TicketReopened);
            action.Value = new List<string>() { targetId.ToString(), actionmessage };
            output.Actions = new List<ZendeskActionField>() { action };

            output.Conditions = new ZendeskTriggerConditions();
            ZendeskAllField allCondition = new ZendeskAllField();
            allCondition.Field = ZendeskConstants.ZendeskConditionReopensValue;
            allCondition.Operator = ZendeskConstants.ZendeskConditionGreaterOperator;
            allCondition.Value = "0";
            output.Conditions.All = new List<ZendeskAllField>() { allCondition };

            return output;
        }

        /// <summary>
        /// ZendeskTrigger GetTriggerForCommentAdded
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <returns>Zendesk Trigger</returns>
        private static ZendeskTrigger GetTriggerForCommentAdded(long targetId)
        {
            ZendeskTrigger output = new ZendeskTrigger();
            output.Title = "O365 Comment Added Trigger";
            output.Active = true;

            ZendeskActionField action = new ZendeskActionField();
            action.Field = ZendeskConstants.ZendeskNotificationTarget;
            string actionmessage = GetActionMessage(ZendeskTriggerType.CommentAdded);
            action.Value = new List<string>() { targetId.ToString(), actionmessage };
            output.Actions = new List<ZendeskActionField>() { action };

            output.Conditions = new ZendeskTriggerConditions();
            ZendeskAllField allCondition1 = new ZendeskAllField();
            allCondition1.Field = ZendeskConstants.ZendeskCommentField;
            allCondition1.Value = ZendeskConstants.ZendeskCommentValue;
            ZendeskAllField allCondition2 = new ZendeskAllField();
            allCondition2.Field = ZendeskConstants.ZendeskConditionUpdateTypeField;
            allCondition2.Value = ZendeskConstants.ZendeskConditionChangeValue;
            output.Conditions.All = new List<ZendeskAllField>() { allCondition1, allCondition2 };
            output.Conditions.Any = new List<ZendeskAllField>();

            return output;
        }

        /// <summary>
        /// ZendeskTrigger GetTriggerForTicketSolved
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <returns>Zendesk Trigger</returns>
        private static ZendeskTrigger GetTriggerForTicketSolved(long targetId)
        {
            ZendeskTrigger output = new ZendeskTrigger();
            output.Title = "O365 Ticket closed trigger";
            output.Active = true;

            ZendeskActionField action = new ZendeskActionField();
            action.Field = ZendeskConstants.ZendeskNotificationTarget;
            string actionmessage = GetActionMessage(ZendeskTriggerType.TicketSolved);
            action.Value = new List<string>() { targetId.ToString(), actionmessage };
            output.Actions = new List<ZendeskActionField>() { action };

            output.Conditions = new ZendeskTriggerConditions();
            ZendeskAllField allCondition = new ZendeskAllField();
            allCondition.Field = ZendeskConstants.ZendeskStatusField;
            allCondition.Operator = ZendeskConstants.ZendeskConditionIsOperator;
            allCondition.Value = ZendeskConstants.ZendeskConditionSolvedValue;
            output.Conditions.All = new List<ZendeskAllField>() { allCondition };
            output.Conditions.Any = new List<ZendeskAllField>();

            return output;
        }

        /// <summary>
        /// ZendeskTrigger GetActionMessage
        /// </summary>
        /// <param name="type">trigger type</param>
        /// <returns>action message</returns>
        private static string GetActionMessage(ZendeskTriggerType type)
        {
            string output = "{\"NotificationType\":\"" + (int)type + "\"," +
                              "\"Id\":\"{{ticket.id}}\"," +
                              "\"Title\":\"{{ticket.title}}\"," +
                              "\"Description\":\"{{ticket.description}}\"," +
                              "\"Link\":\"{{ticket.link}}\"," +
                              "\"RequesterName\":\"{{ticket.requester.name}}\"," +
                              "\"RequesterEmail\":\"{{ticket.requester.email}}\"," +
                              "\"Priority\":\"{{ticket.priority}}\"," +
                              "\"Assignee\":\"{{ticket.assignee.name}}\"," +
                              "\"CurrentUserName\":\"{{current_user.name}}\"," +
                              "\"LatestComment\":\"{{ticket.latest_comment}}\"}";
            return output;
        }
    }

    public class ZendeskActionField
    {
        /// <summary>
        /// Action Field.
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// Action value.
        /// </summary>
        [JsonProperty("value")]
        public List<string> Value { get; set; }
    }
    public class ZendeskTriggerConditions
    {
        /// <summary>
        /// all field
        /// </summary>
        [JsonProperty("all")]
        public List<ZendeskAllField> All { get; set; }

        /// <summary>
        /// any field
        /// </summary>
        [JsonProperty("any")]
        public List<ZendeskAllField> Any { get; set; }
    }
    public class ZendeskTriggerBase
    {
        /// <summary>
        /// Zendesk trigger 
        /// </summary>
        [JsonProperty("trigger")]
        public ZendeskTrigger Trigger { get; set; }
    }

    public class ZendeskAllField
    {
        /// <summary>
        /// all value.
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// all operator.
        /// </summary>
        [JsonProperty("operator")]
        public string Operator { get; set; }

        /// <summary>
        /// all value.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public enum ZendeskTriggerType
    {
        None = 0,
        TicketCreated = 1,
        TicketSolved = 2,
        TicketReopened = 3,
        CommentAdded = 4,
        Escalation = 5
    }

    /// <summary>
    /// ZenDesk constants
    /// </summary>
    public static class ZendeskConstants
    {
        /// <summary>
        /// ZenDesk provider name
        /// </summary>
        public static readonly string ProviderName = "Zendesk";

        /// <summary>
        /// The provider unique identifier
        /// </summary>
        public static readonly Guid ProviderGuid = new Guid("e46b9d8c-f627-4a40-9694-2b74cb7f5f6e");

        /// <summary>
        /// The Zendesk LogoName
        /// </summary>
        public static readonly string LogoName = "Zendesk-logo-128px.png";

        /// <summary>
        /// ZenDesk CDN picture Url format  https://outlook.office365.com/Connectors/Content/Images/Zendesk-logo-128px.png
        /// </summary>
        public static readonly string ZenDeskLogoUrl = "Connectors/Content/Images/Zendesk-logo-128px.png";

        /// <summary>
        /// ZenDesk ZenDesNotificationTarget key name
        /// </summary>
        public static readonly string ZendeskNotificationTarget = "notification_target";

        /// <summary>
        /// ZenDesk ZendeskUrlTarget key name
        /// </summary>
        public static readonly string ZendeskUrlTarget = "url_target";

        /// <summary>
        /// ZenDesk ZendeskCommentField key name
        /// </summary>
        public static readonly string ZendeskCommentField = "comment_is_public";

        /// <summary>
        /// ZenDesk ZendeskStatusField key name
        /// </summary>
        public static readonly string ZendeskStatusField = "status";

        /// <summary>
        /// ZenDesk ZendeskConditionUpdateTypeField key name
        /// </summary>
        public static readonly string ZendeskConditionUpdateTypeField = "update_type";

        /// <summary>
        /// ZenDesk ZendeskConditionCreateValue key name
        /// </summary>
        public static readonly string ZendeskConditionCreateValue = "Create";

        /// <summary>
        /// ZenDesk ZendeskConditionCreateValue key name
        /// </summary>
        public static readonly string ZendeskConditionChangeValue = "Change";

        /// <summary>
        /// ZenDesk ZendeskConditionReopensValue key name
        /// </summary>
        public static readonly string ZendeskConditionReopensValue = "reopens";

        /// <summary>
        /// ZenDesk ZendeskCommentValue key name
        /// </summary>
        public static readonly string ZendeskCommentValue = "true";

        /// <summary>
        /// ZenDesk ZendeskConditionSolvedValue key name
        /// </summary>
        public static readonly string ZendeskConditionSolvedValue = "solved";

        /// <summary>
        /// ZenDesk ZendeskConditionIsOperator key name
        /// </summary>
        public static readonly string ZendeskConditionIsOperator = "is";

        /// <summary>
        /// ZenDesk ZendeskConditionIsNotOperator key name
        /// </summary>
        public static readonly string ZendeskConditionIsNotOperator = "is_not";

        /// <summary>
        /// ZenDesk ZendeskConditionGreaterOperator key name
        /// </summary>
        public static readonly string ZendeskConditionGreaterOperator = "greater_than";

        /// <summary>
        /// ZendeskSwiftNotAssigned key name
        /// </summary>
        public static readonly string ZendeskSwiftNotAssigned = "Not Assigned";

        /// <summary>
        /// ZendeskSwiftsubTitle key name
        /// </summary>
        public static readonly string ZendeskSwiftsubTitle = "Ticket Id : {0}";

        /// <summary>
        /// Invalid Subdomain string constant
        /// </summary>
        public static readonly string InvalidSubDomain = "invalidSubDomain";

        /// <summary>
        /// Request timeout
        /// </summary>
        public static readonly int RequestTimeout = 5000;

        /// <summary>
        /// Requester string
        /// </summary>
        public static readonly string Requester = "Requester";

        /// <summary>
        /// Ticket Source
        /// </summary>
        public static readonly string TicketSource = "Ticket Source";

        /// <summary>
        /// Assignee string
        /// </summary>
        public static readonly string Assignee = "Assignee";

        /// <summary>
        /// Priority string
        /// </summary>
        public static readonly string Priority = "Priority";

        /// <summary>
        /// Type string
        /// </summary>
        public static readonly string Type = "Type";

        /// <summary>
        /// Error GetWebHooks
        /// </summary>
        public static readonly string ErrorGetWebHooks = "Unable to get web hooks for subdomain {0}.";

        /// <summary>
        /// Error DeleteWebHook
        /// </summary>
        public static readonly string ErrorDeleteWebHook = "Unable to get delete web hook for subdomain {0}.";

        /// <summary>
        /// Error CreateTarget
        /// </summary>
        public static readonly string ErrorCreateTarget = "Unable to create target for subdomain {0}.";

        /// <summary>
        /// Error CreateTrigger
        /// </summary>
        public static readonly string ErrorCreateTrigger = "Unable to create trigger {0} for subdomain {1}.";

        /// <summary>
        /// Error ErrorGetApps
        /// </summary>
        public static readonly string ErrorGetApps = "Unable to get apps installed for subdomain {0}.";

        /// <summary>
        /// Error ErrorSetApps
        /// </summary>
        public static readonly string ErrorSetApps = "Unable to update apps for subdomain {0}.";

        /// <summary>
        /// Error ErrorInstallApps
        /// </summary>
        public static readonly string ErrorInstallApps = "Unable to install app for subdomain {0}.";

        /// <summary>
        /// Zendesk login url
        /// </summary>
        public static readonly string LoginURL = "http://go.microsoft.com/fwlink/?LinkId=730655";

        /// <summary>
        /// Zendesk Registration url
        /// </summary>
        public static readonly string RegistrationURL = "http://go.microsoft.com/fwlink/?LinkId=746452";

        /// <summary>
        /// Zendesk AppId
        /// </summary>
        public static readonly long AppId = 82642;

        /// <summary>
        /// Zendesk AppName
        /// </summary>
        public static readonly string AppName = "Escalate to Office 365";

        /// <summary>
        /// Zendesk AppInstallationsURL
        /// </summary>
        public static readonly string AppInstallationsURL = "/api/v2/apps/installations.json";

        /// <summary>
        /// Zendesk AppInstallationIdURL
        /// </summary>
        public static readonly string AppInstallationIdURL = "/api/v2/apps/installations/{0}.json";

        /// <summary>
        /// Zendesk AgentRoleField
        /// </summary>
        public static readonly string AdminRoleField = "admin";

        /// <summary>
        /// Zendesk AttachmentsText
        /// </summary>
        public static readonly string AttachmentsText = "zendesk.com/attachments";
    }   
}
