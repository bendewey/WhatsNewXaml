using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NewXaml.Controls
{
    public class PageContent : ContentControl
    {
        public static readonly DependencyProperty GoBackCommandProperty =
            DependencyProperty.Register("GoBackCommand", typeof(ICommand), typeof(PageContent), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(PageContent), new PropertyMetadata(null));
        
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(PageContent), new PropertyMetadata(null));

        public PageContent()
        {
            DefaultStyleKey = typeof (PageContent);
            SizeChanged += PageContent_SizeChanged;
        }

        void PageContent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Padding.Left <= 0) return;

            var container = GetTemplateChild("Container") as Grid;
            if (container != null)
            {
                if (e.NewSize.Width < 760)
                {
                    Padding = new Thickness(40, 20, 20, 20);
                }
                else
                {
                    Padding = new Thickness(140, 40, 80, 40);
                }    
            }
        }

        public ICommand GoBackCommand
        {
            get { return (ICommand)GetValue(GoBackCommandProperty); }
            set { SetValue(GoBackCommandProperty, value); }
        }

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
    }
}
