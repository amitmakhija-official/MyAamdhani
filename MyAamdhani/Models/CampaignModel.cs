using System;

namespace MyAamdhani.Models
{
    public class CampaignModel
    {
        public string Id { get; set; }
        public string CampaignName { get; set; }
        public int CompanyId { get; set; }
        public string Number { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class MessageBox
    {
        public string Title;
        public string Text;
        public string Type;
    }

    public class MessageBoxPartial
    {
        public string Title;
        public string Text;
        public string Type;
    }

    public class PasswordResetMailer
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class PushNotifyData
    {
        public string body { get; set; }
        public string title { get; set; }
        public int badge { get; set; }
        public string sound { get; set; }
        public string name { get; set; }
        public int AddedBy { get; set; }
        public int NotifyTo { get; set; }
        public int LeadId { get; set; }
        public int Priority { get; set; }
    }
    public class Notifications
    {
        public string title { get; set; }
        public string body { get; set; }
    }
    public class ExpiryMessage
    {
        public string Title;
        public string Text;
        public string Type;
    }

    public class NotifyMessage
    {
        public string Title;
        public string Text;
        public string Type;
    }

}