
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class AnimController5 : MonoBehaviour {
    //任务的状态
    public const int STATUS_IDLE = 3;
    public const int STATUS_KICK = 4;
    public const int STATUS_WAITTING = 5;
    public const int STATUS_WALKING = 6;
    public const int STATUS_FINISH = 7;
    public const int STATUS_START = 8;
    public const int WALKING = 9;  //任务行走的状态


    private const float TIME_IDLE = 7.0f;
    private const float TIME_KICK = 0.9f;
    private const float TIME_WAITTING = 0.3f;
    private const float TIME_WALKING = 2.5f;

    private const int PLAY_COUNT = 6;


    
    public int curStatus;
    private Timer timer;
    private int playCount;
    private Animator animator;
    private GameObject ball;
    //两者之间的距离
    private float distanceToTarget;
    //球飞出的速度
    public float speed = 10;
    //判断最后一下踢球是否还在移动
    private bool move = false;

    private GameObject target;

    float initDistance;
    float distance;
    AnimatorStateInfo stateInfo;
    CameraController2 controller2;

    Vector3 ball_initPosition;
    Vector3 avatar_initPosition;
    //文件相关
    public string filePath;
    public string content;
    Quaternion targetRotation;
    float[] intervals;
    float delay;
    MainUI mainUI;

    // Use this for initialization
    void Start () {
        Init();
        filePath = System.Environment.CurrentDirectory;
        animator = this.GetComponent<Animator>();
        ball = GameObject.Find("Ball_AI");
        initDistance = Vector3.Distance(ball.transform.position, this.transform.position);
        controller2 = GameObject.Find("DefaultAvatar/Main Camera").GetComponent<CameraController2>();

        ball_initPosition = ball.transform.position;
        avatar_initPosition = this.transform.position;

        mainUI = GameObject.Find("Canvas1").GetComponent<MainUI>();
        
        
        intervals = new float[6];
        string times = GenerateRandom();
        FileOperate.WriteFileByLine(filePath, "time.txt", times);
    }

    private void Init()
    {
        playCount = 0;
        curStatus = 1;
        timer = new Timer(20f);
        timer.tick += Listener;
        timer.Start();
    }

    // Update is called once per frame
    void Update () {
        timer.Update(Time.deltaTime);
        //矫正位置
        Vector3 vector = this.transform.position;
        vector.x = 0;
        this.transform.position = vector;
        //获取相机与人的z轴距离
        distance = ball.transform.position.z - this.transform.position.z;
        stateInfo = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        //移动相机 等待状态  waitting---> idle
        if (stateInfo.IsName("WalkFWD") && distance <= initDistance+0.1)
        {
            animator.SetBool("walk", false);
            curStatus = STATUS_START;
            timer.tick -= Listener;
            timer.Stop();
            timer = new Timer(9f);
            timer.tick += Listener;
            timer.Start();
            controller2.delay = 4f;
        }


        if (Input.GetKeyDown(KeyCode.L ) && controller2.content == "1")
        {
            controller2.content = "0";
            delay = 0;
            KickBall('l');
            mainUI.textLeft.text = Global.TEXT_LEFT;
            mainUI.textRight.text = "";
            mainUI.tipImage.SetActive(false);
            timer.Stop();
            timer.tick -= Listener;
            timer = new Timer(TIME_KICK);
            timer.tick += Listener;
            timer.Start();

        }
        if (Input.GetKeyDown(KeyCode.R) && controller2.content.Equals("1"))
        {
            controller2.content = "0";
            delay = 0;
            KickBall('r');
            mainUI.textLeft.text = "";
            mainUI.textRight.text = Global.TEXT_RIGHT;
            mainUI.tipImage.SetActive(false);
            timer.Stop();
            timer.tick -= Listener;
            timer = new Timer(TIME_KICK);
            timer.tick += Listener;
            timer.Start();
        }

        if (Input.GetKeyDown(KeyCode.G) && controller2.content.Equals("2"))
        {
            controller2.content = "0";
            delay = 0;
            animator.SetBool("walk", true);
            curStatus = WALKING;
            mainUI.tipImage.SetActive(false);
        }
        if (curStatus == STATUS_KICK)
        {
            delay += Time.deltaTime;
            if (delay > 0.15f)
            {
                ball.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5);
            }
        }
    }
    //踢球
    private void KickBall(char v)
    {
        playCount++;
        timer = new Timer(TIME_KICK);
        timer.tick += Listener;
        timer.Start();
        curStatus = STATUS_KICK;
        if (v.Equals('l'))
        {
            animator.SetTrigger("left");
        }else if (v.Equals('r'))
        {
            animator.SetTrigger("right");
        }
    }

    private void WalkingFword()
    {
        

    }

    //计时监听器
    void Listener()
    {
        timer.Stop();
        timer.tick -= Listener;
        switch (curStatus)
        {
            case 1:
                curStatus = 1;
                timer = new Timer(20);
                timer.tick += Listener;
                timer.Start();
                break;
            case STATUS_KICK:
                if (playCount < PLAY_COUNT)
                {
                    Debug.Log("当前状态：KICK 结束, 下一个状态：WAITTING !");
                    curStatus = STATUS_WAITTING;
                    timer = new Timer(TIME_WAITTING);
                    timer.tick += Listener;
                    timer.Start();
                }
                else
                {
                    curStatus = STATUS_FINISH;
                }
                break;
            case STATUS_WAITTING:
                curStatus = STATUS_WALKING;
                timer = new Timer(1f);
                timer.tick += Listener;
                timer.Start();
                break;
            case STATUS_WALKING:
                //Debug.Log("当前状态：WALK结束, 下一个状态：IDLE， the playCount = "+playCount);
                curStatus = 2;
                timer = new Timer(1f);
                timer.tick += Listener;
                timer.Start();
                mainUI.textLeft.text = "";
                mainUI.textRight.text = "";
                mainUI.tipImage.SetActive(true);
                ////设置文件内容
                //content = "1";
                //WriteFileByLine(filePath, "test.txt", content);
                //   animator.SetBool("walk", true);
                break;
            case 2:
               
                break;

            case STATUS_FINISH:
                Debug.Log("Finish!!!");
                curStatus = 1;
                timer = new Timer(20);
                timer.tick += Listener;
                timer.Start();
                break;
            case STATUS_START:
                Debug.Log("START结束!!!");
                curStatus = STATUS_IDLE;
                break;

        }


    }
    public void Restart()
    {
        Init();
        this.transform.position = avatar_initPosition;
        ball.transform.position = avatar_initPosition;
    }


    string GenerateRandom()
    {
        StringBuilder newRandom = new StringBuilder(62);
        System.Random rd = new System.Random();
        for (int i = 0; i < 6; i++)
        {
        //    intervals[i] = rd.Next(4,8);
            newRandom.Append(rd.Next(4, 8) + " ");
        }
        string str = newRandom.ToString();
        return str.Substring(0,str.Length-1);
    }

}
