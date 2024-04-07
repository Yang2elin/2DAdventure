using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> OnEventRaised;    //订阅
    public void RaiseEvent(Character character) //广播给订阅者
    {
        OnEventRaised?.Invoke(character);
    }
}
