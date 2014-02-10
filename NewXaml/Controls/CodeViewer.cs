using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Security.Cryptography.Certificates;
using Windows.System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using QSF.CodeFormatting;
using QSF.Infrastructure.Model;

namespace NewXaml.Controls
{
    public class CodeViewer : Control
    {
        // Using a DependencyProperty as the backing store for CodeFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeFileProperty =
            DependencyProperty.Register("CodeFile", typeof(CodeFileInfo), typeof(CodeViewer), new PropertyMetadata(null, OnCodeFilePropertyChanged));

        public static readonly DependencyProperty HScrollVisibilityProperty =
            DependencyProperty.Register("HScrollVisibility", typeof(ScrollBarVisibility), typeof(CodeViewer), new PropertyMetadata(ScrollBarVisibility.Auto));

        public static readonly DependencyProperty VScrollVisibilityProperty =
            DependencyProperty.Register("VScrollVisibility", typeof(ScrollBarVisibility), typeof(CodeViewer), new PropertyMetadata(ScrollBarVisibility.Auto));

        public static readonly DependencyProperty WordWrapProperty =
            DependencyProperty.Register("WordWrap", typeof(bool), typeof(CodeViewer), new PropertyMetadata(false));

        private bool isLoaded;
        private bool isTemplateApplied;
        private StackPanel panel;
        private ScrollViewer scroller;
        private bool _isDown;
        private PointerPoint _prevPoint;

        public CodeViewer()
        {
            this.DefaultStyleKey = typeof(CodeViewer);

            this.LayoutUpdated += this.OnLayoutUpdated;
        }

        public CodeFileInfo CodeFile
        {
            get
            {
                return (CodeFileInfo)this.GetValue(CodeFileProperty);
            }
            set
            {
                this.SetValue(CodeFileProperty, value);
            }
        }

        public ScrollBarVisibility HScrollVisibility
        {
            get
            {
                return (ScrollBarVisibility)this.GetValue(HScrollVisibilityProperty);
            }
            set
            {
                this.SetValue(HScrollVisibilityProperty, value);
            }
        }

        public ScrollBarVisibility VScrollVisibility
        {
            get
            {
                return (ScrollBarVisibility)this.GetValue(VScrollVisibilityProperty);
            }
            set
            {
                this.SetValue(VScrollVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Determines whether the internally used TextBlock will use text wrapping.
        /// </summary>
        public bool WordWrap
        {
            get
            {
                return (bool)this.GetValue(WordWrapProperty);
            }
            set
            {
                this.SetValue(WordWrapProperty, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.panel = this.GetTemplateChild("PART_Panel") as StackPanel;
            this.scroller = this.GetTemplateChild("PART_Scroller") as ScrollViewer;
            this.isTemplateApplied = true;
            this.TryBuildTextBlocks();
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if (!_isDown)
                return;

            var point = e.GetCurrentPoint(this);
            if (_prevPoint == null)
            {
                _prevPoint = point;
                return;
            }

            var offsetToPointerFactor = 1.0;
            var deltaX = (_prevPoint.Position.X - point.Position.X) * offsetToPointerFactor;
            var deltaY = (_prevPoint.Position.Y - point.Position.Y) * offsetToPointerFactor;

            Debug.WriteLine("dragging content to right- x=" + e.GetCurrentPoint(this).Position.X + " y=" + e.GetCurrentPoint(this).Position.Y);

            var hoffset = scroller.HorizontalOffset + deltaX;
            var voffset = scroller.VerticalOffset + deltaY;
            scroller.ChangeView(hoffset, voffset, null);

            base.OnPointerMoved(e);
            _prevPoint = point;
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            _prevPoint = null;
            _isDown = true;
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            _isDown = false;
        }

        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            if ((e.KeyModifiers & VirtualKeyModifiers.Control) == VirtualKeyModifiers.Control)
            {
                var deltaToFontSizeFactor = 0.3;
                var easingFactor = scroller.FontSize/200;
                var fontSize = scroller.FontSize + (e.GetCurrentPoint(this).Properties.MouseWheelDelta * deltaToFontSizeFactor * easingFactor);
                fontSize = Math.Min(120, Math.Max(8, fontSize));
                scroller.FontSize = fontSize;
            }
            base.OnPointerWheelChanged(e);
        }

        private static void OnCodeFilePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var codeViewer = d as CodeViewer;
            codeViewer.TryBuildTextBlocks();
        }

        private RichTextBlock CreateRichTextBlock()
        {
            RichTextBlock block = new RichTextBlock();
            block.HorizontalAlignment = HorizontalAlignment.Stretch;
            block.VerticalAlignment = VerticalAlignment.Stretch;

            if (this.WordWrap)
            {
                block.TextWrapping = TextWrapping.Wrap;
            }

            return block;
        }

        private void OnLayoutUpdated(object sender, object e)
        {
            this.isLoaded = true;

            if (!this.isTemplateApplied)
            {
                return;
            }

            this.LayoutUpdated -= this.OnLayoutUpdated;

            this.TryBuildTextBlocks();
        }

        private void TryBuildTextBlocks()
        {
            if (!this.isTemplateApplied || !this.isLoaded || this.CodeFile == null || string.IsNullOrEmpty(this.CodeFile.CodeContent))
            {
                return;
            }

            Tokenizer tokenizer = new Tokenizer();
            RichTextBlock richTextBlock = this.CreateRichTextBlock();

            var paragraph = new Paragraph();
            string codeFileExtension = this.CodeFile.Extension;
            string code = this.CodeFile.CodeContent;
            foreach (Token token in tokenizer.TokenizeCode(code, codeFileExtension))
            {
                paragraph.Inlines.Add(token.GetInline());
            }

            richTextBlock.Blocks.Add(paragraph);

            this.panel.Children.Clear();
            this.panel.Children.Add(richTextBlock);
        }
    }
}