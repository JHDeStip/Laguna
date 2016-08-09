using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using JhDeStip.Laguna.Client.Droid.ViewRenderers;
using Android.Graphics.Drawables;
using static Android.Views.View;
using System.ComponentModel;
using System.Windows.Input;

[assembly: ExportRenderer(typeof(JhDeStip.Laguna.Client.CustomControls.ImageButton), typeof(ImageButtonRenderer))]
namespace JhDeStip.Laguna.Client.Droid.ViewRenderers
{
    public class ImageButtonRenderer : ImageRenderer
    {
        private Drawable _defaultDrawable = null, _pressedDrawable = null, _disabledDrawable = null;
        private OnTouchListener _onTouchListener;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Image> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var context = Android.App.Application.Context;
                var imageButton = (CustomControls.ImageButton)e.NewElement;
                _defaultDrawable = context.Resources.GetDrawable(imageButton.DefaultImage);
                _pressedDrawable = context.Resources.GetDrawable(imageButton.PressedImage);
                _disabledDrawable = context.Resources.GetDrawable(imageButton.DisabledImage);
                Control.SetImageDrawable(_defaultDrawable);

                _onTouchListener = new OnTouchListener() { DefaultDrawable = _defaultDrawable, PressedDrawable = _pressedDrawable, DisabledDrawable = _disabledDrawable, Command = imageButton.Command, CommandParameter = imageButton.CommandParameter };
                Control.SetOnTouchListener(_onTouchListener);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var imageButton = sender as CustomControls.ImageButton;
            var context = Android.App.Application.Context;

            if (e.PropertyName == nameof(imageButton.IsEnabled))
            {
                if (!Control.Enabled)
                    Control.SetImageDrawable(_disabledDrawable);
                else if (Control.Pressed)
                    Control.SetImageDrawable(_pressedDrawable);
                else
                    Control.SetImageDrawable(_defaultDrawable);
            }

            else if (e.PropertyName == nameof(imageButton.Command))
                _onTouchListener.Command = imageButton.Command;
            else if (e.PropertyName == nameof(imageButton.CommandParameter))
                _onTouchListener.CommandParameter = imageButton.CommandParameter;

            else if (e.PropertyName == nameof(imageButton.DefaultImage))
            {
                _defaultDrawable = context.Resources.GetDrawable(imageButton.DefaultImage);
                _onTouchListener.DefaultDrawable = _defaultDrawable;
                if (Control.Enabled && !Control.Pressed)
                    Control.SetImageDrawable(_defaultDrawable);
            }
            else if (e.PropertyName == nameof(imageButton.PressedImage))
            {
                _pressedDrawable = context.Resources.GetDrawable(imageButton.PressedImage);
                _onTouchListener.PressedDrawable = _pressedDrawable;
                if (Control.Enabled && Control.Pressed)
                    Control.SetImageDrawable(_pressedDrawable);
            }
            else if (e.PropertyName == nameof(imageButton.DisabledImage))
            {
                _disabledDrawable = context.Resources.GetDrawable(imageButton.DisabledImage);
                _onTouchListener.DisabledDrawable = _disabledDrawable;
                if (!Control.Enabled)
                    Control.SetImageDrawable(_disabledDrawable);
            }
        }
    }

    public class OnTouchListener : Java.Lang.Object, IOnTouchListener
    {
        public Drawable DefaultDrawable { get; set; }
        public Drawable PressedDrawable { get; set; }
        public Drawable DisabledDrawable { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }

        public bool OnTouch(Android.Views.View v, MotionEvent e)
        {
            var control = v as ImageView;
            if (e.Action == MotionEventActions.Down && control.Enabled)
                control.SetImageDrawable(PressedDrawable);
            else if (e.Action == MotionEventActions.Up && control.Enabled)
                Command.Execute(CommandParameter);

            return true;
        }
    }
}