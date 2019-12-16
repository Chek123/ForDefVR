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

    private int playerSoldiersCount;
    private int enemySoldiersCount;
    private int currentLevel = 1;

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
        ENEMY_TURN,
        ROLEPLAYING
    }

    public void LoadScene(int current_level)
    {
        //vymaz vsetko co na nej bolo.
        //nastav veci ktore su tam stale.
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

            playerSoldiersCount++;
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

    public void SetEnemyTurnMode()
    {
        gamemode = GameMode.ENEMY_TURN;
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

    public void CheckWinner()
    {
        if (enemySoldiersCount == 0)
        {
            Debug.Log("Player wins");

            GameObject.Find("SceneObjects/PostaPreTebaStena/WinPanel").SetActive(true);

            wall.GetComponent<Animator>().SetBool("GameFinished", true);
            //

        }
        else if (playerSoldiersCount == 0)
        {
            Debug.Log("Enemy wins");
            GameObject.Find("SceneObjects/PostaPreTebaStena/LosePanel").SetActive(true);

            wall.GetComponent<Animator>().SetBool("GameFinished", true);
        }
    }

    public void SetSoldiersCount(int newValue, string soldierType)
    {
        if (soldierType == "EnemySoldier")
        {
            this.enemySoldiersCount = newValue;
        }
        else
        {
            this.playerSoldiersCount = newValue;
        }
    }

    public int GetSoldiersCount(string soldierType)
    {
        if (soldierType == "EnemySoldier")
        {
            return enemySoldiersCount;
        }
        else
        {
            return playerSoldiersCount;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        edc = GetComponent<EnemyDataController>();
        sceneObjects.transform.localScale = PlayAreaRealSize.GetScaleFactor();
    }

    public static GameObject InstantateScaled(GameObject prefab, Transform parent)
    {
        var result = GameObject.Instantiate(prefab, parent) as GameObject;
        return ScaleObject(result);
    }

    public static GameObject InstantateScaled(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var result = GameObject.Instantiate(prefab, position, rotation) as GameObject;
        return ScaleObject(result);
    }

    private static GameObject ScaleObject(GameObject obj)
    {
        obj.transform.localScale = Vector3.Scale(obj.transform.localScale, PlayAreaRealSize.GetScaleFactor());
        return obj;
    }


}
