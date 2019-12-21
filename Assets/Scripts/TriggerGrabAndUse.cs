using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/**
 * Umozni pouzivatelovi vziat objekt pouzit ho a pustit jednym hlavnym tlacitkom 
 * (Ulahcuje ovladanie pre zbrane a navrat do modu vyberania vojakov)
 */ 
[RequireComponent(typeof(VRTK_InteractableObject))]
public class TriggerGrabAndUse : MonoBehaviour
{

    private VRTK_InteractableObject intObj;

    private VRTK_ControllerEvents.ButtonAlias originalAliasGrab;

    private List<VRTK_InteractGrab> controlers;

    // Init
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

    /**
     * Po vykonani akcie za vojaka (napr vystreleni zo zbrane) sa nastavi tlacitko na pustenie
     * predmetu na hlavne (trigger)
     */ 
    public void ActionFinished()
    {
        SetGrabButton(VRTK_ControllerEvents.ButtonAlias.TriggerPress);
    }

    /**
     * Po zobrati zbrane sa zmeni tlacitko na pustenie zbrane na vedlajsie (ktore sa tazsie stlaca)
     */ 
    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        intObj.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.GripPress;
        SetGrabButton(VRTK_ControllerEvents.ButtonAlias.GripPress);
    }

    /**
     * Po pusteni zbrane sa obnovia povodne nastavenia na grab zbrane
     */ 
    private void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        intObj.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;
        SetGrabButton(originalAliasGrab);
    }

    /**
     * Pomocna metoda ktora nastavi vsetkym ovladacom grab button na paramter
     * @param btn novy grab button 
     */ 
    private void SetGrabButton(VRTK_ControllerEvents.ButtonAlias btn)
    {
        foreach (var contoller in controlers)
        {
            contoller.grabButton = btn;
        }
    }
}
