using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using System.Linq;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class TriggerGrabAndUse : MonoBehaviour
{

    private VRTK_InteractableObject intObj;

    private VRTK_ControllerEvents.ButtonAlias originalAliasGrab;

    private List<VRTK_InteractGrab> controlers;

    // Start is called before the first frame update
    void Start()
    {
        intObj = GetComponent<VRTK_InteractableObject>();
        intObj.InteractableObjectGrabbed += ObjectGrabbed;
        intObj.InteractableObjectGrabbed += ObjectUngrabbed;

        controlers = new List<VRTK_InteractGrab>();
        foreach(var go in GameObject.FindGameObjectsWithTag("Controller"))
        {
            controlers.Add(go.GetComponent<VRTK_InteractGrab>());
        }
        originalAliasGrab = controlers[0].grabButton;
    }

    public void ActionFinished()
    {
        SetGrabButton(VRTK_ControllerEvents.ButtonAlias.TriggerPress);
    }

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        intObj.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
        SetGrabButton(VRTK_ControllerEvents.ButtonAlias.GripPress);
    }

    private void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        intObj.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
        SetGrabButton(originalAliasGrab);
    }

    private void SetGrabButton(VRTK_ControllerEvents.ButtonAlias btn)
    {
        foreach (var contoller in controlers)
        {
            contoller.grabButton = btn;
        }
    }
}
