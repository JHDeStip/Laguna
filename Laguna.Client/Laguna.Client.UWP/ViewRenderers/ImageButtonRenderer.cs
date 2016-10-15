
using JhDeStip.Laguna.Client.CustomControls;
using JhDeStip.Laguna.Client.UWP.ViewRenderers;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace JhDeStip.Laguna.Client.UWP.ViewRenderers
{
    public class ImageButtonRenderer : ImageRenderer
    {
        private ImageSource _defaultDrawable = null, _pressedDrawable = null, _disabledDrawable = null;
        private ImageButton _imageButton;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                _imageButton = (ImageButton)e.NewElement;
                _defaultDrawable = new BitmapImage(new Uri($@"ms-appx:///Assets/{_imageButton.DefaultImage.File}.png"));
                _pressedDrawable = new BitmapImage(new Uri($@"ms-appx:///Assets/{_imageButton.PressedImage.File}.png"));
                _disabledDrawable = new BitmapImage(new Uri($@"ms-appx:///Assets/{_imageButton.DisabledImage.File}.png"));
                Control.Source = _defaultDrawable;

                Control.PointerEntered += OnPointerEntered;
                Control.PointerExited += OnPointerExited;
                Control.Tapped += OnTapped;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(_imageButton.IsEnabled))
            {
                if (!_imageButton.IsEnabled)
                    Control.Source = _disabledDrawable;
                else if (Control.ManipulationMode == ManipulationModes.All)
                    Control.Source = _pressedDrawable;
                else
                    Control.Source = _defaultDrawable;
            }

            else if (e.PropertyName == nameof(_imageButton.DefaultImage))
            {
                _defaultDrawable = new BitmapImage(new Uri($@"ms-appx:///Assets/{_imageButton.DefaultImage.File}.png"));
                if (_imageButton.IsEnabled && Control.ManipulationMode != ManipulationModes.All)
                    Control.Source = _defaultDrawable;
            }
            else if (e.PropertyName == nameof(_imageButton.PressedImage))
            {
                _pressedDrawable = new BitmapImage(new Uri($@"ms-appx:///Assets/{_imageButton.PressedImage.File}.png"));
                if (_imageButton.IsEnabled && Control.ManipulationMode == ManipulationModes.All)
                    Control.Source = _pressedDrawable;
            }
            else if (e.PropertyName == nameof(_imageButton.DisabledImage))
            {
                _disabledDrawable = new BitmapImage(new Uri($@"ms-appx:///Assets/{_imageButton.DisabledImage.File}.png"));
                if (!_imageButton.IsEnabled)
                    Control.Source = _disabledDrawable;
            }
        }

        public void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (_imageButton.IsEnabled)
                Control.Source = _pressedDrawable;
        }

        public void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (_imageButton.IsEnabled)
                Control.Source = _defaultDrawable;
        }

        public void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            _imageButton.Command.Execute(_imageButton.CommandParameter);
        }
    }
}