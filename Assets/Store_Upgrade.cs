using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Store_Upgrade", menuName = "Upgrade/Store", order = 0)]
public class Store_Upgrade : ScriptableObject {
    public string title;
    public string code;
    public string description;
    public int cost;

 
    public UnityEvent methodToCallOnPurchase;

    public void Purchase() {
        methodToCallOnPurchase.Invoke();
    }
}

