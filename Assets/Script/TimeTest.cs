using System;
using System.Threading;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
   public bool isTimeIncrased;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Space))
      {
          isTimeIncrased = !isTimeIncrased;
          if (isTimeIncrased)
          {
              Time.timeScale = 5;
          }
          else
          {
              Time.timeScale = 1;
          }
      }
   }
}
