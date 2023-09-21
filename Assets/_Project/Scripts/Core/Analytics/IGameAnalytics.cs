public interface IGameAnalytics
{
    void Init();
    void LogEvent(string name);
    void SetUserProperty(string name, string value);
    //void Update();
    void OnApplicationPause(bool isPaused);
    void UpdateConversionValue(int value);
}