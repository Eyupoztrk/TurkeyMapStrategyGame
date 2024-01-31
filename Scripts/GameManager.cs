using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

   private void Awake()
   {
      // If there is an instance, and it's not me, delete myself.

      if (Instance != null && Instance != this)
      {
         Destroy(this);
      }
      else
      {
         Instance = this;
      }

   }

   public List<GameObject> cities;
   public List<GameObject> canVisitedCircels;
   public City selectedCity;
   public City goalCity;
   public bool firtTouch = true;

   public enum Countries
   { country1, country2, country3 , country4, country5, country6};

   public List<GameObject> Enemies;

   [HideInInspector] public Countries countries;
   [SerializeField] private AttackApplication attackApplication;

   public Countries playerCountry;
   
   /*********** Delegetes and Event Part ***************/
   public delegate void Disappear();
   public event Disappear OnDisappear;

   public delegate void GetCity(bool isOtherCity);
   public event GetCity OnGetOtherCity;
   public event GetCity OnGetOurCity;
   
   public delegate void Win();
   public event Win OnWin;

   private void Update()
   {
      if (OnDisappear != null)
      {
         OnDisappear();
         OnDisappear = null;
      }
      
      if (OnWin != null)
      {
         OnWin();
         OnWin = null;
      }

      if (Input.GetKeyDown(KeyCode.Space))
      {
         ResetGame();
      }
   }

   public void Attack()
   {
      
      if (selectedCity.Country.Equals(playerCountry))
      {
         if (selectedCity.CityPower <= goalCity.CityPower) // şehri almaya gücümüz yetmiyorsa
         {
            if (!selectedCity.Country.Equals(goalCity.Country)) // farklı bi şehre asker yollama
            {
               goalCity.CityPower = goalCity.CityPower - selectedCity.CityPower;
               selectedCity.CityPower = 0;
            }
            else // kendi şehrine asker yollama
            {
               goalCity.CityPower = goalCity.CityPower + selectedCity.CityPower;
               selectedCity.CityPower = 0;
            }
            SetCityText(selectedCity,selectedCity.CityPower);
            SetCityText(goalCity,goalCity.CityPower);
         
        
         }
      
         else if (selectedCity.CityPower > goalCity.CityPower) // şehri almaya gücümüz yetiyorsa
         {
            if (!selectedCity.Country.Equals(goalCity.Country))
            {
               var difference = selectedCity.CityPower - goalCity.CityPower;
               this.selectedCity.CityPower = 0;
               this.goalCity.CityPower = difference;

               goalCity.Renderer.color = selectedCity.Renderer.color;

               goalCity.Country = selectedCity.Country;
            }
            else
            {
               goalCity.CityPower = goalCity.CityPower + selectedCity.CityPower;
               selectedCity.CityPower = 0;
            }
            SetCityText(selectedCity,selectedCity.CityPower);
            SetCityText(goalCity,goalCity.CityPower);
         
         }
         else
         {
            print("nothing");
         }
      }

   
   }
   public void Attack(City selectedCity, City goalCity)
   {

      if (!selectedCity.Country.Equals(playerCountry))
      {
         if (selectedCity.CityPower <= goalCity.CityPower) // şehri almaya gücümüz yetmiyorsa
         {
            if (!selectedCity.Country.Equals(goalCity.Country)) // farklı bi şehre asker yollama
            {
               goalCity.CityPower = goalCity.CityPower - selectedCity.CityPower;
               selectedCity.CityPower = 0;
            }
            else // kendi şehrine asker yollama
            {
               goalCity.CityPower = goalCity.CityPower + selectedCity.CityPower;
               selectedCity.CityPower = 0;
            }

           // OnGetOurCity(false);
            SetCityText(selectedCity, selectedCity.CityPower);
            SetCityText(goalCity, goalCity.CityPower);


         }

         else if (selectedCity.CityPower > goalCity.CityPower) // şehri almaya gücümüz yetiyorsa
         {
            if (!selectedCity.Country.Equals(goalCity.Country))
            {
               var difference = selectedCity.CityPower - goalCity.CityPower;
               selectedCity.CityPower = 0;
               goalCity.CityPower = difference;

               goalCity.Renderer.color = selectedCity.Renderer.color;
               goalCity.Country = selectedCity.Country;
            }
            else
            {
               goalCity.CityPower = goalCity.CityPower + selectedCity.CityPower;
               selectedCity.CityPower = 0;
            }

           // OnGetOtherCity(true);

            SetCityText(selectedCity, selectedCity.CityPower);
            SetCityText(goalCity, goalCity.CityPower);

         }
         else
         {
            print("nothing");
         }

      }

   }

   private void SetCityText(City city, int cityPower)
   {
      city.cityPowerText.text = cityPower.ToString();
   }

   public void CopyAttackObject(GameObject selectedCity, GameObject goalCity)
   {
      attackApplication.CopyAtackObject(selectedCity,goalCity,10);
   }
   
   public int GetNumberOfCities(Countries country)
   {
      var cityAmount = 0;
      
      foreach (var item in cities)
      {
         if (item.GetComponent<City>().Country.Equals(country))
         {
            cityAmount++;
         }
      }

      return cityAmount;
   }

   public void ResetGame()
   {
      foreach (var item in Enemies)
      {
         item.gameObject.SetActive(true);
         var Item = item.GetComponent<EnemyAtack>();

         foreach (var city in Item.firstCities)
         {
            city.CityPower = city.GetFirstCityPower();
            city.gameObject.GetComponent<SpriteRenderer>().color = Item.countryColor;
            city.Country = Item.COUNTRY;

         }
      }

      foreach (var city in cities )
      {
         if (city.GetComponent<City>().firstCountry.Equals(playerCountry))
         {
            city.GetComponent<City>().CityPower = city.GetComponent<City>().GetFirstCityPower();
            city.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            city.GetComponent<City>().Country = playerCountry;
         }
      }
      
      
      
   }
   


}
