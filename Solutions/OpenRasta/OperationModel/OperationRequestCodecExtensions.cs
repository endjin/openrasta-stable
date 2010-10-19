namespace OpenRasta.OperationModel
{
    #region Using Directives

    using OpenRasta.Codecs.Framework;
    using OpenRasta.Contracts.OperationModel;

    #endregion

    public static class OperationRequestCodecExtensions
    {
        private const string RequestCodec = "_REQUEST_CODEC";

        public static CodecMatch GetRequestCodec(this IOperation operation)
        {
            return operation.ExtendedProperties[RequestCodec] as CodecMatch;
        }

        public static void SetRequestCodec(this IOperation operation, CodecMatch codecMatch)
        {
            operation.ExtendedProperties[RequestCodec] = codecMatch;
        }
    }
}