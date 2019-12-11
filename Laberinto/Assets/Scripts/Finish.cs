using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Finish : MonoBehaviour
{
    public TextMeshProUGUI winner;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            winner.text = "Ganador";
        }
    }
}
