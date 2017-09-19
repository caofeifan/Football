using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MianController : MonoBehaviour {

    public string filePath;
    public Timer timer;
    public string content;

    // Use this for initialization
    void Start () {
        filePath = System.Environment.CurrentDirectory;
        Debug.Log(filePath);
        content = "0";
        WriteFileByLine(filePath, "test.txt",content);
        timer = new Timer(5f);
        timer.tick += Test;
        timer.Start();
    }
	
	// Update is called once per frame
	void Update () {
        timer.Update(Time.deltaTime);
        
	}

    public void WriteFileByLine(string file_path, string file_name, string str_info)
    {
        StreamWriter sw;
        if (!File.Exists(file_path + "//" + file_name))
        {
            sw = File.CreateText(file_path + "//" + file_name);//创建一个用于写入 UTF-8 编码的文本  
            Debug.Log("文件创建成功！");
        }
        else
        {
            //打开现有 UTF-8 编码文本文件以进行读取
            sw = new StreamWriter(File.Open(file_path + "//" + file_name, FileMode.Open));
            Debug.Log("打开现有的文件");
        }
        sw.WriteLine(str_info);//以行为单位写入字符串  
        Debug.Log("写入内容 ："+ str_info);
        sw.Close();
        sw.Dispose();//文件流释放  
    }

    void Test()
    {
        //采集脑电信号
        if (content.Equals("0"))
        {
            content = "1";
            ChangeTimer(Global.TIME_GATHER, content);
        }
        //等待播放动画
        else
        {
            content = "0";
            ChangeTimer(Global.TIME_ANIMATOR, content);
        }
    }

    public void ChangeTimer(float time)
    {
        timer.tick -= Test;   
        timer.Stop();
        timer = new Timer(time);
        timer.tick += Test;
        timer.Start();
    }

    public void ChangeTimer(float time, string content)
    {
        WriteFileByLine(filePath, "test.txt", content);
        timer.tick -= Test;
        timer.Stop();
        timer = new Timer(time);
        timer.tick += Test;
        timer.Start();
    }

    private void OnDestroy()
    {
        timer.tick -= Test;
    }
}
