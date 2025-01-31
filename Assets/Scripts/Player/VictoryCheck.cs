using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class VictoryCheck : MonoBehaviour
{

    public TextMeshProUGUI crabCount; //Crab counter UI
    private GameObject[] crabList; //Array of every crab in the level
    private int currentCrabs;
    private int killCount = 0; //How many crabs have been bubbled

    // Start is called before the first frame update
    void Start()
    {
        crabList = GameObject.FindGameObjectsWithTag("Enemy");
        currentCrabs = crabList.Length; 
    }

    // Update is called once per frame
    void Update()
    {
        currentCrabs = GameObject.FindGameObjectsWithTag("Enemy").Length;
        killCount = (crabList.Length - currentCrabs);
        crabCount.text = string.Format("{0}/{1}", killCount, crabList.Length);
    }
}
