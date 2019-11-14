using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VRTK;
public class GameManager : MonoBehaviour
{
    public GameMode gamemode = GameMode.LAYOUTING;
    public GameObject wall;
    private EnemyDataController data;

    public enum GameMode
    {
        LAYOUTING, //TODO: vymysliet lepsi nazov pre rozkladanie panacikou po hracej ploche
        ROLEPLAYING
    }

    public void SetRolePlayMode()
    {
        gamemode = GameMode.ROLEPLAYING;
        data = new EnemyDataController();
        data.LoadData();

        return;
        //wall.GetComponent<Animator>().enabled = true;
        //wall.GetComponent<AudioScript>().source.Play();




        // ak vymazem spawning a prevtelim sa potom do vojaka, hadze to errory. Preto zatial iba schovam objekty
        // po zacati hry, a ak by sa to vyriesilo tak ich rovno mozeme mazat.

        //DestroyPolickaObjects();
        HidePolickaObjects();

        foreach (var go in GameObject.FindGameObjectsWithTag("Vojak"))
        {
            Debug.Log("Changing object: " + go.name);
            Debug.Log(go.GetComponent<PlacableObject>().getIsScaled());
            if (!go.GetComponent<PlacableObject>().getIsScaled())
            {
                //Destroy(go.gameObject);
                go.gameObject.SetActive(false);
                continue;
            }
            go.GetComponent<PlacableObject>().enabled = false;
            go.GetComponent<PlayableObject>().enabled = true;
            go.GetComponent<HealthControl>().enabled = true;
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

    private void DestroyPolickaObjects()
    {
        // couldnt find a way to refactor more :(
        Destroy(GameObject.Find("Policka").gameObject);
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawning");
        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }

    private void HidePolickaObjects()
    {
     
        Destroy(GameObject.Find("Policka").gameObject);
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawning");
        for (var i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
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
