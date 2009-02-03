namespace Huseyint.Windows7.WindowsForms.Demo
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using Huseyint.Windows7.WindowsForms;

    public partial class MainForm : Form
    {
        private IList<ThumbnailBarButton> thumbnailBarButtons;

        public MainForm()
        {
            this.InitializeComponent();
        }

        private void SetOverlayIconButton_Click(object sender, System.EventArgs e)
        {
            var imageName = string.Format(
                "Huseyint.Windows7.WindowsForms.Demo.Images.{0}.png",
                this.overlayIconsCombo.Text);

            using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream(imageName))
            using (var image = (Bitmap)Image.FromStream(stream))
            {
                TaskBarExtensions.SetOverlayIcon(image);
            }
        }

        private void ClearOverlayIconButton_Click(object sender, System.EventArgs e)
        {
            TaskBarExtensions.ClearOverlayIcon();
        }

        private void SetProgressStateButton_Click(object sender, System.EventArgs e)
        {
            TaskBarExtensions.SetProgressState((ProgressState)this.progressStatesCombo.SelectedValue);
        }

        private void SimulateProgressStatesButton_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                // Simulate estimated calculation of a long operation
                TaskBarExtensions.SetProgressState(ProgressState.NoProgress);
                TaskBarExtensions.SetProgressState(ProgressState.Indeterminate);
                
                Thread.Sleep(2000);

                for (ulong i = 1, operationCount = 100; i <= operationCount; i++)
                {
                    // SetProgressValue inherently set the progress state to ProgressState.Normal
                    TaskBarExtensions.SetProgressValue(i, operationCount);

                    if (i == 42)
                    {
                        // Set progress state to Paused (Yellow)
                        TaskBarExtensions.SetProgressState(ProgressState.Paused);

                        // Display a fake warning message
                        var result = MessageBox.Show(
                            "You are warned, do you want to continue?",
                            "Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.Yes)
                        {
                            // Set state back to Normal (Green)
                            TaskBarExtensions.SetProgressState(ProgressState.Normal);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (i == 65)
                    {
                        // Set progress state to Error (Red)
                        TaskBarExtensions.SetProgressState(ProgressState.Error);

                        // Display a fake error message
                        var result = MessageBox.Show(
                            "A critical error occured, do you want to continue?",
                            "Error Occured",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.Yes)
                        {
                            // Set state back to Normal (Green)
                            TaskBarExtensions.SetProgressState(ProgressState.Normal);
                        }
                        else
                        {
                            break;
                        }
                    }

                    Thread.Sleep(100);
                }

                MessageBox.Show("Operation completed!", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                TaskBarExtensions.SetProgressState(ProgressState.NoProgress);
            }));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.overlayIconsCombo.SelectedIndex = 0;

            this.progressStatesCombo.DataSource = Enum.GetValues(typeof(ProgressState));
        }

        private void AddThumbBarButtonsImageList_Click(object sender, EventArgs e)
        {
            this.thumbnailBarButtons = new List<ThumbnailBarButton>()
            {
                new ThumbnailBarButton(0, "Foo", true, false, false, true),
                new ThumbnailBarButton(1, "Bar", false, false, false, true),
                new ThumbnailBarButton(2, "Baz", false, false, false, false),
                new ThumbnailBarButton(3, "Quux", true, true, false, true),
            };

            foreach (var button in this.thumbnailBarButtons)
            {
                button.Click += this.ThumbnailBarButton_Click;
            }

            TaskBarExtensions.AddThumbnailBarButtons(this, this.thumbnailBarButtons, this.iconsImageList);

            this.addThumbBarButtonsImageList.Enabled = false;
            this.addThumbBarButtonsImages.Enabled = false;
        }

        private void AddThumbBarButtonsImages_Click(object sender, EventArgs e)
        {
            this.thumbnailBarButtons = new List<ThumbnailBarButton>()
            {
                this.CreateThumbnailBarImageButton("Error", "Foo", true, false, false, true),
                this.CreateThumbnailBarImageButton("Info", "Bar", false, false, false, true),
                this.CreateThumbnailBarImageButton("New", "Baz", false, false, true, false),
                this.CreateThumbnailBarImageButton("Warning", "Quux", true, true, false, true),
            };

            foreach (var button in this.thumbnailBarButtons)
            {
                button.Click += this.ThumbnailBarButton_Click;
            }

            TaskBarExtensions.AddThumbnailBarButtons(this, this.thumbnailBarButtons);

            this.addThumbBarButtonsImageList.Enabled = false;
            this.addThumbBarButtonsImages.Enabled = false;
        }

        private ThumbnailBarButton CreateThumbnailBarImageButton(
            string imageName, 
            string tooltip, 
            bool isHidden, 
            bool isDisabled, 
            bool isDismissedOnClick, 
            bool hasBackground)
        {
            var imagePath = string.Format("Huseyint.Windows7.WindowsForms.Demo.Images.{0}.png", imageName);

            using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream(imagePath))
            using (var image = (Bitmap)Image.FromStream(stream))
            {
                return new ThumbnailBarButton(image, tooltip, isHidden, isDisabled, isDismissedOnClick, hasBackground);
            }
        }

        private void ThumbnailBarButton_Click(object sender, EventArgs e)
        {
            var button = (ThumbnailBarButton)sender;

            MessageBox.Show(string.Format("Button with tooltip '{0}' clicked.", button.Tooltip));
        }

        private void UpdateThumbBarButtons_Click(object sender, EventArgs e)
        {
            foreach (var button in this.thumbnailBarButtons)
            {
                button.IsDisabled = !button.IsDisabled;
                button.IsHidden = !button.IsHidden;
                button.HasBackground = !button.HasBackground;
            }
        }
    }
}