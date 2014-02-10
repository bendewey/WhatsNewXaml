using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using NewXaml.Controls;

namespace NewXaml
{
    public sealed partial class HubPage
    {
        public HubPage()
        {
            this.InitializeComponent();
            CodeHubSection.Loaded += (s, e) => AttemptToLoadCodeFile();
        }

        public CodeViewer CodeViewer1
        {
            get { return FindVisualChild<Controls.CodeViewer>(CodeHubSection); }
        }

        private TChild FindVisualChild<TChild>(DependencyObject obj) where TChild : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChild)
                    return (TChild)child;
                else
                {
                    TChild childOfChild = FindVisualChild<TChild>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
