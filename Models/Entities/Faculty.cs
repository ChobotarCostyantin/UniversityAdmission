using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace UniversityAdmission.Models
{
    [Collection("faculties")]
    public class Faculty
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("departments")]
        public List<Department> Departments { get; set; } = new List<Department>();

        [BsonElement("numberOfApplicants")]
        public int NumberOfApplicants { get; set; } = 0;
    }
}