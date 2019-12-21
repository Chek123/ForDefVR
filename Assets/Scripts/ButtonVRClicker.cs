using UnityEngine;
using VRTK;
using UnityEngine.UI;

/**
 * Allows to click UI button by controller in VR
 */
public class ButtonVRClicker : MonoBehaviour {

    // Button to click
    private Button button;

	// initialization
	void Start () {
        GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += ObjectUsed;
        GetComponent<VRTK_InteractableObject>().InteractableObjectUnused += ObjectUnused;
        GetComponent<VRTK_InteractableObject>().InteractableObjectTouched += ObjectTouched;
        GetComponent<VRTK_InteractableObject>().InteractableObjectUntouched += ObjectUntouched;

        button = GetComponent<Button>();

    }

    /**
     * Sets pressedColor to button 
     * @param sender controller used
     * @param e event 
     */
    private void ObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        if (button.interactable)
        {
            button.image.color = button.colors.pressedColor;
        }
    }

    /**
     * Sets highlightedColor to button and invoke button actions
     * @param sender controller used
     * @param e event 
     */
    private void ObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        if (button.interactable)
        {
            button.image.color = button.colors.highlightedColor;
            button.onClick.Invoke();
        }
    }

    /**
     * Sets highlightedColor to button
     * @param sender controller used
     * @param e event 
     */
    private void ObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        if (button.interactable)
        {
            button.image.color = button.colors.highlightedColor;
        }
    }

    /**
     * Sets normalColor to button
     * @param sender controller used
     * @param e event 
     */
    private void ObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        if (button.interactable)
        {
            button.image.color = button.colors.normalColor;
        }
    }
}
