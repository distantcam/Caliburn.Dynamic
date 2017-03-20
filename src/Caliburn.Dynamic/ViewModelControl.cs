using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace Caliburn.Dynamic
{
    [TemplatePart(Name = "PART_Presenter", Type = typeof(ContentPresenter))]
    public class ViewModelControl : ContentControl
    {
        static ViewModelControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewModelControl), new FrameworkPropertyMetadata(typeof(ViewModelControl)));
        }

        public static readonly DependencyProperty ContextProperty =
            DependencyProperty.Register("Context", typeof(object), typeof(ViewModelControl), new FrameworkPropertyMetadata(null, OnContextChanged));

        ContentPresenter presenter;

        public object Context
        {
            get { return GetValue(ContextProperty); }
            set { SetValue(ContextProperty, value); }
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            View.SetModel(presenter, newContent);
        }

        public override void OnApplyTemplate()
        {
            presenter = (ContentPresenter)GetTemplateChild("PART_Presenter");

            presenter.Loaded += Presenter_Loaded;
        }

        private void Presenter_Loaded(object sender, RoutedEventArgs e)
        {
            presenter.Loaded -= Presenter_Loaded;

            View.SetModel(presenter, Content);
        }

        static void OnContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            View.SetContext(((ViewModelControl)d).presenter, e.NewValue);
        }
    }
}