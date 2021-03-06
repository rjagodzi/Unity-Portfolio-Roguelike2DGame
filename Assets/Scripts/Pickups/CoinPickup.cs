using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinAmount = 25;
    private bool pickedUp;
    [SerializeField] int sfxToPlay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pickedUp && GetComponent<PickupDelayer>().canBePickedUpMethod())
        {
            pickedUp = true;

            GameManager.instance.AddCoins(coinAmount);
            AudioManager.instance.PlaySFX(sfxToPlay);

            Destroy(gameObject);
        }
    }

    public void PlaySound()
    {
        AudioManager.instance.PlaySFX(sfxToPlay);
    }

}
