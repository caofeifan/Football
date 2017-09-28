using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController1 : MonoBehaviour {

    //Camera的状态  0 : 闲置状态，1: 向前移动，2: 向右旋转， 3: 向左旋转， 4: 暂停1.5s后顺时针旋转，5: 暂停1.5s后逆时针旋转
    public int cameraState;
    private Vector3 initPosition;
    private Quaternion initRotation;

    //旋转
    private Quaternion targetRotation;
    private Quaternion targetRotationBack;

    //人物
    GameObject avatar;
    AnimatorStateInfo stateInfo;
    //相机与人的初始距离
    float initDistance;
    //相机与人的动态距离
    float distance;
    //移动时间
    public float smoothTime = 10.0F;

    private Vector3 velocity = Vector3.zero;

    private Timer timer;


    // Use this for initialization
    void Start () {
        initPosition = this.transform.position;
        initRotation = this.transform.rotation;
        targetRotation = Quaternion.Euler(initRotation.x, 90, initRotation.z) * Quaternion.identity;
        targetRotationBack = Quaternion.Euler(initRotation.x, 0, 0) * Quaternion.identity;
        cameraState = 0;
        avatar = GameObject.Find("DefaultAvatar");
        Debug.Log("开始调用Camera");
        initDistance = Vector3.Distance(transform.position, avatar.transform.position);

        timer = new Timer(2f);
        timer.tick += Test1;
        timer.Start();

    }
	
	// Update is called once per frame
	void Update () {
        //    StartCoroutine(ControllerCamera());
        //Vector3 vector = avatar.transform.position;
        //vector.x = 0;
        //this.transform.position = vector;
        
        
        
	}

    private void FixedUpdate()
    {
        //获取相机与人的z轴距离
        distance = avatar.transform.position.z - this.transform.position.z;
        stateInfo = avatar.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        //移动相机
        if (stateInfo.IsName("Idle_Neutral") && distance > Global.DISTANCE_CAMERA_AVATAR)
        {
            transform.position = Vector3.SmoothDamp(transform.position,
                transform.position + new Vector3(0, 0, initDistance*5), ref velocity, smoothTime);
            cameraState = Global.WALKING;
        }
        switch (cameraState)
        {
            case Global.STOP:
                break;
            case Global.WALKING:
                //1: 向前移动
                //Debug.Log("开始向前行走");
                MoveForward();
                timer.Update(Time.deltaTime);
                break;
            case Global.TURNNING_RIGHT:
                //2: 逆时针旋转
                // Debug.Log("开始逆时针旋转");
                RotateTo(targetRotationBack);
                timer.Update(Time.deltaTime);
                break;
            case Global.TURNNING_LEFT:
                //3: 顺时针旋转
                // Debug.Log("开始顺时针旋转，状态 3");
                RotateTo(targetRotation);
                timer.Update(Time.deltaTime);
                break;
            case Global.WAITING_TURNLEFT:
                // 4: 暂停1.5s，顺时针旋转
               // Debug.Log("开始暂停,执行状态4");
                timer.Update(Time.deltaTime);
                break;
            case Global.WAITING_TURNRIGHT:
                // 5: 暂停1.5s，逆时针旋转
                //Debug.Log("开始暂停，状态 5");
                timer.Update(Time.deltaTime);
                break;
        }
    }
    //1: 向前移动
    void MoveForward()
    {
        //transform.position = Vector3.SmoothDamp(transform.position,
        //        transform.position + new Vector3(0, 0, initDistance), ref velocity, smoothTime);

    }
    //2: 旋转指定角度
    void RotateTo(Quaternion rotation)
    {
        if(rotation.eulerAngles.y > 0)
        {
              transform.rotation = Quaternion.Slerp(avatar.transform.rotation, targetRotation, Time.deltaTime*2);
            transform.RotateAround(avatar.transform.position, new Vector3(0, 1f, 0), 30f * Time.deltaTime);
            
        }
        else
        {
            //   transform.rotation = Quaternion.Slerp(avatar.transform.rotation, targetRotationBack, Time.deltaTime*2);
            //旋转回来
            
            transform.RotateAround(avatar.transform.position, new Vector3(0, 1f, 0), -30f * Time.deltaTime);
        }

    }
    // 4: 暂停1.5s
    void SetSuspend(int nextState)
    {
        StartCoroutine(TurnState(nextState, 2.5f));
    }

    void SetSuspend(int nextState, float time)
    {
        StartCoroutine(TurnState(nextState,time));
    }


    //暂停切换状态
    IEnumerator TurnState(int nextState, float time)
    {
        yield return new WaitForSeconds(time);
        if(nextState == 0)
        {
            avatar.GetComponent<AnimController4>().isPlaying = false;

        }
        cameraState = nextState;
        yield return null;
    }

    //顺时针转，前一个状态是 4， 后一个状态是 5
    IEnumerator ControllerCamera()
    {
        transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime);
        yield return new WaitForSeconds(5f);
        cameraState = 5;
        yield return null;
    }
    //顺时针转，前一个状态是 5， 后一个状态是 0
    IEnumerator ControllerCameraBack()
    {
        //等待1s，
        transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotationBack, Time.deltaTime);
        yield return new WaitForSeconds(5f);
        cameraState = 0;
        yield return null;
    }

    public void Restart()
    {
        cameraState = 0;
        this.transform.position = initPosition;
        this.transform.rotation = initRotation;

    }

    void Test1()
    {
        Debug.Log("==============");
        switch (cameraState)
        {
            
            case Global.STOP:
                break;
            case Global.WALKING:
                Debug.Log("==============1");
                cameraState = Global.WAITING_TURNLEFT;
                timer = new Timer(Global.TIME_WAITING_TURN_LEFT_4_3);
                timer.tick += Test1;
                timer.Start();
                break;
            case Global.TURNNING_RIGHT:
                Debug.Log("==============2");
                cameraState = Global.STOP;
                timer = new Timer(1f);
                timer.tick += Test1;
                timer.Start();
                
                break;
            case Global.TURNNING_LEFT:
                Debug.Log("==============3");
                timer = new Timer(Global.TIME_WAITING_TURN_RIGHT_5_2);
                timer.tick += Test1;
                timer.Start();
                cameraState = Global.WAITING_TURNRIGHT;
                break;
            case Global.WAITING_TURNLEFT:
                timer = new Timer(Global.TIME_TURNNING_LEFT_3_5);
                timer.tick += Test1;
                timer.Start();
                cameraState = Global.TURNNING_LEFT;
                Debug.Log("==============4=-----"+cameraState);
                break;
            case Global.WAITING_TURNRIGHT:
                Debug.Log("==============5");
                timer = new Timer(Global.TIME_TURNNING_RIGHT_2_0);
                timer.tick += Test1;
                timer.Start();
                cameraState = Global.TURNNING_RIGHT;

                break;
        }
        
    }
    private void OnDestroy()
    {
        timer.tick -= Test1;
    }
}
