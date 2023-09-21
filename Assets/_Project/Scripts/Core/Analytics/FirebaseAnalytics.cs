#if USE_FIREBASE
using UnityEngine;

public class FirebaseAnalytics : IGameAnalytics
{
    private float _startTime;
    private const float _deltaTimeToSendEngage = 10f;

    public void Init()
    {
        _startTime = Time.time;
    }

    public void LogEvent(string name)
    {
        //Debug.Log("LogEvent >>> " + name);
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name);
    }

    public void LogEvent(string name, Firebase.Analytics.Parameter[] parameters)
    {
        //Debug.Log("LogEvent >>> " + name);
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameters);
    }

    public void LogEvent(string name, string parameterName, int parameterValue)
    {
        //Debug.Log("LogEvent >>> " + name);
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
    }

    public void OnApplicationPause(bool isPaused)
    {
        _startTime = Time.time;
    }

    public void SetUserProperty(string name, string value)
    {
        Firebase.Analytics.FirebaseAnalytics.SetUserProperty(name, value);
    }

    public void UpdateConversionValue(int value)
    {

    }
}
#endif