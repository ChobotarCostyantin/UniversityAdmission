using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace UniversityAdmission.Models.Entities
{
    [Collection("groupExams")]
    public class GroupExam
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("groupId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId GroupId { get; set; }

        [BsonElement("examId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ExamId { get; set; }

        [BsonIgnore]
        public Exam? Exam { get; set; }
    }
}