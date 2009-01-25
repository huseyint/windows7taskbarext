namespace Huseyint.Windows7.WindowsForms.Demo
{
    using System;
    using System.Windows.Forms;
    using Huseyint.Windows7.WindowsForms;

    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            TaskBarExtensions.Attach();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}