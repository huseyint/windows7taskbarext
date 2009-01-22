namespace Huseyint.Windows7.Demo
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using Huseyint.Windows7.WindowsForms;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void SetOverlayIconButton_Click(object sender, System.EventArgs e)
        {
            var imageName = string.Format(
                "Huseyint.Windows7.Demo.Images.{0}.png",
                overlayIconsCombo.Text);

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
            TaskBarExtensions.SetProgressState((ProgressState)progressStatesCombo.SelectedValue);
        }

        private void SimulateProgressStatesButton_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                // Simulate estimated calculation of a long operation
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
    }
}