using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VRTK;



[RequireComponent(typeof(EnemyDataController))]
public class GameManager : MonoBehaviour
{
    public GameMode gamemode = GameMode.LAYOUTING;
    public GameObject wall;
    public GameObject sceneObjects;
    private EnemyDataController edc;

    private static GameManager instance = null;
    public static GameManager Instance
    {
        get {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public enum GameMode
    {
        LAYOUTING, //TODO: vymysliet lepsi nazov pre rozkladanie panacikou po hracej ploche
        ENEMY_CHOOSING,
        ROLEPLAYING
    }

    public void StartLevel()
    {
        edc.LoadData();

        wall.GetComponent<Animator>().enabled = true;
        wall.GetComponent<AudioScript>().source.Play();

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

    public void SetEnemyChoosingMode()
    {
        gamemode = GameMode.ENEMY_CHOOSING;
    }

    public void SetRolePlayMode()
    {
        gamemode = GameMode.ROLEPLAYING;

        // maybe set actual soldier object..

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

    /*private void DestroyPolickaObjects()
    {
        // couldnt find a way to refactor more :(
        Destroy(GameObject.Find("Policka").gameObject);
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Spawning");
        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }*/

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
        edc = GetComponent<EnemyDataController>();
        //sceneObjects.transform.localScale = PlayAreaRealSize.GetScaleFactor();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
