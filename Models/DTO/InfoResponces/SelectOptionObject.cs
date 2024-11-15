using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class SelectOptionObject
    {
        public ObjectId Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}