using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DeepWork.UserControls
{
    public sealed partial class TimeInputBox : UserControl
    {
        public TextBox MinuteDisplay { get => m_MinuteDisplay; set => m_MinuteDisplay = value; }
        int m_MinuteValue;
        public TimeInputBox()
        {
            m_MinuteValue = 30;
            this.InitializeComponent();
            MinuteDisplay.Text = m_MinuteValue.ToString();
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_MinuteValue < 960)
            {
                DownButton.IsEnabled = true;
                m_MinuteValue += 35;
                MinuteDisplay.Text = m_MinuteValue.ToString();
            }
            else
                UpButton.IsEnabled = false;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_MinuteValue > 30)
            {
                UpButton.IsEnabled = true;
                m_MinuteValue -= 35;
                MinuteDisplay.Text = m_MinuteValue.ToString();
            }
            else
                DownButton.IsEnabled = false;
        }
    }
}
