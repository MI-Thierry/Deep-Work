using Microsoft.UI.Xaml.Controls;

namespace DeepWork.UserControls
{
    public sealed partial class DateSlider : UserControl
    {
        public Button RightButton { get; set; }
        public Button LeftButton { get; set; }
        public Button DateDisplay { get; set; }
        public DateSlider()
        {
            this.InitializeComponent();
            RightButton = m_RightButton;
            LeftButton = m_LeftButton;
            DateDisplay = m_DateDisplay;
        }
    }
}
