using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomization<T>
{
    public static T RandomObject(ObjectPossibility[] possibleObjects)
    {
        float totalPossibility= 0;
        float[] possibilities = new float[possibleObjects.Length];


        for (int i = 0; i < possibleObjects.Length; i++)
        {
            possibilities[i] = possibleObjects[i].possibility;
            totalPossibility += possibleObjects[i].possibility;
        }

        if (totalPossibility <= 0) Debug.LogError("Possibility can not be 0");

        float roll = Random.Range(0, totalPossibility);

        for (int i = 0; i < possibilities.Length; i++)
        {
            if(roll > possibilities[i])
                roll -= possibilities[i];
            else
                return possibleObjects[i].possibleObject;
        }

        return possibleObjects[^1].possibleObject;
    }

    [System.Serializable]
    public class ObjectPossibility
    {
        [HideInInspector] public string objectName;
        public float possibility;
        public T possibleObject;
    }
}