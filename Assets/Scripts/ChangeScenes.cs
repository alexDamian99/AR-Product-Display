using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScenes : MonoBehaviour
{
    public string sceneName;
    public InputField textInput;
    public void Change()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeWithCond()
    {
        if(textInput != null)
        {
            if (textInput.text != "")
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            Debug.Log("null input field");
        }

    }
}
