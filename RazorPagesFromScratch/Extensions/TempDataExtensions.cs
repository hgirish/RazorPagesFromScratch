﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesFromScratch.Extensions
{
    public static class TempDataExtensions
    {
        const string Alerts = "Alerts";
        public static List<Alert> GetAlert(this ITempDataDictionary tempData)
        {
            CreateAlertTempData(tempData);
            return DeserializeAlerts(tempData[Alerts] as string);
        }

        public static void CreateAlertTempData(this ITempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(Alerts))
            {
                tempData[Alerts] = "";
            }
        }
        public static void AddAlert(this ITempDataDictionary tempData, Alert alert)
        {
            if (alert == null)
            {
                throw new ArgumentNullException(nameof(alert));
            }
            var deserializeAlertList = tempData.GetAlert();
            deserializeAlertList.Add(alert);
            tempData[Alerts] = SerializeAlerts(deserializeAlertList);
        }
        public static string SerializeAlerts(List<Alert> tempData)
        {
            return JsonConvert.SerializeObject(tempData);
        }
        public static List<Alert> DeserializeAlerts(string tempData)
        {
            if (tempData.Length == 0)
            {
                return new List<Alert>();
            }
            return JsonConvert.DeserializeObject<List<Alert>>(tempData);
        }
    }

    public class Alert
    {
    }
}
