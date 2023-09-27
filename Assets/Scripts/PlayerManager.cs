using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoginRoutine());
    }

    IEnumerator LoginRoutine() {
        bool done = false;

        LootLockerSDKManager.StartGuestSession((response) => {
            if (response.success) {
                Debug.Log("Player is logged in");
                PlayerPrefs.SetString("ServerPlayerID", response.player_id.ToString());
                done = true;
            } else {
                Debug.Log("Could not start session...");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
