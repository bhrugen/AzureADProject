using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureADB2CWeb.Models
{
    public class User
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        public string B2CObjectId { get; set; }

        public string Email { get; set; }

        public string UserRole { get; set; }
    }
}
