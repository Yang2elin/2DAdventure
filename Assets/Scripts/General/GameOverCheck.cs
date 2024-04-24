using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCheck : MonoBehaviour
{
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;
    public SceneLoadEventSO loadEventSO;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("GameOver");
            loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);   //呼叫
        }

    }
}
