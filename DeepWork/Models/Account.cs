using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeepWork.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public ElementTheme Theme { get; set; }

        public List<LongTask> LongTasks { get; set; } = [];
    }
}
