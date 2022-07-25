using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        switch (sceneName.ToLower())
        {
            case "save":
                SettingMenuController.selectedPage = "Save";
                sceneName = "SettingMenu";
                break;

            case "load":
                SettingMenuController.selectedPage = "Load";
                sceneName = "SettingMenu";
                break;

            case "memo":
                SettingMenuController.selectedPage = "Memo";
                sceneName = "SettingMenu";
                break;

            case "option":
                SettingMenuController.selectedPage = "Option";
                sceneName = "SettingMenu";
                break;

            default:
                break;
        }
        SceneManager.LoadScene(sceneName);
    }
}
