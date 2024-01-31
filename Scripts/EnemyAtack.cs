using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EnemyAtack : Agent
{
    [SerializeField] private GameManager.Countries Country;
    public GameManager.Countries COUNTRY => Country;
    public City selectedCity;
    public City goalCity;
    [SerializeField] private float attackSpeed;
    private bool isWin;
    private bool isDone = false;

    [Header("Country properties")] 
    [HideInInspector] public int numberOfCities;
    public int cityAmount;
    public Color32 countryColor;
    [HideInInspector] public List<City> firstCities;
    
    public enum AttackType
    {
        RANDOM,
        JUST_NEIGHBOR,
        MACHINE_LEARNING
    }

    public AttackType _AttackType;
    
    
    

    private void Start()
    {
        SetColors();
        InitializeCountry(); 
        numberOfCities = GameManager.Instance.GetNumberOfCities(Country);
        GameManager.Instance.OnGetOurCity += SetRewardForGetingCity;
        GameManager.Instance.OnGetOtherCity += SetRewardForGetingCity;
        StartCoroutine(AttackW());
    }

    
    private void Update()
    {
        if (cityAmount == GameManager.Instance.cities.Count)
        {
            print("girdi");
            isWin = true;
        }
           
        
        if (isWin)
        {
            GameManager.Instance.OnWin += Win;
            cityAmount = numberOfCities;
            isWin = false;
        }
        
        if (!isCountryExist())
        {
            GameManager.Instance.OnDisappear += Disappear;
            cityAmount = numberOfCities;
            if(!_AttackType.Equals(AttackType.MACHINE_LEARNING))
                 this.gameObject.SetActive(false);
        }

      
    }


    private void SetColors()
    {
        if (Country.Equals(GameManager.Countries.country1))
        {
            countryColor = Color.red;
        }else if (Country.Equals(GameManager.Countries.country2))
        {
            countryColor = Color.blue;
        }else if (Country.Equals(GameManager.Countries.country5))
        {
            countryColor = Color.yellow;
        }
        else if (Country.Equals(GameManager.Countries.country4))
        {
            countryColor = Color.cyan;
        }
    }

    private void InitializeCountry()
    {
        foreach (var item in GameManager.Instance.cities)
        {
            if (item.GetComponent<City>().Country.Equals(Country))
            {
                item.GetComponent<SpriteRenderer>().color = countryColor;
                firstCities.Add(item.GetComponent<City>());
                
                
            }
              
        }

        // numberOfCities = GameManager.Instance.GetNumberOfCities(Country);
        GameManager.Instance.Enemies.Add(this.gameObject);
    }
    
    private int FindIndexOfCity(City city)
    {
        for (int i = 0; i < GameManager.Instance.cities.Count; i++)
        {
            if (city.Equals(GameManager.Instance.cities[i].GetComponent<City>()))
                return i;
        }

        return 0;
    }

    private bool CheckIsOurCity(City city)
    {
        if (city.Country.Equals(Country))
            return true;
        else
        {
            return false;
        }
    }
    
    private bool isCountryExist()
    {
        foreach (var item in GameManager.Instance.cities)
        {
            if (item.GetComponent<City>().Country.Equals(Country))
                return true;
        }
        return false;
    }

    private void Disappear()
    {
        print(Country + " has disappeared");
        if (_AttackType.Equals(AttackType.MACHINE_LEARNING))
        {
            SetReward(-2f);
            EndEpisode();
        }
    }
    
    public void Win()
    {
        print(Country + " win");
        if (_AttackType.Equals(AttackType.MACHINE_LEARNING))
        {
            SetReward(15f);
            EndEpisode();
        }
    }
    
    private List<City> FindBorderCities()
    {
        List<City> borderCities = new List<City>();

        foreach (var item in GameManager.Instance.cities)
        {
            City Icity = item.GetComponent<City>();
            if (Icity.Country.Equals(Country)) // bizim şehrimiz ise
            {
                // Komşularına tek tek bak
                foreach (var neighbor in Icity.GetNeighbors())
                {
                    // Eğer komşularından birinin ülkesi bizim ülkemiz değilse yani başka bir ülkeyle komşu ise
                    if (!neighbor.GetComponent<City>().Country.Equals(Country))
                    {
                        borderCities.Add(Icity);
                        break;
                    }
                }
            }
        }


        return borderCities;
    }

    

   


    #region NormalAIAplication

    public void Attack()
    {
        if (isCountryExist())
        {
            if (_AttackType.Equals(AttackType.RANDOM))
            {
                do
                {
                    selectedCity = ChooseRandomCities();
                    goalCity = ChooseRandomCities();
                } 
                while (!selectedCity.Country.Equals(Country));
                
                if (!selectedCity.Country.Equals(GameManager.Instance.playerCountry)) // yapay zeka ise
                {
                    GameManager.Instance.CopyAttackObject(selectedCity.gameObject,goalCity.gameObject);
                    GameManager.Instance.Attack(selectedCity,goalCity);
                
                }
            
               
            }
            else if (_AttackType.Equals(AttackType.JUST_NEIGHBOR))
            {
                SetCitiesForNeighbors();
                if (!selectedCity.Country.Equals(GameManager.Instance.playerCountry)) // yapay zeka ise
                {
                    GameManager.Instance.CopyAttackObject(selectedCity.gameObject,goalCity.gameObject);
                    GameManager.Instance.Attack(selectedCity,goalCity);
                
                }
            }

            
        }
        cityAmount = GameManager.Instance.GetNumberOfCities(this.Country);
        


    }

    

    private int randomIndex;
    private City ChooseRandomCities()
    {
        randomIndex = Random.Range(0, GameManager.Instance.cities.Count);
        return GameManager.Instance.cities[randomIndex].GetComponent<City>();
    }
     private void SetCitiesForNeighbors()
    {
        do
        {
            var randomIndex = Random.Range(0, GameManager.Instance.cities.Count);
            selectedCity = GameManager.Instance.cities[randomIndex].GetComponent<City>();
            
        } while (!selectedCity.Country.Equals(Country));  // eğer seçilen şehir sınır şehri ise tamamdır

        
        bool isFind = false;
        List<City> goalCities = new List<City>();
        foreach (var borderCity in FindBorderCities())
        {
            foreach (var item in borderCity.GetNeighbors())
            {
                if (!item.Country.Equals(Country))
                {
                    goalCities.Add(item);
                }
            }
        }
        var random = Random.Range(0, goalCities.Count);
        goalCity = goalCities[random];
    }
     
    private IEnumerator AttackW()
    {
        while (true)
        {
            isDone = false;
            yield return new WaitForSeconds(attackSpeed);
            Attack();
            isDone = true;
        }
    }

    #endregion



    #region ML Agent
    
    /// <summary>
    /// This part has Machine Learning functions
    /// </summary>


    
    public override void OnEpisodeBegin()
    {
        if (_AttackType.Equals(AttackType.MACHINE_LEARNING))
        {
            print("EndEpisode");
            GameManager.Instance.ResetGame();
        }
             
           
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (_AttackType.Equals(AttackType.MACHINE_LEARNING))
        {
            int randomIndex1 = Random.Range(0, GameManager.Instance.cities.Count); // saldırının yapılacağı birinci şehir random olacaktır
            int randomIndex2 = actions.DiscreteActions[0]; // saldırının yapıldığı şehir AI tarafından seçilecektir

            selectedCity = GameManager.Instance.cities[randomIndex1].GetComponent<City>();

            if (selectedCity.Country.Equals(Country))
            {
                goalCity = GameManager.Instance.cities[randomIndex2].GetComponent<City>();
            
                GameManager.Instance.CopyAttackObject(selectedCity.gameObject,goalCity.gameObject);
                GameManager.Instance.Attack(selectedCity,goalCity);
            
               Debug.Log(actions.DiscreteActions[0]);
            }
        }
           
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (var item in GameManager.Instance.cities)
       {
           if (!item.GetComponent<City>().Country.Equals(Country))
           {
               sensor.AddObservation(FindIndexOfCity(item.GetComponent<City>()));
               //sensor.AddObservation(item.GetComponent<City>().CityPower);
           }
              
       }
    }


    public void SetRewardForGetingCity(bool isOtherCity)
    {
        if(isOtherCity)
            SetReward(10f);
        else 
            SetReward(5f);
        
    }
    
    #endregion
   
}
