using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock_Script : MonoBehaviour
{
    // Variables
    Transform hour_hand, minute_hand, second_hand;
    // Start is called before the first frame update
    void Start()
    {
        hour_hand = transform.GetChild(1);
        minute_hand = transform.GetChild(2);
        second_hand = transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        // Get current time
        DateTime now = DateTime.Now;
        int hour = now.Hour;
        int minute = now.Minute;
        int second = now.Second;
        // Debug
        // Debug.Log($"Time right now is {hour}:{minute}:{second}");
        // Get seconds since half day
        int seconds_since_half_day = (hour % 12) * 3600 + minute * 60 + second;
        // Set hour hand
        float new_hour_rotation = (float) seconds_since_half_day / (12 * 3600) * 360;
        hour_hand.localEulerAngles = new Vector3(new_hour_rotation, 0 ,0);
        // Get seconds since new hour
        int seconds_since_hour = minute * 60 + second;
        // Set minute hand
        float new_minute_rotation = (float) seconds_since_hour / 3600 * 360;
        minute_hand.localEulerAngles = new Vector3(new_minute_rotation, 0 ,0);
        // Set second hand
        float new_second_rotation = (float) second  / 60 * 360;
        second_hand.localEulerAngles = new Vector3(new_second_rotation, 0, 0);
    }
}
