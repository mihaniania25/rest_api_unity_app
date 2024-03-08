using System;
using UnityEngine;

namespace TABApps.TestTask
{
    [Serializable]
    public class ButtonData
    {
        public string id;

        public string text;
        public string color;
        public bool appearAnimEnabled;
        public bool disappearAnimEnabled;

        public void Update(ButtonData newData)
        {
            text = newData.text; 
            color = newData.color;
            appearAnimEnabled = newData.appearAnimEnabled;
            disappearAnimEnabled = newData.disappearAnimEnabled;
        }

        public ButtonData Clone()
        {
            return new ButtonData
            {
                id = id,
                text = text,
                color = color,
                appearAnimEnabled = appearAnimEnabled,
                disappearAnimEnabled = disappearAnimEnabled
            };
        }

        public void RandomizeColor()
        {
            Color newColor = new Color
            (
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                1f
            );

            color = $"#{ColorUtility.ToHtmlStringRGB(newColor)}";
        }
    }
}