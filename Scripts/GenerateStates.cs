using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateStates: MonoBehaviour
{
    

    [SerializeField] private int stateAmount; // like country
    private List<GameObject> cities;
    [SerializeField] private List<GameObject> mainStates;



    private void Start()
    {
        stateAmount = 3;
        cities = GameManager.Instance.cities;
        ChooseMainStates();
    }
    
    
    private void ChooseMainStates()
    {
        for (int i = 0; i < stateAmount; i++)
        {
            if (mainStates != null)
            {
               int randomNumber = Random.Range(0, cities.Count);
               
               if(!mainStates.Contains(cities[randomNumber]))
                  mainStates.Add(cities[randomNumber]);
               
            }
            else
            {
                int randomNumber = Random.Range(0, cities.Count);
                mainStates.Add(cities[randomNumber]);
            }
        }
    }

    private void CreateStates()
    {
        
    }

   /* List<T> ArrayToList(T[] array)
    {
        List<T> list = null;
        foreach (var item in array)
        {
            list.Add(item);
        }

        return list;
    }*/
}
