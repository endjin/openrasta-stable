namespace OpenRasta.Pipeline
{
    public enum PipelineContinuation
    {
        /// <summary>
        /// An error occured and the pipeline should abort.
        /// </summary>
        /// <remarks>Aborting the pipeline will result in the error list 
        /// being sent back to the client</remarks>
        Abort,

        /// <summary>
        /// Processing of the pipeline is complete.
        /// </summary>
        Finished,

        /// <summary>
        /// Continue processing the pipeline.
        /// </summary>
        Continue,
        
        RenderNow
    }
}