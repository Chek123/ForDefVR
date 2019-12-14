using System.Collections;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int number_of_levels;
    public Level[] levels;
}

[System.Serializable]
public class Level
{
    public Grid[] grids;
}

[System.Serializable]
public class Grid
{
    public Square[] square;
}

[System.Serializable]
public class Square
{
    public float x;
    public float z;
    public int vojak;
}

[System.Serializable]
public class EnemyDataController : MonoBehaviour
{
    [SerializeField]
    private Transform enemyPlayground;

    public void LoadData()
    {
        string path = "Assets/Database/data.json";
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        Data data = JsonUtility.FromJson<Data>(json);

        // treba mat premennu current_level, potom nacitanie:
        //Debug.Log(data.levels[current_level - 1])
        Level level = data.levels[0];
        int grid_id = Random.Range(0, level.grids.Length);
        Square[] square = level.grids[grid_id].square;

        foreach (var obj in square)
        {
            if (obj.vojak != 0)
            {
                var prefab = Resources.Load("VojakModel" + obj.vojak, typeof(GameObject));
                GameObject soldier = Instantiate(prefab, enemyPlayground) as GameObject;
                soldier.transform.localPosition = new Vector3(obj.x, 0.05f, obj.z);
                soldier.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                soldier.transform.localScale *= 5;
                soldier.GetComponent<PlacableObject>().setIsScaled(true);
            }
        }
    }
}