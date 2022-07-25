using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingMenuController: MonoBehaviour
{
    AudioManager audioManager;
    DialogueSystem dialogueSystem;
    public AudioMixer mixer;
    public static string selectedPage;
    private string sampleText;
    private float textSpeed = 0.025f;
    private bool isWriting = false;
    [SerializeField] private TextMeshProUGUI sampleTMP;
    [SerializeField] private List<GameObject> allPage;
    [SerializeField] private List<TabButton> tabButton;
    [SerializeField] private GameObject screenModeToggle;
    [SerializeField] private GameObject textSpeedToggle;
    [SerializeField] private GameObject masterVolToggle;
    [SerializeField] private GameObject songVolToggle;
    [SerializeField] private GameObject sfxVolToggle;


    private void Start()
    {
        audioManager = AudioManager.instance;
        dialogueSystem = DialogueSystem.instance;
        textSpeed = PlayerPrefs.GetFloat("TextSpeed");
        sampleText = "Sample Text Speed...";
        switch (selectedPage.ToLower())
        {
            case "save":
                SetActivePage(0);
                break;

            case "load":
                SetActivePage(1);
                break;

            case "memo":
                SetActivePage(2);
                break;

            case "option":
                SetActivePage(3);
                break;
        }

        SetUpScreenToggle();
        SetUpTextSpeedToggle();
        SetUpVolumeToggle("song");
        SetUpVolumeToggle("sfx");
        SetUpVolumeToggle("master");

    }

    private void Update()
    {
        if (isWriting == false)
        {
            isWriting = true;
            StartCoroutine(WriteSampleText(sampleText));
        }
    }

    IEnumerator WriteSampleText(string speech)
    {
        sampleTMP.text = "";
        while (sampleTMP.text != speech)
        {
            sampleTMP.text += speech[sampleTMP.text.Length];
            yield return new WaitForSeconds(textSpeed);
        }
        isWriting = false;
        yield break;
    }

    public void SetActivePage(int index)
    {
        string pageType = "";
        for (int i = 0; i < allPage.Count; i++)
        {
            allPage[i].SetActive(i == index);
            if (i != index)
            {
                tabButton[i].ResetPosition();
            }
        }
        switch (index)
        {
            case 0:
                pageType = "Save";
                break;

            case 1:
                pageType = "Load";
                break;
            case 2:
                CgManager.instance.ShowCgPreview();
                break;
        }
        if(pageType != "") SaveLoadPanelController.instance.ShowDataPage(pageType);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("FullScreen",1);
    }

    public void SetTextSpeed(float textSpeed)
    {
        this.textSpeed = textSpeed;
        DialogueSystem.textSpeed = textSpeed;
        PlayerPrefs.SetFloat("TextSpeed", textSpeed);
    }

    public void MasterVolume(float volume)
    {
        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SongVolume(float volume)
    {
        audioManager.playerSong.volume =  volume;
        audioManager.maxSongVolume = volume;
        PlayerPrefs.SetFloat("SongVolume", volume);
    }

    public void SFXVolume(float volume)
    {
        audioManager.playerSFX.volume = volume;
        audioManager.maxSFXVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void SetUpScreenToggle()
    {
        if (PlayerPrefs.GetInt("FullScreen") == 1)
        {
            screenModeToggle.transform.Find("FullScreen").GetComponent<Toggle>().isOn = true;
        }
        else
        {
            screenModeToggle.transform.Find("Windowed").GetComponent<Toggle>().isOn = true;
        }
    }

    private void SetUpTextSpeedToggle()
    {
        string textSpeedMode = "";
        switch (PlayerPrefs.GetFloat("TextSpeed"))
        {
            case 0.025f:
                textSpeedMode = "Fast";
                break;

            case 0.05f:
                textSpeedMode = "Medium";
                break;
            case 0.15f:
                textSpeedMode = "Slow";
                break;
        }
        textSpeedToggle.transform.Find(textSpeedMode).GetComponent<Toggle>().isOn = true;
    } 

    private void SetUpVolumeToggle(string type)
    {
        int targetToggle;
        string toggleName;
        switch (type.ToLower())
        {
            case "song":
                targetToggle = (int)(PlayerPrefs.GetFloat("SongVolume") * 10);
                if (targetToggle == 10) { toggleName = "10"; }
                else { toggleName = "0" + targetToggle.ToString(); }
                Debug.Log(toggleName);
                songVolToggle.transform.Find(toggleName).GetComponent<Toggle>().isOn = true;
                break;

            case "sfx":
                targetToggle = (int)(PlayerPrefs.GetFloat("SFXVolume") * 10);
                if (targetToggle == 10) { toggleName = "10"; }
                else { toggleName = "0" + targetToggle.ToString(); }
                sfxVolToggle.transform.Find(toggleName).GetComponent<Toggle>().isOn = true;
                break;

            case "master":
                targetToggle = (int)(PlayerPrefs.GetFloat("MasterVolume") * 10);
                if (targetToggle == 10) { toggleName = "10"; }
                else { toggleName = "0" + targetToggle.ToString(); }
                masterVolToggle.transform.Find(toggleName).GetComponent<Toggle>().isOn = true;
                break;
        }
    }
}
