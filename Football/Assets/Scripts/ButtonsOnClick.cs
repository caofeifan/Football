using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsOnClick : MonoBehaviour {

    GameObject avatar;
    GameObject camera;

	// Use this for initialization
	void Start () {
        //avatar = GameObject.Find("DefaultAvatar");
        //camera = GameObject.Find("Camera02");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    ////点击再来一次
    //public void OneMoreOnClick()//
    //{
    //    Debug.Log("\"再来一次\"  被点击了");

    //    //重置人物
    //    avatar.GetComponent<AnimController5>().Restart();
    //    //重置Camera
    //    camera.GetComponent<CameraController2>().Restart();
    //    //重置显示界面
    //    GameObject.Find("Canvas1").GetComponent<MainUI>().Restart();
    //}

    //点击退出
    public void CloseOnClick()
    {
        Debug.Log("\"退出\"  被点击了");
        //关闭场景
        Application.Quit();
    }
}
