namespace FUNewsManagementSystem.Web.Models
{
    public class AccountModel
    {
        public short AccountID { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int? AccountRole { get; set; }
    }
}