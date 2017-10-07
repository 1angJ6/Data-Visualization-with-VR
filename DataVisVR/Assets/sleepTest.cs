using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Bson;

public class sleepTest : MonoBehaviour
{

    BsonDocument tweetInfo = null;
    GameObject popCanvas;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name  + " trigger object is: " + other.gameObject.name);

        if(gameObject.tag == "cube" && other.gameObject.name == "bone3" && other.gameObject.transform.parent.name == "index")
        {
            Debug.Log("touched" + gameObject.name);
            //gameObject.GetComponent<Renderer>().material.color = Color.red;

            popCanvas = GameObject.Find("PopCanvas");
            popCanvas.transform.position = gameObject.transform.position;
            Vector3 pos = gameObject.transform.position;
            pos.z -= 0.1f;
            popCanvas.transform.position = pos;
            
        }

        //tweetInfo = GameObject.Find("DBConnector").GetComponent<DatabaseConnection>().getTweetInfo("796261046004621312");

        //Debug.Log(tweetInfo["id_str"]);

    }

    void OnTriggerExit(Collider other)
    {
        if (gameObject.tag == "cube" && other.gameObject.name == "bone3" && other.gameObject.transform.parent.name == "index")
        {
            Debug.Log("leave" + gameObject.name);
            //gameObject.GetComponent<Renderer>().material.color = Color.white;
            //popCanvas.SetActive(false);
        }
    }
}
