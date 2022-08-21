using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTooltip : MonoBehaviour
{
    public UnityEvent OnCall = new UnityEvent();

    public void CallWithCheck()
    {
        if(GameController.controller.tutorial)
        {
            OnCall.Invoke();
        }
    }
}
