using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class getInf : MonoBehaviour
{
    public Transform contentWindow;
    public GameObject recallTextObj;

    void Start()
    {
        string readFromFilePath = Application.streamingAssetsPath + "/Recall_Info/" + "info" + ".txt";
        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();
        foreach(string line in fileLines)
        {
            Instantiate(recallTextObj, contentWindow);
            recallTextObj.GetComponent<Text>().text = line;
        }
    }
}
