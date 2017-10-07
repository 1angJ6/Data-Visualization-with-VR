using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class loading : MonoBehaviour {
    private float fps = 10.0f;
    private float time;
    //一组动画的贴图，在编辑器中赋值。
    public Texture2D[] animations;
    private int nowFram;

    AsyncOperation async;

    int progress = 0;

	// Use this for initialization
	void Start () {
        //StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        //async = Application.LoadLevelAsync("DatabaseConnTest");
        async = EditorSceneManager.LoadSceneAsync("DatabaseConnTest");

        yield return async;
    }

    void OnGUI()
    {
        DrawAnimation(animations);
    }
	
	// Update is called once per frame
	void Update () {
        progress = (int)(async.progress * 100);

        Debug.Log(progress);
	}

    void DrawAnimation(Texture2D[] tex)
    {
        //time += Time.deltaTime;

        //if(time >= 1.0 / fps)
        //{
        //    nowFram++;
        //    time = 0;
        //    if(nowFram >= tex.Length)
        //    {
        //        nowFram = 0;
        //    }
        //}
        //GUI.DrawTexture(new Rect(100, 100, 40, 60), tex[nowFram]);
        GUI.Label(new Rect(100, 180, 300, 60), "Loading...." + progress);
    }
}
