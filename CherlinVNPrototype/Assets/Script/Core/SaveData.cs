using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

[System.Serializable]
public class SaveData
{
    public string playerName;
    public List<string> charSetAtSave;
    public List<string> choiceTextList;
    public Dictionary<string, string> charPosition;
    public Dictionary<string, string> charPicture;
    public string speaker;
    public string currentText;
    public int choiceAmount;
    public bool isChoice;
    public string location;
    public string storyState;
    public string song;


    public SaveData(StoryState state)
    {
        storyState = state.ToJson();
    }

    public void SaveCurrentChar(List<string> charSet)
    {
        charSetAtSave = charSet;
    }

    public void SaveCurrentText(string text)
    {
        currentText = text;
    }

    public void SaveCurrentSpeaker(string speakerName)
    {
        speaker = speakerName;
    }

    public void SaveCurrentLocation(string locationName)
    {
        location = locationName;
    }

    public void SaveCharactersPosition(Dictionary<string, string> charactersPositionDict)
    {
        charPosition = charactersPositionDict;
    }

    public void SaveCharactersPicture(Dictionary<string, string> charactersPicDict)
    {
        charPicture = charactersPicDict;
    }

    public void SaveChoice(List<string> choiceTextList)
    {
        isChoice = true;
        choiceAmount = choiceTextList.Count;
        this.choiceTextList = choiceTextList;
    }

    public void SaveSong(string songName)
    {
        song = songName;
    }

}
