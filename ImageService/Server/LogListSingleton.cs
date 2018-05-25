using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;

public class LogListSIngleton
{
    private static LogListSIngleton instance = null;
    private static List<string> list;


    public LogListSIngleton()

	{
        list = new List<string>();
    }
   

    public static LogListSIngleton Instance
    {
        get
        {

            if (instance == null)
            {
                instance = new LogListSIngleton();
            }
            return instance;

        }
    }


    public void AddItem(object sender, MessageRecievedEventArgs e)
    {
        list.Add(e.Status.ToString()+";"+e.Message);
    }


    public string getListAsString()
    {
        string ret = "";
        foreach(string item in list)
        {
            ret +=item+"|";
        }
        ret = ret.Remove(ret.Length - 1);
        return ret;
    }


}
