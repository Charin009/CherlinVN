using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    DialogueSystem dialogue;
    AudioManager audioManager;
    TransitionController transitionController;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = DialogueSystem.instance;
        audioManager = AudioManager.instance;
        transitionController = TransitionController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogue.CanContinue()) {
                dialogue.AdvanceStory();
            }
        }
    }

}
