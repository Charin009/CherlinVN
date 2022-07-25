using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] private string characterName;
    [SerializeField] private List<Sprite> imageList = new List<Sprite>();
    [SerializeField] private GameObject characterImagePanelBase, characterImagePanelChange;
    private float fadingWaitTime = 0.0025f;
    public string characterAction = "Idle";
    public string characterEmotion = "Default";

    public string GetCharacterName()
    {
        return this.characterName;
    }
    public void SetImage(string action, string emotion)
    {
        Sprite targetImage = GetImage(action, emotion);
        if (targetImage != null)
        {
            characterImagePanelBase.GetComponent<Image>().sprite = targetImage;
            characterAction = action;
            characterEmotion = emotion;
        }
    }

    public void ChangeEmotion(string emotion)
    {
        Sprite targetImage = GetImage(characterAction, emotion);
        if (targetImage != null)
        {
            characterImagePanelChange.GetComponent<Image>().sprite = targetImage;
            StartCoroutine(Fading(targetImage));
            characterEmotion = emotion;
        }

    }

    IEnumerator Fading(Sprite target)
    {
        Color initialColor = characterImagePanelBase.GetComponent<Image>().color;
        Color alpha = new Color(255, 255, 255, 0);
        for(float f = 0f; f <= 1f; f+= 0.01f)
        {
            initialColor.a = f;
            characterImagePanelChange.GetComponent<Image>().color = initialColor;
            yield return new WaitForSeconds(fadingWaitTime);
        }
        characterImagePanelBase.GetComponent<Image>().sprite = target;
        characterImagePanelChange.GetComponent<Image>().color = alpha;


    }


    private Sprite GetImage(string action, string emotion)
    {
        Sprite targetImage = null;
        if (!characterAction.Equals(action) || !characterEmotion.Equals(emotion))
        {
            if (action == "") action = characterAction;
            if (emotion == "") emotion = characterEmotion;
            foreach (Sprite image in imageList)
        {
            if (image.name.Contains(action) && image.name.Contains(emotion))
            {
                targetImage = image;
                break;
            }
        }
        }
        return targetImage;
    }
}
