using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global  {
    public  const int PLAY_COUNT = 6;
    public const float DISTANCE_CAMERA_AVATAR = 2.0f;
    public const string TEXT_LEFT = "左";
    public const string TEXT_RIGHT = "右";

    //相机动作时间
    public const float TIME_WAITING_TURN_LEFT_4_3 = 1.5f;
    public const float TIME_TURNNING_LEFT_3_5 = 3.0f;
    public const float TIME_WAITING_TURN_RIGHT_5_2 = 4f;
    public const float TIME_TURNNING_RIGHT_2_0 = 3.0f;

    //相机动作编号
    public const int STOP = 0;
    public const int WALKING = 1;
    public const int WAITING_TURNLEFT = 4;
    public const int TURNNING_LEFT = 3;
    public const int WAITING_TURNRIGHT = 5;
    public const int TURNNING_RIGHT = 2;

    //相机移动时间
    public const float TIME_ANIMATOR = 19.0f;
    public const float TIME_GATHER = 5;
}
