using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CgManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> cgPictureList;
    [SerializeField] private List<Button> cgSlot;
    [SerializeField] private int maximumPage;
    public static CgManager instance;
    private string hasSeenCG = "";
    private int pageIndex;

    private void Start()
    {
        instance = this;
        hasSeenCG = PlayerPrefs.GetString("CGList");
        pageIndex = 0;
    }

    public void ShowCgPreview()
    {
        int slotAmount = cgSlot.Count;
        List<int> hasSeenCGList = new List<int>();
        string[] cgArr = hasSeenCG.Split("_");
        ResetPreview();
        foreach(string hasSeenCG in cgArr)
        {
            hasSeenCGList.Add(int.Parse(hasSeenCG));
        }
        for(int i = 1; i <= slotAmount; i++)
        {
            int slotNumber = i + (slotAmount * pageIndex);
            if (hasSeenCGList.Contains(slotNumber))
            {
                cgSlot[i - 1].transform.Find("PreviewImage").GetComponent<Image>().sprite = cgPictureList[slotNumber - 1];
                cgSlot[i - 1].transform.Find("PreviewImage").gameObject.SetActive(true);
            }
        }
    }

    private void ResetPreview()
    {
        foreach (Button btn in cgSlot)
        {
            btn.transform.Find("PreviewImage").gameObject.SetActive(false);
        }
    }

    public void onClickNextPage()
    {
        if(pageIndex < maximumPage)
        {
            pageIndex += 1;
            ShowCgPreview();
        }
    }

    public void onClickPreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex -= 1;
            ShowCgPreview();
        }
    }
}
