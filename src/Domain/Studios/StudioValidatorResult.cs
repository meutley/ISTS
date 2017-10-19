using System;

namespace ISTS.Domain.Studios
{
    public enum StudioValidatorResult
    {
        Success,
        UrlContainsInvalidCharacters,
        UrlAlreadyInUse
    }
}