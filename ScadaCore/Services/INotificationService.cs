namespace ScadaCore.Services;

public interface INotificationService {
    public Task SendTrendingNotification(string message);

    public Task SendAlarmNotification(string message);
}