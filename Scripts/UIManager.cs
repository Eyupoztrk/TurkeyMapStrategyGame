using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [Header("Info Bar")] 
    [SerializeField] private GameObject barPanel;
    [SerializeField] private RawImage countryBar;
    List<GameObject> coutryBars = new List<GameObject>();


    private void SetInforBar(GameManager.Countries countries)
    {
        var countryCount = Enum.GetNames(typeof(GameManager.Countries)).Length; // ülkelerin sayısını bulma işlemi
        
        for (int i = 0; i < countryCount; i++)
        {
            barPanel.transform.GetChild(i).gameObject.SetActive(true); // bar'ları aktif etme işlemi
            coutryBars.Add(barPanel.transform.GetChild(i).gameObject);
        }
        
    }

    private void ApplicationsOnInfoBar()
    {
        for (int i = 0; i < coutryBars.Count; i++)
        {
            coutryBars[i].transform.localScale = new Vector3(2, 0, 0);
        }
    }
}
