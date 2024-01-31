using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Circle : MonoBehaviour
{
   private GameObject arrow;
   private bool isDown = false;
   private bool isUp = false;
   private City selectedCity;
   private bool isSelected;
   
   private GameManager.Countries Country;



   private Vector3 GetMousePosition()
   {
       Vector3 mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
       return mousePos;
   }


   private void OnMouseDown()
   {
      
       if (GameManager.Instance.firtTouch)
       {

           if (GameManager.Instance.selectedCity == null)
           {
               GameManager.Instance.selectedCity = this.transform.parent.gameObject.GetComponent<City>(); // basılan cirle'ın parent objesi bizim seçilen şehrimiz
               GameManager.Instance.selectedCity.gameObject.tag = "SC";
               
               if (!GameManager.Instance.selectedCity.Country.Equals(GameManager.Instance.playerCountry))
               {
                   GameManager.Instance.selectedCity.gameObject.tag = "Bolgeler";
                   GameManager.Instance.selectedCity = null;
               }
               else
               {
                   GameManager.Instance.selectedCity = this.transform.parent.gameObject.GetComponent<City>(); // basılan cirle'ın parent objesi bizim seçilen şehrimiz
                   GameManager.Instance.selectedCity.gameObject.tag = "SC";
       
                   List<GameObject> canVisitedCircles = CreateList(GameManager.Instance.canVisitedCircels);
       
                   canVisitedCircles.Remove(this.gameObject);
                   foreach (var item in canVisitedCircles)
                   {
                       item.transform.GetChild(0).gameObject.SetActive(true);
                   }
                  
               }

           }
           else
           {
               GameManager.Instance.selectedCity = this.transform.parent.gameObject.GetComponent<City>(); // basılan cirle'ın parent objesi bizim seçilen şehrimiz
               GameManager.Instance.selectedCity.gameObject.tag = "SC";
               
               if (!GameManager.Instance.selectedCity.Country.Equals(GameManager.Instance.playerCountry))
               {
                   GameManager.Instance.selectedCity.gameObject.tag = "Bolgeler";
                   GameManager.Instance.selectedCity = null;
               }
               else
               {
                   ResetCities();
                   GameManager.Instance.selectedCity =
                       this.transform.parent.gameObject
                           .GetComponent<City>(); // basılan cirle'ın parent objesi bizim seçilen şehrimiz
                   GameManager.Instance.selectedCity.gameObject.tag = "SC";

                   List<GameObject> canVisitedCircles = CreateList(GameManager.Instance.canVisitedCircels);

                   canVisitedCircles.Remove(this.gameObject);
                   foreach (var item in canVisitedCircles)
                   {
                       item.transform.GetChild(0).gameObject.SetActive(true);
                   }
                   
               }

           }
       }
       else
       {
           GameManager.Instance.goalCity = this.transform.parent.gameObject.GetComponent<City>(); // basılan cirle'ın parent objesi bizim hedef şehrimiiz
           GameManager.Instance.goalCity.gameObject.tag = "GC";
           foreach (var item in GameManager.Instance.canVisitedCircels)
           {
               item.transform.GetChild(0).gameObject.SetActive(false);
           }

       }
       
   }

  

   private void OnMouseUp()
   {
       if (GameManager.Instance.firtTouch)
       {
           if (GameManager.Instance.selectedCity.Country.Equals(GameManager.Instance.playerCountry))
               GameManager.Instance.firtTouch = false;
          
       }
          
          
       else if(!GameManager.Instance.firtTouch)
       {
           GameManager.Instance.CopyAttackObject(GameManager.Instance.selectedCity.gameObject,GameManager.Instance.goalCity.gameObject); 
           GameManager.Instance.firtTouch = true;
       }
   }

   private void ResetCities()
   {
       if (GameManager.Instance.selectedCity != null || GameManager.Instance.goalCity != null)
       {
           print("reset");
           GameManager.Instance.selectedCity.gameObject.tag = "Bolgeler";
           GameManager.Instance.goalCity.gameObject.tag =  "Bolgeler";
           GameManager.Instance.selectedCity = null;
           GameManager.Instance.goalCity = null;
       }
      
       
   }
   
   private List<GameObject> CreateList(List<GameObject> list)
   {
       List<GameObject> copy = new List<GameObject>();

       foreach (var item in list)
       {
           copy.Add(item);
       }

       return copy;
   }
}
