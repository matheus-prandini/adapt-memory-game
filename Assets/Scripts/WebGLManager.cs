using UnityEngine;
using System.Runtime.InteropServices;

public class WebGLManager : MonoBehaviour
{
    public static WebGLManager Instance { get; private set; }

    public string AuthToken { get; private set; }
    public string SchoolId { get; private set; }
    public string Discipline { get; private set; }
    public string Subarea { get; private set; }
    public string GameId { get; private set; }
    public string SessionNumber { get; private set; }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void UnityReady();
#endif

    void Start()
    {
        Debug.Log("[WebGLManager] Start");
#if UNITY_WEBGL && !UNITY_EDITOR
    #if UNITY_2022_3_OR_NEWER && !UNITY_6000_0_OR_NEWER
        Debug.Log("Initializing App for older versions than UNITY_6000");
        Application.ExternalEval("window.onUnityReady && window.onUnityReady()");
    #else
        Debug.Log("Initializing App for newer versions from UNITY_6000");
        UnityReady();
    #endif
#endif
    }

    void Awake()
    {
        Debug.Log("[WebGLManager] Awake");
        if (Instance == null)
        {
            Debug.Log($"[WebGLManager] Awake - Initializing gameobject Instance: {gameObject}");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Debug.Log($"[WebGLManager] Awake - Destroying gameobject: {gameObject}");
            Destroy(gameObject);
        }

        Debug.Log("Finalizando Awake");
    }

    public void OnReceiveAuthToken(string token)
    {
        Debug.Log("[WebGLManager] - OnReceiveAuthToken");
        AuthToken = token;
        Debug.Log("Token recebido: " + AuthToken);
    }

    public void OnReceiveParams(string jsonParams)
    {
        Debug.Log("[WebGLManager] - OnReceiveParams");
        try
        {
            Debug.Log("Parâmetros recebidos: " + jsonParams);
            var data = JsonUtility.FromJson<ParamsData>(jsonParams);

            SchoolId = data.schoolId;
            Discipline = data.discipline;
            Subarea = data.subarea;
            GameId = data.gameId;
            SessionNumber = data.sessionNumber;

            Debug.Log($"SchoolId: {SchoolId}, Discipline: {Discipline}, Subarea: {Subarea}, GameId: {GameId}, SessionNumber: {SessionNumber}");
        }
        catch
        {
            Debug.LogWarning("Falha ao parsear params JSON.");
        }
    }

    [System.Serializable]
    private class ParamsData
    {
        public string discipline;
        public string subarea;
        public string schoolId;
        public string gameId;
        public string sessionNumber;
    }
}