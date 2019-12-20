using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private List<Button> levelButtons;

    [SerializeField]
    private Text LevelNumber;

    void Start()
    {
        //PlayerPrefs.SetInt("finishedLvl" + 0, 1); // prvy lvl prejdeny
        CheckNextLevel();

    }

    private void CheckNextLevel()
    {
        LevelNumber.text = "" + (GetNextLevel() + 1); // index posunuty o 1
    }

    private int GetNextLevel()
    {
        int max = -1;

        for (var i = 0; i < levelButtons.Count-1; i++)
        {
            if (PlayerPrefs.GetInt("finishedLvl" + i, -1) != -1)
            {
                max = i;
            }
        }

        return max + 1; // ak som presiel 2. level mal by sa odomknut treti
    }

    public void LoadLevel()
    {
        LoadLevel(GetNextLevel());
    }
    public void LoadLevel(int level)
    {
        GameManager.Reset(level+1);

        SceneManager.LoadSceneAsync("MainScene");
    }

    public void CheckLevels()
    {
        levelButtons[0].interactable = true;

        for (var i = 1; i< levelButtons.Count; i++)
        {
            if (PlayerPrefs.GetInt("finishedLvl" + (i - 1), -1) != -1)
            {
                levelButtons[i].interactable = true;
            } else
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void UnlockAllLevels()
    {
        for (var i = 0; i < levelButtons.Count; i++)
        {
            PlayerPrefs.SetInt("finishedLvl" + i, 1);
        }
        CheckNextLevel();
    }

    public void RestoreProgress()
    {
        PlayerPrefs.DeleteAll();
        CheckNextLevel();
    }
}
