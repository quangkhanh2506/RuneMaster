using System.Collections.Generic;
using UnityEngine;

public partial class GameAnalyticsService
{
    private List<IGameAnalytics> _gameAnalytics = new List<IGameAnalytics>();
    private bool _logEnable = false;
    
    // public partial void GameStart();

    public GameAnalyticsService(bool logEnable)
    {
        _logEnable = logEnable;
    }
    
    public void AddService(IGameAnalytics analytic)
    {
        analytic.Init();
        _gameAnalytics.Add(analytic);
    }
    
#if USE_FIREBASE 
    public void LogEvent(string name, Firebase.Analytics.Parameter[] parameters)
    {
        if (_logEnable)
        {
            Debug.Log("LogEvent name:" + name + "  - parameters:" + parameters.Length);
        }
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameters);	
    }

    public void LogEvent(string name, string parameterName, string parameterValue)
    {
        if (_logEnable)
        {
            Debug.Log("LogEvent name:" + name);
        }
        Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
    }
#endif //USE_FIREBASE
	
    public void SetProperties(string name, string value)
    {
        foreach (IGameAnalytics analytic in _gameAnalytics)
        {
            analytic.SetUserProperty(name, value);
        }
    }

    public void LogEvent(string name)
    {
        if (_logEnable)
        {
            Debug.Log("LogEvent name:" + name);
        }
        foreach (IGameAnalytics analytic in _gameAnalytics)
        {
            analytic.LogEvent(name);
        }
    }
}