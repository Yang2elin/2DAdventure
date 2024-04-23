using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;
    public SceneLoadEventSO loadEventSO;
    public void TriggerAction()
    {
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);   //呼叫
    }


}
