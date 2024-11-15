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
        public string FullName { get; set; } = string.Empty;

        [BsonElement("dateOfBirth")]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("isBeneficiary")]
        public bool IsBeneficiary { get; set; }

        [BsonElement("transferMechanism")]
        public bool TransferMechanism { get; set; } = false;

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("specialityId")]
        public ObjectId SpecialityId { get; set; }

        [BsonIgnore]
        public Speciality? Speciality { get; set; }
    }
}