using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadPanelController : MonoBehaviour
{
    [SerializeField] private List<Button> saveSlotList;
    [SerializeField] private List<Button> loadSlotList;
    [SerializeField] private int maximumPage = 10;
    public static SaveLoadPanelController instance;
    private int savePageIndex = 0;
    private int loadPageIndex = 0;

    private void Start()
    {
        instance = this;
    }

    public void SaveInSlot(int slot)
    {

        if(savePageIndex != 0)
        {
            slot = slot + (saveSlotList.Count * savePageIndex);
        }
        SaveSystem.Save(slot);
        ShowDataPage("Save");
    }

    public void ShowDataPage(string pageType)
    {
        string filePath = Application.persistentDataPath + "/";
        int slotAmount = 0;
        int pageIndex = 0;
        List<Button> slotList = null;
        if (pageType.ToLower() != "save" && pageType.ToLower() != "load") return;
        if (pageType.ToLower() == "save")
        {
            slotAmount = saveSlotList.Count;
            pageIndex = savePageIndex;
            slotList = saveSlotList;
            ResetSlotPreview(saveSlotList);
        }
        if(pageType.ToLower() == "load")
        {
            slotAmount = loadSlotList.Count;
            pageIndex = loadPageIndex;
            slotList = loadSlotList;
            ResetSlotPreview(loadSlotList);
        }
        if (slotList == null) return;
        for(int i = 1; i <= saveSlotList.Count; i++)
        {
            int slotNumber = i + (slotAmount * pageIndex);
            string targetFilePath = filePath + "SaveData/CherLinSdata" + slotNumber + ".cher";
            if (File.Exists(targetFilePath))
            {
                Sprite placeHolder = SaveSystem.GetSavePicture(slotNumber);
                slotList[i-1].transform.Find("PreviewImage").GetComponent<Image>().sprite = placeHolder;
                slotList[i-1].transform.Find("Detail").GetComponent<TextMeshProUGUI>().text = placeHolder.name.ToString().Split("_")[2];
                slotList[i - 1].transform.Find("PreviewImage").gameObject.SetActive(true);

            }
        }
    }

    private void ResetSlotPreview(List<Button> slotList)
    {
        foreach(Button slot in slotList)
        {
            slot.transform.Find("PreviewImage").gameObject.SetActive(false);
            slot.transform.Find("Detail").GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    // int type = 0 for save page and int type 1 = for load page
    public void onClcikNextPage(int typeByInt)
    {
        if(typeByInt == 0 || typeByInt == 1)
        {
            switch (typeByInt)
            {
                case 0: 
                    if(savePageIndex < maximumPage-1)
                    {
                        savePageIndex += 1;
                        ShowDataPage("Save");
                    }
                    break;
                case 1:
                    if(loadPageIndex < maximumPage-1)
                    {
                        loadPageIndex += 1;
                        ShowDataPage("Load");
                    }
                    break;

            }
        }
        else
        {
            Debug.Log("Please enter the correct number. 0 for save page and 1 for load page");
        }
    }

    public void onClcikPrevPage(int typeByInt)
    {
        if (typeByInt == 0 || typeByInt == 1)
        {
            switch (typeByInt)
            {
                case 0:
                    if(savePageIndex > 0 )
                    {
                        savePageIndex -= 1;
                        ShowDataPage("Save");
                    }
                    break;
                case 1:
                    if (loadPageIndex > 0)
                    {
                        loadPageIndex -= 1;
                        ShowDataPage("Load");
                    }
                    break;

            }
        }
        else
        {
            Debug.Log("Please enter the correct number. 0 for save page and 1 for load page");
        }
    }
}
