using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;



/// <summary>
/// we will use this class in order to consistenly update the
/// list of logs, it will be updated every time we will use the .log command
/// as we registered it to the event
/// </summary>
public class LogListSIngleton
{
    private static LogListSIngleton instance = null;
    private static List<string> list;

    //building the list
    public LogListSIngleton()

	{
        list = new List<string>();
    }
   

    /// <summary>
    /// instance to the list
    /// </summary>
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

    /// <summary>
    /// adding item to the list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void AddItem(object sender, MessageRecievedEventArgs e)
    {
        list.Add(e.Status.ToString()+";"+e.Message);
    }

    /// <summary>
    /// sending the list as a string on an agreed format between the client and the server
    /// </summary>
    /// <returns></returns>
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
