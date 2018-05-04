using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePeerMode : MonoBehaviour {
    public Object serverScene, clientScene;
    public void BeServer() {
        //Debug.LogError("load: " + serverScene.name);
        SceneManager.LoadScene("03_server");
    }
    public void BeClient() {
        //Debug.LogError("load: " + clientScene.name);
        SceneManager.LoadScene("03_client");
    }
}
