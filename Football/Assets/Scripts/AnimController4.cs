using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController4 : MonoBehaviour {
    Animator animator;
    GameObject ball;

    Vector3 ball_initPosition;
    Quaternion ball_initRotation;
    Vector3 avatar_initPosition;
    Quaternion avatar_initRotation;
    //球门
    GameObject target;
    //两者之间的距离
    private float distanceToTarget;
    //最后一踢的目标
    
    GameObject camera_main;
    GameObject camera_end;

    //球飞出的速度
    public float speed = 10;
    //判断最后一下踢球是否还在移动
    private bool move = false;
    int playCount;
    public bool isPlaying = false;

    MianController mainController;
    MainUI mainUI;

    Timer timer;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        camera_main = GameObject.Find("Camera02");
        camera_end = GameObject.Find("Camera03");
        ball = GameObject.Find("Ball_AI");
        target = GameObject.Find("target");
        ball_initPosition = ball.transform.position;
        ball_initRotation = ball.transform.rotation;
        avatar_initPosition = this.transform.position;
        avatar_initRotation = this.transform.rotation;

        camera_main.SetActive(true);
        camera_end.SetActive(false);

        //计算两者之间的距离  
        distanceToTarget = Vector3.Distance(ball.transform.position, target.transform.position);
        //初始化踢球的次数
        playCount = 0;

        mainController = GameObject.Find("MainCamera").GetComponent<MianController>();
        mainUI = GameObject.Find("Canvas1").GetComponent<MainUI>();

        timer = new Timer(2.5f);
        timer.tick += Test;
        timer.Start();
    }

    void Test()
    {
        camera_main.SetActive(false);
        camera_end.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = this.transform.position;
        vector.x = 0;
        this.transform.position = vector;

        //Debug.Log("人与摄像机之间的距离  distance： " + distance);
        if (Input.GetKeyDown(KeyCode.L) && playCount < Global.PLAY_COUNT && mainController.content == "1")
        {
            //if (!isPlaying)
            //{
            //    isPlaying = true;
            //}
            playLeft();
            mainController.ChangeTimer(Global.TIME_ANIMATOR,"0");
            mainController.content = "0";
            mainUI.textLeft.text = Global.TEXT_LEFT;
            mainUI.textRight.text = "";
        }
        if (Input.GetKeyDown(KeyCode.R) && playCount < Global.PLAY_COUNT && mainController.content == "1")
        {
            //isPlaying = true;
            playRight();
            mainController.ChangeTimer(Global.TIME_ANIMATOR, "0");
            mainController.content = "0";
            mainUI.textLeft.text = "";
            mainUI.textRight.text = Global.TEXT_RIGHT;
        }

        if ( Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.R) && mainController.content == "1")
        {
            //最后一下，将球踢出去
            if (playCount == Global.PLAY_COUNT)
            {
                //if (Input.GetKeyDown(KeyCode.L))
                //{
                //    playLeft();
                //    mainUI.textLeft.text = Global.TEXT_LEFT;
                //    mainUI.textRight.text = "";
                //}
                //else if (Input.GetKeyDown(KeyCode.R))
                //{
                //    playRight();
                //    mainUI.textLeft.text = "";
                //    mainUI.textRight.text = Global.TEXT_RIGHT;
                //}
                playKick();
                Debug.Log("------------------"+playCount);
                //StartCoroutine("StartShoot");
                move = true;
            }
        }

        //游戏结束，切换相机
        if (playCount > Global.PLAY_COUNT)
        {
            timer.Update(Time.deltaTime);
            
        }

        //最后踢球
        if (move)
        {
            Shoot();
        }
    }
    //最后踢球
    void Shoot()
    {
        Vector3 targetPos = target.transform.position;
        //让始终它朝着目标  
        ball.transform.LookAt(targetPos);
        //计算弧线中的夹角  
        float angle = Mathf.Min(1, Vector3.Distance(ball.transform.position, targetPos) / distanceToTarget) * 45;
        ball.transform.rotation = ball.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
        float currentDist = Vector3.Distance(ball.transform.position, target.transform.position);
        if (currentDist < 0.5f)
            move = false;
        ball.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
    }

    ////射门左
    //IEnumerator StartShoot()
    //{

    //    while (move)
    //    {
    //        Vector3 targetPos = target.transform.position;

    //        //让始终它朝着目标  
    //        ball.transform.LookAt(targetPos);

    //        //计算弧线中的夹角  
    //        float angle = Mathf.Min(1, Vector3.Distance(ball.transform.position, targetPos) / distanceToTarget) * 45;
    //        ball.transform.rotation = ball.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
    //        float currentDist = Vector3.Distance(ball.transform.position, target.transform.position);
    //        if (currentDist < 0.5f)
    //            move = false;
    //        ball.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(1.2f);
    //    camera_main.SetActive(false);
    //    camera_end.SetActive(true);
    //}


    //左脚踢球
    void playLeft()
    {
        //踢球动作
        animator.SetTrigger("leftFirst");
        playCount++;
        Debug.Log(playCount);
    }
    //右脚踢球
    void playRight()
    {
        //踢球动作
        animator.SetTrigger("rightFirst");
        playCount++;
        Debug.Log(playCount);
    }

    void playKick()
    {
        animator.SetTrigger("kick");
        playCount++;

    }

    public void Restart()
    {
      
        //相机切换
        camera_main.SetActive(true);
        camera_end.SetActive(false);
        //球
        ball.transform.position = ball_initPosition;
        ball.transform.rotation = ball_initRotation;
        //人物返回
        this.transform.position = avatar_initPosition;
        this.transform.rotation = avatar_initRotation;

        camera_main.SetActive(true);
        camera_end.SetActive(false);
        distanceToTarget = Vector3.Distance(ball.transform.position, target.transform.position);
        playCount = 0;
        StopCoroutine("StartShoot");
    }



}
