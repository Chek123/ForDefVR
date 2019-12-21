using UnityEngine;

public class Spawning : MonoBehaviour
{
    private int vojakSpawnPocetnost;

    [SerializeField]
    private TextMesh pocetnostCounter;

    [SerializeField]
    private GameObject lastVojakSpawned;

    [SerializeField]
    private GameObject vojakPrefab;

    [SerializeField]
    private int soldierId = 0;

    [SerializeField]
    private WeaponController weaponController;

    /**
       * A normal member called on Start - includes loading of number of Soldiers to be available.
       */
    void Start()
    {
        Debug.Log(GameManager.Instance.levelData);
        switch(soldierId)
        {
            case 1:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_1;
                break;
            case 2:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_2;
                break;
            case 3:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_3;
                break;
            case 4:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_4;
                break;
            case 5:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_5;
                break;
            case 6:
                vojakSpawnPocetnost = GameManager.Instance.levelData.soldier_6;
                break;
            default:
                Debug.LogWarning("Unable to load soldier with id " + soldierId);
                vojakSpawnPocetnost = 1;
                break;
        }

        if(vojakSpawnPocetnost == 0)
        {
            GameObject.Destroy(lastVojakSpawned);
            GameObject.Destroy(gameObject);
        }

        pocetnostCounter.text = "" + vojakSpawnPocetnost;
    }

    /**
       * A normal member to detect Soldier is taken out of spawning area.
       * @param other - Collider.
       */
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "VojakStred" && other.transform.parent.gameObject.Equals(lastVojakSpawned))
        {
            if (vojakSpawnPocetnost > 1)
            {
                spawnVojak();
            }
            else
            {
                lastVojakSpawned = null;
            }
            decreaseCounter();
        }
    }

    /**
       * A normal member to be called when Soldier (vojak) is wrongly placed
       * @param vojak - GameObject.
       */
    public void onWrongPlacement(GameObject vojak)
    {
        if (vojakSpawnPocetnost == 0)
        {
            spawnVojak();
            increaseCounter();
        }
        // if vojak hasn't left spawning collider yet (no OnTriggerExit)
        else if (vojak.Equals(lastVojakSpawned))
        {
            spawnVojak();
        }
        else
        {
            increaseCounter();
        }
    }
    /**
       * A normal member to be called when number of remaining Soldiers (to be taken from spawning area) increases.
       * (e.g. Soldier was wrongly placed)
       */
    private void increaseCounter()
    {
        pocetnostCounter.text = "" + (++vojakSpawnPocetnost);
    }

    /**
       * A normal member to be called when number of remaining Soldiers (to be taken from spawning area) decreases.
       * (e.g. Soldier was taken from spawning area)
       */
    private void decreaseCounter()
    {
        pocetnostCounter.text = "" + (--vojakSpawnPocetnost);
    }

    /**
       * A normal member to spawn a new Soldier
       * (e.g. current soldier was taken from spawning area)
       */
    private void spawnVojak()
    {
        lastVojakSpawned = GameManager.InstantateScaled(vojakPrefab, transform.position, transform.rotation);
        lastVojakSpawned.transform.parent = transform.parent;
        lastVojakSpawned.GetComponent<PlacableObject>().spawning = this;
        lastVojakSpawned.GetComponent<PlayableObject>().weaponController = this.weaponController;
    }
}
