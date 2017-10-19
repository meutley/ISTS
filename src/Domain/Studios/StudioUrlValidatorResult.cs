using System;

namespace ISTS.Domain.Studios
{
    public enum StudioUrlValidatorResult
    {
        Success,
        UrlContainsInvalidCharacters,
        UrlAlreadyInUse
    }
}