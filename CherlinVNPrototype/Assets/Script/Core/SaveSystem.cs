using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSystem : MonoBehaviour
{
    private static void Serialize(SaveData data, int saveSlot, string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();

    }

    private static SaveData Deserialize(int loadSlot, string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);
        SaveData data = null;


        if (File.Exists(path))
        {
            data = formatter.Deserialize(stream) as SaveData;
        }
        else
        {
            Debug.Log("SaveFile not found");
        }
        return data;
    }
    public static void Save(int saveSlot)
    {

        SaveData data = new SaveData(DialogueSystem.instance.story.state);
        Dictionary<string, string> charactersPicDict = new Dictionary<string, string>();
        Dictionary<string, string> charactersPositionDict = new Dictionary<string, string>();

        string saveDataDir = Application.persistentDataPath + "/SaveData";
        string previewPicDir = Application.persistentDataPath + "/Preview";
        string fileName = "/CherLinSdata" + saveSlot + ".cher";

        if (!Directory.Exists(saveDataDir)) Directory.CreateDirectory(saveDataDir);
        if (!Directory.Exists(previewPicDir)) Directory.CreateDirectory(previewPicDir);

        ScreenCapture.CaptureScreenshot(previewPicDir + "/SaveScreenSlot_" + saveSlot + "_" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".png");
        data.SaveCurrentChar(CharactersManager.instance.GetListOfCurrentCharacter());
       
        if (DialogueSystem.instance.nameBox.activeInHierarchy)
        {

            data.SaveCurrentSpeaker(DialogueSystem.instance.speakerNameText.text);
        }
        else
        {
            data.SaveCurrentSpeaker("Narrator");
        }
        data.SaveCurrentText(DialogueSystem.instance.speechText.text);
        data.SaveCurrentLocation(BackgroundManager.instance.backgroundPanel.GetComponent<Image>().sprite.name);

        if (DialogueSystem.instance.isChoosingChoice)
        {
            List<string> choiceList = new List<string>();
            foreach(GameObject choice in DialogueSystem.instance.choiceList)
            {
                if (choice.activeInHierarchy)
                {
                    choiceList.Add(choice.GetComponentInChildren<TextMeshProUGUI>().text);
                }
            }
            data.SaveChoice(choiceList);
        }

        if (AudioManager.instance.playerSong.isPlaying)
        {
            data.SaveSong(AudioManager.instance.playerSong.clip.name);
        }

        foreach (GameObject character in GameObject.FindGameObjectsWithTag("Character"))
        {
            string characterName = character.GetComponent<Character>().GetCharacterName();
            string characterPicture = character.transform.Find("CharacterImage1").GetComponent<Image>().sprite.name;
            string characterPosition = character.transform.position.x + "_" + character.transform.position.y + "_" + character.transform.position.z;

            charactersPicDict.Add(characterName, characterPicture);
            charactersPositionDict.Add(characterName, characterPosition);

        }
        data.SaveCharactersPicture(charactersPicDict);
        data.SaveCharactersPosition(charactersPositionDict);
        Serialize(data, saveSlot, saveDataDir+fileName);
    }

    public void Load(int loadSlot)
    {
        string filePath = Application.persistentDataPath + "/SaveData/CherLinSdata" + loadSlot + ".cher";
        SaveData loadedData = Deserialize(loadSlot,filePath);
        List<string> charSet = loadedData.charSetAtSave;

        Dictionary<string, string> charactersPicDict = loadedData.charPicture;
        Dictionary<string, string> charactersPositionDict = loadedData.charPosition;

        CharactersManager.instance.ClearCharacter();
        CharactersManager.instance.PrepareCharacter(charSet);

        if (loadedData.isChoice)
        {
            foreach(GameObject choice in DialogueSystem.instance.choiceList)
            {
                choice.SetActive(false);
            }
            List<string> choiceListFromLoadedData = loadedData.choiceTextList;
            for(int i = 0; i < loadedData.choiceAmount; i++)
            {
                DialogueSystem.instance.choiceList[i].GetComponentInChildren<TextMeshProUGUI>().text = choiceListFromLoadedData[i];
                DialogueSystem.instance.choiceList[i].SetActive(true);
            }
        }

        if(loadedData.song != "")
        {
            AudioManager.instance.PlaySong(loadedData.song);
        }

        string speakerName = loadedData.speaker;
        string currentText = loadedData.currentText;
        string location = loadedData.location;

        BackgroundManager.instance.SetBackground(location);
        DialogueSystem.instance.speechText.text = currentText;
        if (speakerName != "Narrator")
        {
            DialogueSystem.instance.speakerNameText.text = speakerName;
            DialogueSystem.instance.nameBox.SetActive(true);
        }
        else
        {
            DialogueSystem.instance.nameBox.SetActive(false);
        }

        if(charactersPicDict.Count > 0)
        {
            foreach (string name in charSet)
            {
                string positionString;
                string pictureName;
                charactersPositionDict.TryGetValue(name, out positionString);
                charactersPicDict.TryGetValue(name, out pictureName);

                string action = pictureName.Split('_')[1];
                string emotion = pictureName.Split('_')[2];
                float posX = float.Parse(positionString.Split('_')[0]);
                float posY = float.Parse(positionString.Split('_')[1]);
                float posZ = float.Parse(positionString.Split('_')[2]);

                Vector3 position = new Vector3(posX, posY, posZ);

                CharactersManager.instance.ShowCharacter(name);
                CharactersManager.instance.SetCharacterImage(name, action, emotion);
                CharactersManager.instance.SpawnCharacterAt(name, position);
            }
        }
        DialogueSystem.instance.LoadStory(loadedData.storyState);
    }

    public static Sprite GetSavePicture(int saveSlot)
    {
        Sprite targetSprite = null;
        foreach (string filename in Directory.GetFiles(Application.persistentDataPath + "/Preview/"))
        {
            if (int.Parse(filename.Split("_")[1]) == saveSlot)
            {
                byte[] bytes = File.ReadAllBytes(filename);
                Texture2D texture = new Texture2D(2,2);
                texture.LoadImage(bytes);
                targetSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0,0),  100.0f, 0, SpriteMeshType.Tight);
                targetSprite.name = Path.GetFileNameWithoutExtension(filename);
            }
        }
        return targetSprite;
    }
}
