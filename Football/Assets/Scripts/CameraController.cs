using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    //--旋转速度  
    private float xSpeed = 125.0f;
    private float ySpeed = 60.0f;

    //--移动速度  
    private float xMoveSpeed = 200;
    private float yMoveSpeed = 200;
    private float zMoveSpeed = 200;
    //-----------------  移动限制  -----------------  

    //--是否允许移动相机  
    public static bool IsCanMove;
    //--是否允许旋转相机  
    public static bool IsCanRotate;

    //--是否允许缩放相机  
    public static bool IsCanZoom;

    //--是否开启位置限制  
    private bool IsCheck;

    //--镜头与中心点距离，在 IsYYFD 变量为 true 时有效  
    public static float JvLi;

    //--移动限制  

    //z轴最大值、最小值  
    private float zMaxMove = 44;
    private float zMinMove = -44;

    //中心点物体  
    public static Transform Tanks;

    //位置控制  
    private Vector3 position;
    //角度控制  
    private Quaternion rotation1;
    //相机动画控制：0停止；1旋转指定角度；2旋转到某个角度  
    static int Ani_Camera;
    //旋转动画的角度  

    static float XAniAngle = 0f;
    static float YAniAngle = 0f;
    static float ZAniAngle = 0f;
    //旋转动画的方向  
    static bool XFX;
    static bool YFX;
    static bool ZFX;
    //是否强行跳转  
    static bool IsQXTZ = false;
    //是否跟随事件移动  
    public static bool IsFollow = false;
    //锁定物体名称  
    static string LockModName = "";
    //旋转速度  
    static float XZSD = 1;

    //旋转角度（二维屏幕）  
    public static float y = 90.0f;
    public static float x = 0.0f;
    public static float z = 0.0f;
    //坐标还原点  
    private Vector3 LPosition;
    //中心物体坐标还原点  
    private Vector3 LTPosition;

    //相机旋转到某个角度  
    public static void CameraRotateTo(string ModName, float XAngle, float YAngle, float ZAngle, float Velocity, bool LsFollow)
    {
        //throw new Exception(ModName);  
        //设置是否跟随视角  
        IsFollow = LsFollow;
        //设置相机位置  
        LockModName = ModName;
        //设置旋转角度  
        XAniAngle = XAngle;
        YAniAngle = YAngle;
        ZAniAngle = ZAngle;
        //设置速度  
        XZSD = Velocity;
        //定义偏移速度  
        XFX = (XAniAngle > x) ? true : false;
        YFX = (YAniAngle > y) ? true : false;
        ZFX = (ZAniAngle > z) ? true : false;
        //开启动画控制  
        Ani_Camera = 2;

    }

    //停止旋转  
    public static void StopXZ()
    {
        Ani_Camera = 0;
    }

    //格式化X、Y、Z轴  
    public static void FormatXYZ()
    {
        y = y % 360;
        x = x % 360;
        z = z % 360;
        //y = (360+y) % 360;  
        //x = (360+x) % 360;  
        //z = (360+z) % 360;  
        //if (y < 0) y = 360 + y;  
        //if (x < 0) x = 360 + x;  
        //if (z < 0) z = 360 + z;  
    }

    // Use this for initialization
    void Start () {
        //初始化中心模块  
        Tanks = GameObject.Find("DefaultAvatar").transform;
        //初始化相机动画控制器  
        Ani_Camera = 0;
        //初始化相机控制权，默认全部允许  
        IsCanMove = true;
        IsCanRotate = true;
        IsCanZoom = true;
        //开启移动范围距离  
        IsCheck = true;
        //关闭相机视角锁定  
        IsFollow = false;
        //初始化相机位置  
        transform.position = new Vector3(0f, 2.01f, -12.8f);
        //初始化相机与中心模型的距离  
        JvLi = 1.7f;
        //初始化相机角度  
        x = 25.2f; y = 0; z = 0;
        //初始化中心物体位置  
        Tanks.position = new Vector3(0f, 0.0f, -11.23f);
        //初始化中心物体角度  
        Tanks.rotation = Quaternion.Euler(0, 0, 0);
        //强制旋转  
        IsQXTZ = true;
    }

    //使相机跳转到模型  
    public static bool MoveToMod(string ModName)
    {
        if (ModName == null || ModName == "") return false;
        //获得模型名称的GameObject  
        Transform newTran = GameObject.Find(ModName).transform;
        //如果获取到了  
        if (newTran != null)
        {
            //设置相机位置  
            Tanks.position = newTran.position;
            IsQXTZ = true;
            //返回成功  
            return true;
        }
        //如果失败  
        else
        {
            //返回假  
            return false;
        }
    }

    // Update is called once per frame
    void Update () {
    //    if (Tanks == null) return;

    //    if (IsFollow) MoveToMod(LockModName);

    //    if (Ani_Camera == 1)
    //    {

    //        //当还有剩余旋转角度则继续旋转  
    //        if (XAniAngle > 0)
    //        {
    //            //横向坐标加旋转速度  
    //            x += XZSD;
    //            //剩余角度减去速度  
    //            XAniAngle -= XZSD;
    //        }
    //        if (YAniAngle > 0)
    //        {
    //            //横向坐标加旋转速度  
    //            y += XZSD;
    //            //剩余角度减去速度  
    //            YAniAngle -= XZSD;
    //        }
    //        //如果没有剩余旋转角度  
    //        if (YAniAngle <= 0 && XAniAngle <= 0)
    //        {
    //            //设置强制跳转无效  
    //            IsQXTZ = false;
    //            //关闭动画控制器  
    //            Ani_Camera = 0;
    //            //取消跟踪  
    //            IsFollow = false;
    //        }
    //    }
    //    //获取相机与模型的距离  
    //    distance = Vector3.Distance(transform.position, Tanks.position);
    //    //设置角度  
    //    rotation1 = Quaternion.Euler(y, x, z);
    //    //设置位置  
    //    position = rotation1 * new Vector3(0.0f, 0.0f, -distance) + Tanks.position;
    //    //更新角度  
    //    Tanks.rotation = rotation1;

    }



}
