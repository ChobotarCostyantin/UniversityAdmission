using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace UniversityAdmission.Models.Entities
{
    [Collection("teacherExams")]
    public class TeacherExam
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("teacherId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId TeacherId { get; set; }

        [BsonElement("examId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ExamId { get; set; }

        [BsonIgnore]
        public Exam? Exam { get; set; }
    }
}