using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AttackApplication : MonoBehaviour
{
    [SerializeField] private AtackObject atackObject;
    [SerializeField] private float attacObjectSpeed;

    private Vector3 FindDirection(GameObject obj1, GameObject obj2)
    {
        var direction = obj1.transform.TransformPoint(obj1.transform.localPosition) - obj2.transform.TransformPoint(obj2.transform.localPosition);
        return direction.normalized;
    }

    public void CopyAtackObject(GameObject instantiatePos, GameObject goalPos ,int insantiateAmount)
    {
        StartCoroutine(Wait(instantiatePos,goalPos,insantiateAmount));

    }
    
    
    

    private IEnumerator Wait(GameObject selectedPos, GameObject goalPos,int insantiateAmount)
    {
       
        for (int i = 0; i < insantiateAmount; i++)
        {
            AtackObject copyAtackObject = Instantiate(atackObject, selectedPos.transform.position, quaternion.identity);
            copyAtackObject.AddSpeed(FindDirection(selectedPos.transform.GetChild(0).gameObject,
                goalPos.transform.GetChild(0).gameObject), attacObjectSpeed );
            
            copyAtackObject.goalCity = goalPos.gameObject;
            copyAtackObject.selectedCity = selectedPos.gameObject;
            copyAtackObject.goalCity.gameObject.tag = "GC";
            copyAtackObject.selectedCity.gameObject.tag = "SC";
            
            yield return new WaitForSeconds(0.1f);
        }
        if(selectedPos.GetComponent<City>().Country.Equals(GameManager.Instance.playerCountry))
           GameManager.Instance.Attack();
        
    }
}
