using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    private float transitionPos = 55f;
    private Vector3 originPos;
    // Start is called before the first frame update

    private void Start()
    {
        originPos = this.transform.position;
    }
    public void OnTabButton()
    {
        this.transform.position = new Vector3(this.transform.position.x, transform.position.y + transitionPos, this.transform.position.z);
        this.transform.GetComponent<Button>().interactable = false;
    }

    public void ResetPosition()
    {
        this.transform.position = this.originPos;
        this.transform.GetComponent<Button>().interactable = true;
    }

}
