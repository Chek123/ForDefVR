using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonVRClicker : MonoBehaviour {

    private Button button;

	// Use this for initialization
	void Start () {
        GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += ObjectUsed;
        GetComponent<VRTK_InteractableObject>().InteractableObjectUnused += ObjectUnused;
        GetComponent<VRTK_InteractableObject>().InteractableObjectTouched += ObjectTouched;
        GetComponent<VRTK_InteractableObject>().InteractableObjectUntouched += ObjectUntouched;

        button = GetComponent<Button>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void ObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        if (button.interactable)
        {
            button.image.color = button.colors.pressedColor;
        }
    }

    private void ObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        if (button.interactable)
        {
            button.image.color = button.colors.highlightedColor;
            button.onClick.Invoke();
        }
    }

    private void ObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        if (button.interactable)
        {
            button.image.color = button.colors.highlightedColor;
        }
    }

    private void ObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        if (button.interactable)
        {
            button.image.color = button.colors.normalColor;
        }
    }
}
