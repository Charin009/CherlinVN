using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    [SerializeField] private List<Character> characterPrefabList = new List<Character>();
    [SerializeField] private RectTransform characterPanel;
    [SerializeField] private float moveSpeed;
    private Dictionary<string, Character> charactersPrefabDict = new Dictionary<string, Character>();
    public Dictionary<string, Character> charactersDict = new Dictionary<string, Character>();

    public Transform leftPosObjt;
    public Transform leftOutPosObjt;
    public Transform rightPosObjt;
    public Transform rightOutPosObjt;
    public Transform centerPosObjt;
    public Transform moveOutLeftPosObjt;
    public Transform moveOutRightPosObjt;

    [HideInInspector]
    public Vector3 leftPos, leftOutPos, rightPos, rightOutPos, centerPos, moveOutLeftPos, moveOutRightPos;

    public static CharactersManager instance;

    private void Awake()
    {
        instance = this;
        foreach(Character character in characterPrefabList)
        {
            charactersPrefabDict.Add(character.GetCharacterName(), character);
        }

        leftPos = leftPosObjt.position;
        leftOutPos = leftOutPosObjt.position;
        rightPos = rightPosObjt.position;
        rightOutPos = rightOutPosObjt.position;
        centerPos = centerPosObjt.position;
        moveOutLeftPos = moveOutLeftPosObjt.position;
        moveOutRightPos = moveOutRightPosObjt.position;

}

    public Character GetCharacter(string name)
    {
        Character targetCharacter;
        if(charactersDict.TryGetValue(name, out targetCharacter))
        {
            return targetCharacter;
        }
        else
        {
            return targetCharacter;
        }
    }

    public void PrepareCharacter(List<string> characterSet)
    {
        foreach(string characterName in characterSet)
        {
            Character preparedCharacter = Instantiate(charactersPrefabDict[characterName], characterPanel);
            preparedCharacter.gameObject.SetActive(false);
            charactersDict.Add(preparedCharacter.GetCharacterName(), preparedCharacter);

        }
    }
    public void ShowCharacter(string name)
    {
        if(name != "Narrator")
        {
            GetCharacter(name).gameObject.SetActive(true);
        }
        
    }

    public void SetCharacterImage(string name, string action, string emotion)
    {
        GetCharacter(name).SetImage(action, emotion);
    }

    public void ChangeCharacterEmotion(string name, string emotion)
    {
        GetCharacter(name).ChangeEmotion(emotion);
    }

    public void MoveCharacter(string name, Vector3 position)
    {
        StartCoroutine(Moving(name, position));
    }

    public void SpawnCharacterAt(string name, Vector3 position)
    {
        GetCharacter(name).gameObject.transform.position = position;
    }

    public List<string> GetListOfCurrentCharacter()
    {
        List<string> currentList = new List<string>();

        foreach (KeyValuePair<string, Character> character in charactersDict)
        {
            currentList.Add(character.Key);
        }
        return currentList;
    }

    public void ClearCharacter()
    {
        foreach(KeyValuePair<string,Character> character in charactersDict)
        {
            Destroy(character.Value.gameObject);
        }
        charactersDict.Clear();
    }

    IEnumerator Moving(string name, Vector3 position)
    {
        while(GetCharacter(name).gameObject.transform.position != position)
        {
            GetCharacter(name).gameObject.transform.position = Vector3.MoveTowards(GetCharacter(name).gameObject.transform.position, position, moveSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
