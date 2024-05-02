using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCheck : MonoBehaviour
{
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;
    public SceneLoadEventSO loadEventSO;
    public Character player;

    // private void Update()
    // {
    //     if (player.currentHealth <= 0)
    //     {
    //         ReSetGame();
    //     }
    // }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("GameOver");
            ReSetGame();
        }

    }
    public void ReSetGame()
    {
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);   //呼叫
    }
}
