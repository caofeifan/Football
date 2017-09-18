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
            tips.text = "未采集";
        }
        else
        {
            tipImage.SetActive(true);
            tips.text = "采集脑电信号中";
            textLeft.text = "";
            textRight.text = "";
        }
	}


    
}
