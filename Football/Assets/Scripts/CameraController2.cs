using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour {
    GameObject football;
    Quaternion startRotation;
    Quaternion curRotation;
   Quaternion targetRotation;
    AnimController5 animController;

    public GameObject camera_main;
    public GameObject camera_end;

    MainUI mainUI;

    string fileName;
    public string content;
    public float delay;
    private string prevStatus;
	// Use this for initialization
	void Start () {
        football = GameObject.Find("Ball_AI");
        this.transform.LookAt(football.transform);
        animController = GameObject.Find("DefaultAvatar").GetComponent<AnimController5>();
        camera_main = GameObject.Find("DefaultAvatar/Main Camera");
        camera_end = GameObject.Find("environment/Camera_end");

        mainUI = GameObject.Find("Canvas1").GetComponent<MainUI>();
        
        fileName = "test.txt";
        
        delay = 0f;
        Init();
    }

    private void Init()
    {
        camera_main.SetActive(true);
        camera_end.SetActive(false);
        mainUI.textLeft.text = "";
        mainUI.textRight.text = "";
        mainUI.tipImage.SetActive(false);
        content = "0";
        //设置test.txt文件
        FileOperate.WriteFileByLine("test.txt", content);
        prevStatus = "-1";
    }

    // Update is called once per frame
    void Update () {
        if(delay < 12f)
        {
            delay += Time.deltaTime;
            if (delay > 5f && delay < 8f)
            {
                transform.RotateAround(transform.position, new Vector3(-1f, 0, 0), 20f * Time.deltaTime);
            }
            else if (delay > 8.5f && delay < 11.5f)
            {
                transform.RotateAround(transform.position, new Vector3(1f, 0, 0), 20f * Time.deltaTime);
            }
            else if(delay > 11.5f && delay < 12f)
            {
                animController.curStatus = AnimController5.STATUS_IDLE;
            }
        }

        if (animController.curStatus == AnimController5.STATUS_IDLE)
        {
            content = "1";
            if (!prevStatus.Equals(content))
            {
                prevStatus = content;
                FileOperate.WriteFileByLine(fileName, content);
                mainUI.tipImage.SetActive(true);
                mainUI.textLeft.text = "";
                mainUI.textRight.text = "";
            }
            
        }
        //else if (animController.curStatus == AnimController5.STATUS_WAITTING)
        //{
        //    this.transform.LookAt(football.transform);
        //}
        else if (animController.curStatus == AnimController5.STATUS_WALKING)
        {
            content = "0";
            if (!prevStatus.Equals(content))
            {
                prevStatus = content;
                FileOperate.WriteFileByLine(fileName, content);
            }
           
            this.transform.LookAt(football.transform);
        }
        else if (animController.curStatus == 2)
        {
            content = "2";
            if (!prevStatus.Equals(content))
            {
                prevStatus = content;
                FileOperate.WriteFileByLine(fileName, content);
            }
            this.transform.LookAt(football.transform);
        }
        else if (animController.curStatus == AnimController5.STATUS_FINISH)
        {
            content = "0";
            if (!prevStatus.Equals(content))
            {
                prevStatus = content;
                FileOperate.WriteFileByLine(fileName, content);
            }
            camera_main.SetActive(false);
            camera_end.SetActive(true);
        }else if (animController.curStatus == AnimController5.WALKING)
        {
            this.transform.LookAt(football.transform);
        }

        //    this.transform.LookAt(football.transform);

    }

    public void Restart()
    {
        Init();
    }

}
