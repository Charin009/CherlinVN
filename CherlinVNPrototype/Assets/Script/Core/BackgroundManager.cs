using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;
    [SerializeField] private List<Sprite> backgroundList = new List<Sprite>();
    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private List<Texture2D> effectList = new List<Texture2D>();
    [SerializeField] private Texture2D defaultEffect;
    public GameObject backgroundPanel;
    public GameObject overlayPanel;
    public RawImage effectPanel;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void SetBackground(string bgName)
    {
        backgroundPanel.GetComponent<Image>().sprite = FindImage(bgName);
    }

    public void ChangeBackground(string bgName, string effectName)
    {
        overlayPanel.SetActive(true);
        Sprite newBackground = FindImage(bgName);
        Texture2D effect = FindEffect(effectName);
        effectPanel.material.SetTexture("_AlphaTex", effect);
        StartCoroutine(TransitionTo(newBackground));
    }

    private Texture2D FindEffect(string effectName)
    {
        Texture2D targetEffect = null;
        if (effectName == "") return defaultEffect;
        foreach (Texture2D effect in effectList)
        {
            if (effect.name.Equals(effectName))
            {
                targetEffect = effect;
                break;
            }
        }

        return targetEffect;
    }


    public Sprite FindImage(string cgName)
    {
        Sprite targetImage = null;
        foreach (Sprite image in backgroundList)
        {
            if (image.name.Equals(cgName))
            {
                targetImage = image;
                break;
            }

        }
        return targetImage;
    }

    IEnumerator TransitionTo(Sprite newbackground)
    {
        float maxVal = 1;
        float minVal = 0;
        float curVal = effectPanel.material.GetFloat("_Cutoff");

        while (curVal > minVal)
        {
            curVal = Mathf.MoveTowards(curVal, minVal, Time.deltaTime * transitionSpeed);
            effectPanel.material.SetFloat("_Cutoff", curVal);
            yield return null;
        }
        backgroundPanel.GetComponent<Image>().sprite = newbackground;
        while (curVal < maxVal)
        {
            curVal = Mathf.MoveTowards(curVal, maxVal, Time.deltaTime * transitionSpeed);
            effectPanel.material.SetFloat("_Cutoff", curVal);
            yield return null;
        }
        overlayPanel.SetActive(false);
        yield break;
    }
}
