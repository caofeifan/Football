using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    GameObject mainCamera;
    MianController mainController;
    GameObject tipImage;
    public Text tips;
    public Text textRight;
    public Text textLeft;
    private float curTime;

    void Init()
    {
        textLeft.text = "";
        textRight.text = "";
    }

    public void Restart()
    {
        Init();
    }

	// Use this for initialization
	void Start () {
        mainCamera = GameObject.Find("MainCamera");
        mainController = mainCamera.GetComponent<MianController>();
        tipImage = GameObject.Find("Canvas1/Image");

        Init();

    }
	
	// Update is called once per frame
	void Update () {
		if(mainController.content == "0")
        {
            tipImage.SetActive(false);
            curTime = mainController.timer.GetCurTime();
            tips.text = "未采集  "+ curTime.ToString("0")+"s";
            ;
        }
        else
        {
            tipImage.SetActive(true);
            curTime = mainController.timer.GetCurTime();
            tips.text = "采集中" + curTime.ToString("0")+"s";
            textLeft.text = "";
            textRight.text = "";
        }
	}


    
}
