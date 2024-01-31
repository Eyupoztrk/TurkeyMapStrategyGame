using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackObject : MonoBehaviour
{

    private Vector3 direction;
    private float speed;
    private bool canGo;

    public GameObject selectedCity;
    public GameObject goalCity;
    

    public void AddSpeed(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        canGo = true;
    }

    private void Update()
    {
        if (canGo)
        {
            transform.Translate(-direction * speed );
        }
        
    }

   

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if(GameManager.Instance.goalCity != null && col.gameObject.transform.parent != null)
        {
            if (col.gameObject.transform.parent.tag.Equals(GameManager.Instance.goalCity.tag))
            {
                Destroy(this.gameObject);
            } 
        }

        if (col.gameObject != null  && col.gameObject.transform.parent != null)
        {
            if (col.gameObject.transform.parent.tag.Equals(this.goalCity.gameObject.tag))
            {
                this.goalCity.transform.tag = "Bolgeler";
                this.selectedCity.transform.tag = "Bolgeler";
                Destroy(this.gameObject);
            }
        }
      
        
    }
}
