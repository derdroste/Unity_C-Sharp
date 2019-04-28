using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class menuStart : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}