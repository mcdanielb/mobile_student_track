using Plugin.LocalNotification;
using System.Security.Cryptography.X509Certificates;

namespace bmcdani_c971_task;

public class NotificationData
{
    public async Task ScheduleNotification(int notificationId, string title, string description, DateTime notifyTime)
    {
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        var notification = new NotificationRequest
        {
            NotificationId = notificationId,
            Title = title,
            Description = description,
            Schedule =
        {
            NotifyTime = notifyTime
        }
        };
        await LocalNotificationCenter.Current.Show(notification);
    }

    public async Task CancelNotification(int notificationId)
    {
        LocalNotificationCenter.Current.Cancel(notificationId);
    }

    public async Task ScheduleOrCancelNotification(bool isChecked, int notificationId, string title, string description, DateTime notifyTime)
    {
        if (isChecked)
        {
            await ScheduleNotification(notificationId, title, description, notifyTime);
        }
        else
        {
            await CancelNotification(notificationId);
        }
    }
}

