using Microsoft.Windows.AppNotifications;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace DeepWork.ViewModels
{
    public enum SessionType
    {
        None,
        Break,
        Focus,
    }
    public class FocusSessionManager : INotifyPropertyChanged
    {
        private TimeSpan m_CountDown;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<SessionType> SessionEnded;
        public event Action<TimeSpan> WholeSessionEnded;

        public Timer Timer { get; set; }
        public int TotalFocusCount { get; set; }
        public int TotalBreakCount { get; set; }
        public int CurrentFocusCount { get; set; }
        public int CurrentBreakCount { get; set; }
        public TimeSpan SessionDuration { get; set; }
        public TimeSpan CountDown
        {
            get => m_CountDown;
            set
            {
                if (m_CountDown != value)
                {
                    m_CountDown = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public TimeSpan ElapsedTime { get; set; }
        public SessionType SessionType { get; set; }
        public FocusSessionManager()
        {
            SessionType = SessionType.None;
        }

        public void StartSession(TimeSpan duration)
        {
            CurrentFocusCount = TotalFocusCount = (int)duration.TotalMinutes / 30;
            CurrentBreakCount = TotalBreakCount = CurrentFocusCount - 1;
            SessionDuration = duration;
            SessionType = SessionType.Focus;
            Timer = new Timer(new TimerCallback(TimerTick), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1));
        }

        private void TimerTick(object state)
        {
            if (SessionDuration > TimeSpan.Zero)
            {
                if (CountDown != TimeSpan.Zero)
                {
                    CountDown = CountDown.Subtract(TimeSpan.FromSeconds(1));
                    SessionDuration = SessionDuration.Subtract(TimeSpan.FromSeconds(1));
                    ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(1));
                }
                else
                {
                    if (CurrentBreakCount < CurrentFocusCount)
                    {
                        CountDown = TimeSpan.FromMinutes(30);
                        CurrentFocusCount--;
                        SessionEnded?.Invoke(SessionType.Break);
                        SessionType = SessionType.Focus;
                        PlayRingTone();
                        RaiseNotification($"Now it's time for 30 minute focus time.");
                    }
                    else
                    {
                        CountDown = TimeSpan.FromMinutes(5);
                        CurrentBreakCount--;
                        SessionEnded?.Invoke(SessionType.Focus);
                        SessionType = SessionType.Break;
                        PlayRingTone();
                        RaiseNotification($"Now it's time for 5 minute break time.");
                    }
                }
            }
            else
            {
                StopFocusSession();
            }
        }

        public void StopFocusSession()
        {
            Timer.Dispose();
            SessionType = SessionType.None;
            CountDown = TimeSpan.Zero;
            WholeSessionEnded?.Invoke(ElapsedTime);
            RaiseNotification("Focus session is stopped.");
        }

        private void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        void PlayRingTone()
        {
            MediaPlayer mediaPlayer = new()
            {
                Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/alarm1.wav")),
                Volume = 5,
            };
            mediaPlayer.Play();
        }
        bool RaiseNotification(string message)
        {
            var xmlPayload = new string($@"
			<toast>    
			    <visual>    
			        <binding template=""ToastGeneric"">    
			            <text>{"Deep Work"}</text>
			            <text>{message}</text>    
			        </binding>
			    </visual>
			</toast>");

            var toast = new AppNotification(xmlPayload);
            AppNotificationManager.Default.Show(toast);
            return toast.Id != 0;
        }
    }
}
