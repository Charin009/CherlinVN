using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    public static TransitionController instance;

    [SerializeField] private float transitionSpeed= 2f;
    [SerializeField] private List<Texture2D> effectList = new List<Texture2D>();
    [SerializeField] private Texture2D defaultEffect;
    public GameObject backgroundPanel;
    public RawImage overlayPanel;
    BackgroundManager backgroundManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        backgroundManager = BackgroundManager.instance;
    }

    public void ChangeBackground(string name, string effectName)
    {
        Sprite newBackground = backgroundManager.FindImage(name);
        Texture2D effect = FindEffect(effectName);
        overlayPanel.material.SetTexture("_AlphaTex", effect);
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

    IEnumerator TransitionTo(Sprite newbackground)
    {
        float maxVal = 1;
        float minVal = 0;
        float curVal = overlayPanel.material.GetFloat("_Cutoff");

        while(curVal > minVal)
        {
            curVal = Mathf.MoveTowards(curVal, minVal, Time.deltaTime * transitionSpeed);
            overlayPanel.material.SetFloat("_Cutoff", curVal);
            yield return null;
        }
        backgroundPanel.GetComponent<Image>().sprite = newbackground;
        while (curVal < maxVal)
        {
            curVal = Mathf.MoveTowards(curVal, maxVal, Time.deltaTime * transitionSpeed);
            overlayPanel.material.SetFloat("_Cutoff", curVal);
            yield return null;
        }
        yield break;
    }
}
