using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using NewXaml.Controls;
using QSF.Infrastructure.Model;

namespace NewXaml.Common
{
    public class ExamplePage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ExamplePage()
        {
            this.navigationHelper = new NavigationHelper(this);
            Loaded += ExamplePage_Loaded;
        }

        void ExamplePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            AttemptToLoadCodeFile();
        }

        public async void AttemptToLoadCodeFile()
        {
            var codeViewer = GetProperty<CodeViewer>("CodeViewer1");
            if (codeViewer != null)
            {
                var files = await CodeFileInfoFactory.GetCodeFileForType(GetType());
                codeViewer.CodeFile = files.FirstOrDefault();
            }
            var codeFlipViewer = FindName("CodeFlipViewer1") as FlipView;
            if (codeFlipViewer != null)
            {
                var files = await CodeFileInfoFactory.GetCodeFileForType(GetType());
                codeFlipViewer.ItemsSource = files;
            }
        }

        private T GetProperty<T>(string propertyName)
        {
            var type = GetType().GetTypeInfo();
            var matchProperty = type.DeclaredProperties.FirstOrDefault(p => p.Name == propertyName);
            if (matchProperty != null)
            {
                return (T)matchProperty.GetMethod.Invoke(this, new object[] { }); 
            }
            var matchField = type.DeclaredFields.FirstOrDefault(p => p.Name == propertyName);
            if (matchField != null)
            {
                return (T)matchField.GetValue(this);
            }
            return default(T);
        }
    }
}
