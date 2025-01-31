using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class VictoryCheck : MonoBehaviour
{

    public TextMeshProUGUI crabCount; //Crab counter UI
    private GameObject[] crabList; //Array of every crab in the level
    private GameObject[] crabRemain;
    public int killCount = 0; //How many crabs have been bubbled

    public bool winCon;

    // Start is called before the first frame update
    void Start()
    {
        crabList = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        crabRemain = GameObject.FindGameObjectsWithTag("Enemy");
        crabCount.text = string.Format("{0}/{1}", crabRemain.Length, crabList.Length);
        if(crabRemain.Length == 0)
        {
            winCon = true;
        }
    }
}
