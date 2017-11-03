namespace ISTS.Domain.Sessions
{
    public enum SessionScheduleValidatorResult
    {
        Success,
        Overlapping,
        StartGreaterThanOrEqualToEnd
    }
}