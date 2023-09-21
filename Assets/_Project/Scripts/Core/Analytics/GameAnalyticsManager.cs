using System.Collections.Generic;
using UnityEngine;
#if USE_ADJUST
using com.adjust.sdk;
#endif //USE_ADJUST

#if USE_APPSFLYER
using AppsFlyerSDK;
#endif //USE_APPSFLYER
namespace Core
{
    public class GameAnalyticsManager : SingletonMono<GameAnalyticsManager>
    {
        [Header("Setting"), SerializeField] private bool _logEnable = true;

        private GameAnalyticsService _service;
        public GameAnalyticsService Service => _service;

        protected void Awake()
        {
            _service = new GameAnalyticsService(_logEnable);
#if USE_FIREBASE
            _service.AddService(new FirebaseAnalytics());
#endif //USE_FIREBASE
        }
#if USE_FIREBASE
        public void LogEvent(string name, Firebase.Analytics.Parameter[] parameters)
        {
            _service.LogEvent(name, parameters);
        }

        public void LogEvent(string name, string parameterName, string parameterValue)
        {
            _service.LogEvent(name, parameterName, parameterValue);
        }
#endif

        public void SetProperties(string name, string value)
        {
            _service.SetProperties(name, value);
        }

        public void LogEvent(string name)
        {
            _service.LogEvent(name);
        }
    }
}