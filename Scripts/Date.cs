using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Date : MonoBehaviour
{
    [SerializeField] float secondsPerMinute = 60f;
    [SerializeField] float minutesPerHour = 60f;
    [SerializeField] float hoursPerDay = 24f;
    [SerializeField] float daysPerYear = 365f;
    [SerializeField] float daysPerMonth = 30f; 

    [SerializeField] private float timeSpeed;
    private float currentYear;
    private float currentMonth;
    private float currentDay;
    private float totalYears;

    [Header("UI")] 
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI monthText;
    [SerializeField] private TextMeshProUGUI yearText;

    private void Start()
    {
        timeSpeed = 1;
        currentYear = 2023;
    }


    void Update()
    {

        Time.timeScale = timeSpeed;
        
        float totalTime = Time.time;
        float totalMinutes = totalTime / secondsPerMinute;
        float totalHours = totalMinutes / minutesPerHour;
        float totalDays = totalHours / hoursPerDay;
        
        totalYears = currentYear + (totalDays / daysPerYear);
        currentMonth = ((totalYears - currentYear) % 1) * 12 + 1;
        currentDay = (totalDays % daysPerMonth) + 1; // Günü hesapla

      /*  Debug.Log("Yıl: " + Mathf.Floor(totalYears));
        Debug.Log("Ay: " + Mathf.Floor(currentMonth));
        Debug.Log("Gün: " + Mathf.Floor(currentDay));*/
        SetTexts();
    }


    private void SetTexts()
    {
        yearText.text = ((int)(totalYears)).ToString();
        monthText.text = ((int)(currentMonth)).ToString();
        dayText.text = ((int)(currentDay)).ToString();
    }
}
