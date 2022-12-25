using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace cart_api.Models
{
    public class BaseDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}