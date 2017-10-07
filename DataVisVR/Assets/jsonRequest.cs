using UnityEngine;
using System.Net;
using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

public class jsonRequest : MonoBehaviour {

    //static MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
    //static MongoServer server = client.GetServer();
    //static MongoDatabase db = server.GetDatabase("twitter");
    //MongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("tweets");

    string address = "http://127.0.0.1:8000/nodes/";
    WebClient client = new WebClient();


    // Use this for initialization
    void Start () {
        //foreach(BsonDocument item in collection.FindAll())
        //{
        //}

        string reply = client.DownloadString(address);
        Debug.Log(reply);

    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
