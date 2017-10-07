using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Driver;

public class DatabaseConn : MonoBehaviour {

	static MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
	static MongoServer server = client.GetServer();
	static MongoDatabase db = server.GetDatabase("twitter");
	MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("tweets");

	public MongoCollection<BsonDocument> getConnection()
	{
		return this.collection;
	}

	public BsonDocument getTweetInfo(string id_str)
	{
		var query = new QueryDocument("id_str", id_str);
		var count = collection.Find (query).Count();

		BsonDocument res;

		if (count == 0) {
			res = collection.FindOne (new QueryDocument ("retweeted_status.id_str", id_str));
		} else {
			res = collection.FindOne(query);
		}
			
		return res;
	}
}
