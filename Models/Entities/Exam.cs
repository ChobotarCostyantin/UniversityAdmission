using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace UniversityAdmission.Models.Entities
{
    [Collection("exams")]
    public class Exam
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("subject")]
        public Subjects Subject { get; set; }

        [BsonElement("minScore")]
        public uint MinScore { get; set; } //to pass

        [BsonElement("isCreativeContest")]
        public bool IsCreativeContest { get; set; }
    }
}