using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public static class WTMITag
{
    const string Key = "WayTooMuchInfo";
    static LoopRunner runner;

    public static void Enable()
    {
        if (PhotonNetwork.LocalPlayer == null) return;

        var props = new Hashtable { { Key, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public static void Disable()
    {
        Stop();
        if (PhotonNetwork.LocalPlayer == null) return;

        var props = new Hashtable { { Key, null } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    // Starts the once-per-second check. Safe to call more than once.
    public static void Looping()
    {
        if (runner != null) return;

        var go = new GameObject("WTMITagRunner");
        Object.DontDestroyOnLoad(go);
        runner = go.AddComponent<LoopRunner>();
        runner.StartCoroutine(Loop());
    }

    public static void Stop()
    {
        if (runner == null) return;
        Object.Destroy(runner.gameObject);
        runner = null;
    }

    static IEnumerator Loop()
    {
        var wait = new WaitForSeconds(1f);

        while (true)
        {
            var player = PhotonNetwork.LocalPlayer;

            if (player != null && PhotonNetwork.InRoom &&
                !player.CustomProperties.ContainsKey(Key))
            {
                Enable();
            }

            yield return wait;
        }
    }

    class LoopRunner : MonoBehaviour { }
}
