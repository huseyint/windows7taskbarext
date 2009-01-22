namespace Huseyint.Windows7
{
    public enum ProgressState
    {
        /// <summary>
        /// Stops displaying progress and returns the button to its 
        /// normal state. Call this method with this flag to dismiss 
        /// the progress bar when the operation is complete or cancelled.
        /// </summary>
        NoProgress = 0,

        /// <summary>
        /// The progress indicator does not grow in size, but cycles 
        /// repeatedly along the length of the taskbar button. This 
        /// indicates activity without specifying what proportion of 
        /// the progress is complete. Progress is taking place, but 
        /// there is no prediction as to how long the operation will take.
        /// </summary>
        Indeterminate = 1,

        /// <summary>
        /// The progress indicator grows in size from left to right 
        /// in proportion to the estimated amount of the operation 
        /// completed. This is a determinate progress indicator; a 
        /// prediction is being made as to the duration of the operation.
        /// </summary>
        Normal = 2,

        /// <summary>
        /// The progress indicator turns red to show that an error 
        /// has occurred in one of the windows that is broadcasting 
        /// progress. This is a determinate state. If the progress 
        /// indicator is in the indeterminate state, it switches to a 
        /// red determinate display of a generic percentage not 
        /// indicative of actual progress.
        /// </summary>
        Error = 4,

        /// <summary>
        /// The progress indicator turns yellow to show that progress 
        /// is currently stopped in one of the windows but can be 
        /// resumed by the user. No error condition exists and nothing 
        /// is preventing the progress from continuing. This is a 
        /// determinate state. If the progress indicator is in the 
        /// indeterminate state, it switches to a yellow determinate 
        /// display of a generic percentage not indicative of actual progress.
        /// </summary>
        Paused = 8,
    }
}