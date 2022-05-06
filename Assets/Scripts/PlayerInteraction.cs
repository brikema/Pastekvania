using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{

    // Gestion des pièces
    public TextMeshProUGUI nbCoinText;
    private int nbCoins = 0;

    private void Awake()
    {
        nbCoinText.text = "<sprite index=0>" + Data.Coins + "<color=#00000000>.</color>";
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch(other.transform.tag){
            case("Coin"): 
                getCoin(other);
                break;
            // case("Death"): 
            //     manageDeath(other);
            //     break;
            // case("Finish"):
            //     manageFinish();
            //     break;
        }
    }

    private void getCoin(Collider2D other) {
        int coinValue = other.transform.GetComponent<ObjectCoin>().value;
        Destroy(other.gameObject);
        nbCoins = nbCoins + coinValue;
        Data.Coins = nbCoins;
        nbCoinText.text = "<sprite index=0>" + Data.Coins + "<color=#00000000>.</color>";
    }

}
