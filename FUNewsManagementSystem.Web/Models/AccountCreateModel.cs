namespace FUNewsManagementSystem.Web.Models
{
    public class AccountCreateModel
    {
        public short AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public int AccountRole { get; set; }
        public string AccountPassword { get; set; } = string.Empty;
    }
}