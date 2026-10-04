using FUNewsManagementSystem.Web.Models;

namespace FUNewsManagementSystem.Web.ViewModels
{
    public class AccountListViewModel
    {
        public List<AccountModel> Accounts { get; set; } = new();
        public string? Keyword { get; set; }
    }
}