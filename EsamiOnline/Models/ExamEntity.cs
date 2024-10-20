using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EsamiOnline.Models;

public class ExamEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public string? Name { get; set; }
}