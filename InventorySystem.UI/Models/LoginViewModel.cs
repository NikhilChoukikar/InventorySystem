namespace InventorySystem.UI.Models
{
        public class LoginViewModel
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
            public string? Error { get; set; }
        }
    }

