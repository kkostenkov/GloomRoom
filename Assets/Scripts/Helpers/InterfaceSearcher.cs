using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InterfaceSearcher : MonoBehaviour {

    // Get all components that implement specified interface;
    public static void GetInterfaces<T>(out List<T> resultList, GameObject objectToSearch) where T : class
    {
        if (objectToSearch == null)
        {
            resultList = new List<T>();
            return;
        } 
        MonoBehaviour[] list = objectToSearch.GetComponents<MonoBehaviour>();
        resultList = new List<T>();
        foreach (MonoBehaviour mb in list)
        {
            if (mb is T)
            {
                //found one
                resultList.Add((T)((System.Object)mb));
            }
        }
    }
}
