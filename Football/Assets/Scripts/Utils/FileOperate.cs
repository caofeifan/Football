using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileOperate {

    public static void WriteFileByLine(string file_name, string str_info)
    {
        string file_path = System.Environment.CurrentDirectory;
        WriteFileByLine(file_path, file_name, str_info);
    }

    public static void WriteFileByLine(string file_path, string file_name, string str_info)
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
        Debug.Log("写入内容 ：" + str_info);
        sw.Close();
        sw.Dispose();//文件流释放  
    }
}
