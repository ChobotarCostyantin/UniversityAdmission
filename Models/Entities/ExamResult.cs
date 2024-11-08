using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace UniversityAdmission.Models.Entities
{
    [Collection("examResults")]
    public class ExamResult
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        [BsonElement("applicantId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }

        [BsonElement("examId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ExamId { get; set; }
        public Exam? Exam { get; set; }
    }
}