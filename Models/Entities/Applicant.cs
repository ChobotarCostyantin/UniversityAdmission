using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace UniversityAdmission.Models.Entities
{
    [Collection("applicants")]
    public class Applicant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; } = null!;

        [BsonElement("dateOfBirth")]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("address")]
        public string Address { get; set; } = null!;

        [BsonElement("isBeneficiary")]
        public bool IsBeneficiary { get; set; }

        [BsonElement("transferMechanism")]
        public bool TransferMechanism { get; set; } = false;

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId SpecialityId { get; set; }
        public Speciality? Speciality { get; set; }

        // public List<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }
}