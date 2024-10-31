using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models
{

    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [Required]
        [BsonElement("login")]
        public string Login { get; set; } = null!;

        [Required]
        [EmailAddress]
        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [BsonElement("password")]
        public string Password { get; set; } = null!;

        [Required]
        [BsonElement("role")]
        public Roles Role { get; set; }
    }

}
