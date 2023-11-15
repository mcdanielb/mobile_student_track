using CommunityToolkit.Maui.Markup;
using Plugin.LocalNotification;

namespace bmcdani_c971_task;

public class NotificationData
{
    private static HashSet<int> ScheduledNotificationIds = new HashSet<int>();

    public static int GenerateNotificationId(int baseId, string uniqueName)
    {
        int hash = uniqueName.GetHashCode();
        int positiveHash = Math.Abs(hash % 10000);
        return baseId * 10000 + positiveHash;
    }

    public static async Task ScheduleNotification(int notificationId, string title, string description, DateTime selectedDate)
    {
        if (ScheduledNotificationIds.Contains(notificationId))
        {
            return;
        }

        ScheduledNotificationIds.Add(notificationId);

        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        DateTime notifyTime;

        if (selectedDate == DateTime.Today)
        {
            notifyTime = DateTime.Now;
        }
        else
        {
            notifyTime = selectedDate;
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

    public static async Task CancelNotification(int notificationId)
    {
        ScheduledNotificationIds.Remove(notificationId);
        LocalNotificationCenter.Current.Cancel(notificationId);
    }

    public static async Task ScheduleOrCancelNotification(bool isChecked, int baseNotificationId, string uniqueName, string title, string description, DateTime notifyTime)
    {
        int notificationId = GenerateNotificationId(baseNotificationId, uniqueName);

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

