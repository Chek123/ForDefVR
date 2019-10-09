using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VRTK;
public class GameManager : MonoBehaviour
{
    public GameMode gamemode = GameMode.LAYOUTING;

    public enum GameMode
    {
        LAYOUTING, //TODO: vymysliet lepsi nazov pre rozkladanie panacikou po hracej ploche
        ROLEPLAYING
    }

    public void SetRolePlayMode()
    {
        gamemode = GameMode.ROLEPLAYING;
        foreach(var go in GameObject.FindGameObjectsWithTag("Vojak"))
        {
            go.GetComponent<PlacableObject>().enabled = false;
            go.GetComponent<PlayableObject>().enabled = true;

            var interactable = go.GetComponent<VRTK_InteractableObject>();
            interactable.isGrabbable = false;
        }
        

    }

    public void SetLayoutMode()
    {
        gamemode = GameMode.LAYOUTING;
        foreach (var go in GameObject.FindGameObjectsWithTag("Vojak"))
        {
            go.GetComponent<PlayableObject>().enabled = false;
            go.GetComponent<PlacableObject>().enabled = true;
            var interactable = go.GetComponent<VRTK_InteractableObject>();
            interactable.isGrabbable = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
