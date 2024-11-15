using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace UniversityAdmission.Models.Entities
{
    [Collection("groupApplicants")]
    public class GroupApplicant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("groupId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId GroupId { get; set; }

        [BsonElement("applicantId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ApplicantId { get; set; }

        [BsonIgnore]
        public Applicant? Applicant { get; set; }
    }
}