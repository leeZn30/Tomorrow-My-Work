using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpBtn : MonoBehaviour
{
    public void HelpInfoOpen()
    {
        if (!GameManager.Instance.isHelpShown)
        {
            GameManager.Instance.Help.SetActive(true);
            GameManager.Instance.isHelpShown = true;

        }
    }
    
}
