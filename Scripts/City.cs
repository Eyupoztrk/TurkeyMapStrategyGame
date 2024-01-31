using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class City : MonoBehaviour
{
    [Header("--------- City Features ---------")]
    public int CityPower;
    public float IncreaseSpeed = 2;
    private int firstCityPower;
    
    [Header("--------- Game ---------")]
    [SerializeField] private City[] neighbors;
    [SerializeField] public GameManager.Countries Country;
    public GameManager.Countries firstCountry;
    public TextMeshProUGUI cityPowerText;
    [HideInInspector] public SpriteRenderer Renderer;




    private void Start()
    {
        firstCityPower = CityPower;
        firstCountry = Country;
        Renderer = GetComponent<SpriteRenderer>();
        cityPowerText = gameObject.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0)
            .GetComponent<TextMeshProUGUI>();

        cityPowerText.text = CityPower.ToString();

        StartCoroutine(IncreaseCityPower());
    }

    private IEnumerator IncreaseCityPower()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/IncreaseSpeed);
            Increase();
        }
       
        
    }

    private void Increase()
    {
        CityPower += 1;
        cityPowerText.text = CityPower.ToString();
    }

    public City[] GetNeighbors()
    {
        return neighbors;
    }

    public int GetFirstCityPower()
    {
        return firstCityPower;
    }

    public void SetCityPower(int cityPower)
    {
        this.CityPower = cityPower;
    }


}


    
