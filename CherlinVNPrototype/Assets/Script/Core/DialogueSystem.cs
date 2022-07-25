using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] TextAsset mainStory;
    public List<GameObject> choiceList;
    public Story story;
    public static float textSpeed;
    public bool isChoosingChoice;
    public int choiceCount;
    private List<string> tagList;

    CharactersManager charManager;
    BackgroundManager bgManager;
    AudioManager audioManager;

    public static DialogueSystem instance;
    public ELEMENTS elements;

    public bool isWaitForUserInput = false;
    public GameObject speechPanel { get { return elements.speechPanel; } }
    public GameObject nameBox { get { return elements.nameBox; } }
    public TextMeshProUGUI speakerNameText { get { return elements.speakerNameText; } }
    public TextMeshProUGUI speechText { get { return elements.speechText; } }



    [System.Serializable]
    public class ELEMENTS
    {
        public GameObject speechPanel;
        public GameObject nameBox;
        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI speechText;
    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        story = new Story(mainStory.text);
        charManager = CharactersManager.instance;
        bgManager = BackgroundManager.instance;
        audioManager = AudioManager.instance;
        textSpeed = PlayerPrefs.GetFloat("TextSpeed");
    }

    public void AdvanceStory()
    {
        string currentSpeech = story.Continue();
        choiceCount = 0;
        isChoosingChoice = false;
        SettingByTags();
        StopAllCoroutines();
        DisplayChoice();
        StartCoroutine(Speaking(currentSpeech));

    }


    IEnumerator Speaking(string speech)
    {
        speechPanel.SetActive(true);
        speechText.text = "";
        isWaitForUserInput = false;

        while (speechText.text != speech)
        {
            speechText.text += speech[speechText.text.Length];
            yield return new WaitForSeconds(textSpeed);
        }
        isWaitForUserInput = true;
        while (isWaitForUserInput)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void SettingByTags()
    {
        tagList = story.currentTags;
        bool isSetPicture = false;
        bool isPictureChange = false;
        bool isBGChange = false;
        bool isSongChange = false;
        string tmpName = "";
        string tmpAction = "";
        string tmpEmotion = "";
        string spawnPos = "";
        string movePos = "";
        string background = "";
        string backgroundEffect = "";
        string tmpSongName = "";
        string tmpSFXName = "";
        List<string> charSet = new List<string>();

        foreach (string tag in tagList)
        {
            string key = tag.Split(':')[0];
            string value = tag.Split(':')[1];

            switch (key.ToLower())
            {
                case "name":
                    tmpName = value;
                    SetSpeakerName(value);
                    charManager.ShowCharacter(value);
                    break;

                case "add_char":
                    tmpName = value;
                    charManager.ShowCharacter(value);
                    break;

                case "set_action":
                    isSetPicture = true;
                    tmpAction = value;
                    break;

                case "set_emotion":
                    isSetPicture = true;
                    tmpEmotion = value;
                    break;

                case "change_emotion":
                    isPictureChange = true;
                    tmpEmotion = value;
                    break;

                case "show_at":
                    spawnPos = value;
                    break;

                case "move_to":
                    movePos = value;
                    break;

                case "change_bg":
                    background = value;
                    isBGChange = true;
                    break;

                case "effect":
                    backgroundEffect = value;
                    break;

                case "song":
                    tmpSongName = value;
                    break;

                case "change_song":
                    tmpSongName = value;
                    isSongChange = true;
                    break;

                case "sfx":
                    tmpSFXName = value;
                    break;

                case "prepare":
                    if (value == "None")
                    {
                        charManager.ClearCharacter();
                    }
                    else
                    {
                        charManager.ClearCharacter();
                        string[] allCharacter = value.Split('&');
                        for (int i = 0; i < allCharacter.Length; i++)
                        {
                            charSet.Add(allCharacter[i]);
                        }
                        charManager.PrepareCharacter(charSet);
                    }
                    break;

            }

            if (isBGChange)
            {
                bgManager.ChangeBackground(background, backgroundEffect);
                isBGChange = false;
            }
            if (tmpSongName != "")
            {
                if (isSongChange)
                {
                    audioManager.ChangeSong(tmpSongName);
                    isSongChange = false;
                    tmpSongName = "";
                }
                else
                {
                    audioManager.PlaySong(tmpSongName);
                    tmpSongName = "";
                }

            }
            if (tmpSFXName != "")
            {
                audioManager.PlaySFX(tmpSFXName);
            }
            if (isSetPicture)
            {
                charManager.SetCharacterImage(tmpName, tmpAction, tmpEmotion);
                isSetPicture = false;
            }
            if (isPictureChange)
            {
                charManager.ChangeCharacterEmotion(tmpName, tmpEmotion);
                isPictureChange = false;
            }
            if (spawnPos != "")
            {
                charManager.SpawnCharacterAt(tmpName, GetPositionFromCharacterManagaer(spawnPos));
                spawnPos = "";
            }
            if (movePos != "")
            {
                charManager.MoveCharacter(tmpName, GetPositionFromCharacterManagaer(movePos));
                movePos = "";
            }

        }

    }

    public bool CanContinue()
    {
        return story.canContinue;
    }

    public void SetSpeakerName(string name)
    {
        if (name.Equals("Narrator"))
        {
            nameBox.gameObject.SetActive(false);
        }
        else
        {
            nameBox.gameObject.SetActive(true);
            speakerNameText.text = charManager.GetCharacter(name).GetCharacterName();
        }
    }

    public void DisplayChoice()
    {
        List<Choice> currentChoice = story.currentChoices;
        if (currentChoice.Count > choiceList.Count)
        {
            Debug.Log("Warning: More choice than the UI can support");
        }
        isChoosingChoice = true;
        choiceCount = currentChoice.Count;
        int index = 0;
        foreach (Choice choice in currentChoice)
        {
            choiceList[index].GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            choiceList[index].SetActive(true);
            index++;
        }
    }

    public void ClearChoice()
    {
        foreach (GameObject choice in choiceList)
        {
            choice.SetActive(false);
        }
    }

    public void ChooseChoice(int choiceIndex)
    {
        ClearChoice();
        story.ChooseChoiceIndex(choiceIndex);
        AdvanceStory();
    }

    public void LoadStory(string saveData)
    {
        story.state.LoadJson(saveData);
    }

    private Vector3 GetPositionFromCharacterManagaer(string position)
    {
        switch (position.ToLower())
        {
            case "left":
                return charManager.leftPos;

            case "left_out":
                return charManager.leftOutPos;

            case "center":
                return charManager.centerPos;

            case "right":
                return charManager.rightPos;

            case "right_out":
                return charManager.rightOutPos;

            default:
                return new Vector3(0, 0, 0);
        }
    }
}
