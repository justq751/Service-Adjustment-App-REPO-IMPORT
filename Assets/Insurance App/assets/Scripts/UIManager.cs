using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if ( _instance == null)
            {
                Debug.LogError("UIManager Null");
            }
            return _instance;
        }
    }

    public Case activeCase;
    public ClientInfoPanel clientInfoPanel;
    public GameObject borderPanel;

    private void Awake()
    {
        _instance = this;
    }

    public void CreateNewCase()
    {
        activeCase = new Case();

        int randomCaseID = Random.Range(0, 1000);
        activeCase.caseID = "" + randomCaseID;

        clientInfoPanel.gameObject.SetActive(true);
        borderPanel.SetActive(true);
    }
}