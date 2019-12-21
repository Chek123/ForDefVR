using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VRTK;

[RequireComponent(typeof(EnemyDataController))]
public class GameManager : MonoBehaviour
{

    [System.Serializable]
    public class Level
    {
        public int level_id;
        public int soldier_1;
        public int soldier_2;
        public int soldier_3;
        public int soldier_4;
        public int soldier_5;
        public int soldier_6;
    }

    [System.Serializable]
    public class PlayerData
    {
        public Level[] levels;
    }
    public GameMode gamemode = GameMode.LAYOUTING;
    public GameObject wall;
    public GameObject sceneObjects;
    public GameObject standardMenu;
    public GameObject noSoldiersPlacedMenu;
    public GameObject noSoldiersWithWeaponsPlacedMenu;
    public GameObject someSoldiersLeftMenu;
    public int polickoGridSize;

    public AudioSource winningSound;
    public AudioSource loosingSound;
    public AudioSource backgroundMusic;

    private EnemyDataController edc;

    private int playerSoldiersCount;
    private int enemySoldiersCount;
    private int startEnemySoldiersCount;
    private int maxSoldierHP;
    public static int currentLevel = 1;

    public Level levelData;

    private static GameManager instance = null;

    private SoldierCheckState currentSoldierCheckState = SoldierCheckState.OK;
    private bool confirmedToContinue = false;

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

    public static void Reset(int level)
    {
        Debug.Log("setting level:" + level);
        instance = null;
        currentLevel = level;
    }

    public enum GameMode
    {
        LAYOUTING, //TODO: vymysliet lepsi nazov pre rozkladanie panacikou po hracej ploche
        PLAYER_TURN,
        ENEMY_TURN,
        ROLEPLAYING,
        MENU
    }

    enum SoldierCheckState
    {
        NoSoldierPlaced,
        SomeSoldiersLeft,
        OK
    }

    public void StartLevel()
    {
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Vojak");

        confirmedToContinue = currentSoldierCheckState == SoldierCheckState.SomeSoldiersLeft ? true : false;

        currentSoldierCheckState = CheckSoldiers(soldiers);

        if (currentSoldierCheckState == SoldierCheckState.OK || (currentSoldierCheckState == SoldierCheckState.SomeSoldiersLeft && confirmedToContinue))
        {
            edc.LoadData();
            gamemode = GameMode.PLAYER_TURN;

            wall.GetComponent<Animator>().enabled = true;

            HidePolickaObjects();

            foreach (var go in soldiers)
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

                var weapon = go.transform.Find("Weapon");
                if (weapon)
                {
                    playerSoldiersCount++;
                }
            }            
        }
    }

    private SoldierCheckState CheckSoldiers(GameObject[] soldiers)
    {
        bool noSoldiersPlaced = true;
        bool noSoldiersWithWeapons = true;
        bool soldiersRemaining = false;
        
        int scaledSoldiersCounter = 0;
        
        foreach(var soldier in soldiers)
        {
            Debug.Log(soldier.gameObject.name);
            PlacableObject placableObject = soldier.GetComponent<PlacableObject>();
            if (!placableObject.getIsScaled())
            {
                if (!placableObject.GetIsInSpawningArea())
                {
                    placableObject.WrongPlacement();
                }
                soldiersRemaining = true;
            }
            else {
                string name = soldier.gameObject.name;
                Debug.Log(name);
                if (!name.Contains("Bait") && !name.Contains("Medic"))
                {
                    noSoldiersWithWeapons = false;
                }
                noSoldiersPlaced = false;
                scaledSoldiersCounter++;
            }
        }

        if (scaledSoldiersCounter == polickoGridSize)
        {
            return SoldierCheckState.OK;
        }

        if (noSoldiersPlaced)
        {
            Debug.Log("No soldiers placed");
            standardMenu.SetActive(false);
            noSoldiersPlacedMenu.SetActive(true);
            noSoldiersWithWeaponsPlacedMenu.SetActive(false);
            someSoldiersLeftMenu.SetActive(false);
            return SoldierCheckState.NoSoldierPlaced;
        }

        if (noSoldiersWithWeapons)
        {
            Debug.Log("No soldiers with weapons placed");
            standardMenu.SetActive(false);
            noSoldiersPlacedMenu.SetActive(false);
            noSoldiersWithWeaponsPlacedMenu.SetActive(true);
            someSoldiersLeftMenu.SetActive(false);
            return SoldierCheckState.NoSoldierPlaced;
        }

        if (soldiersRemaining)
        {
            Debug.Log("Some soldiers remaining");
            standardMenu.SetActive(false);
            noSoldiersPlacedMenu.SetActive(false);
            noSoldiersWithWeaponsPlacedMenu.SetActive(false);
            someSoldiersLeftMenu.SetActive(true);
            return SoldierCheckState.SomeSoldiersLeft;
        }
        
        return SoldierCheckState.OK;
    }

    public void SetPlayerTurnMode()
    {
        gamemode = GameMode.PLAYER_TURN;
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
            
            // enable winning animation
            wall.GetComponent<Animator>().SetBool("GameFinished", true);
            wall.GetComponent<Animator>().SetBool("WinLevel", true);
    
            backgroundMusic.Stop();
            winningSound.Play(0);

            gamemode = GameMode.MENU;

            Debug.Log("finishedLvl"+(GameManager.currentLevel - 1));

            PlayerPrefs.SetInt("finishedLvl" +(GameManager.currentLevel-1), 1);

        }
        else if (playerSoldiersCount == 0)
        {
            Debug.Log("Enemy wins");

            // enable losing annimation
            wall.GetComponent<Animator>().SetBool("GameFinished", true);
            wall.GetComponent<Animator>().SetBool("LoseLevel", false);

            gamemode = GameMode.MENU;

            backgroundMusic.Stop();
            loosingSound.Play(0);

        }
    }

    public void SetPlayerSoldiersCount(int newValue)
    {
        playerSoldiersCount = newValue;
    }

    public void SetEnemySoldiersCount(int newValue)
    {
        enemySoldiersCount = newValue;
    }

    public void SetMaxSoldierHP(int newValue)
    {
        maxSoldierHP = newValue;
    }

    public void SetStartEnemySoldiersCount(int newValue)
    {
        startEnemySoldiersCount = newValue;
    }

    public int GetPlayerSoldiersCount()
    {
        return playerSoldiersCount;
    }

    public int GetEnemySoldiersCount()
    {
        return enemySoldiersCount;
    }

    public int GetMaxSoldierHP()
    {
        return maxSoldierHP;
    }

    public int GetStartEnemySoldiersCount()
    {
        return startEnemySoldiersCount;
    }

    // Start is called before the first frame update
    void Awake()
    {
        edc = GetComponent<EnemyDataController>();
        sceneObjects.transform.localScale = PlayAreaRealSize.GetScaleFactor();

        var playerData = JsonUtility.FromJson<PlayerData>((Resources.Load("Database/player_data") as TextAsset).text);
        Debug.Log("Level" + currentLevel);
        levelData = playerData.levels.Where(x => x.level_id == currentLevel).FirstOrDefault();
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
