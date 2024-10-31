using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace UniversityAdmission.Models.Entities
{
    [Collection("specialities")]
    public class Speciality
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("code")]
        public int Code { get; set; }

        [BsonElement("numberOfApplicants")]
        public int NumberOfApplicants { get; set; } = 0;

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId DepartmentId { get; set; }

        public Department? Department { get; set; }

        [BsonElement("applicants")]
        public List<Applicant> Applicants { get; set; } = new List<Applicant>();

        [BsonElement("specialityExams")]
        public List<Exam> SpecialityExams { get; set; } = new List<Exam>(); //One specialty can have multiple required exams
    }
}