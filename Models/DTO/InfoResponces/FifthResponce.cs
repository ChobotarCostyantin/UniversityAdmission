using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace UniversityAdmission.Models.DTO.InfoResponces
{
    public class FifthResponce
    {
        public List<SelectOptionResponse> ResponseOptions { get; set; } = new();
        public Dictionary<string, int> Result { get; set; } = new();
    }
}