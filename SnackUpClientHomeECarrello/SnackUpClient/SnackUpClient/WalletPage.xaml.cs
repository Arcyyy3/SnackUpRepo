using System.Diagnostics;

namespace SnackUpClient
{
    public partial class WalletPage : ContentPage
    {
        public WalletPage()
        {
            InitializeComponent();
        }

        private async void OnFlipButtonClicked(object sender, EventArgs e)
        {
            if (FrontFrame.IsVisible)
            {
                await Task.WhenAll(
                    FrontFrame.ScaleTo(0.55, 250),
                    FrontFrame.RotateYTo(90, 250)
                );
                FrontFrame.IsVisible = false;

                BackFrame.IsVisible = true;
                BackFrame.RotationY = -90;
                await Task.WhenAll(
                    BackFrame.ScaleTo(1.0, 250),
                    BackFrame.RotateYTo(0, 250)
                );
            }
            else
            {
                await Task.WhenAll(
                    BackFrame.ScaleTo(0.55, 250),
                    BackFrame.RotateYTo(90, 250)
                );
                BackFrame.IsVisible = false;

                FrontFrame.IsVisible = true;
                FrontFrame.RotationY = -90;
                await Task.WhenAll(
                    FrontFrame.ScaleTo(1.0, 250),
                    FrontFrame.RotateYTo(0, 250)
                );
            }
        }
    }
}
