using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;
using UniversityAdmission.Models.Entities;

namespace UniversityAdmission.Models.Entities
{
    [Collection("departments")]
    public class Department
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;
        public List<Speciality> Specialties { get; set; } = new List<Speciality>();

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("facultyId")]
        public ObjectId FacultyId { get; set; }
        public Faculty? Faculty { get; set; }
    }
}