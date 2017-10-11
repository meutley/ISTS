namespace ISTS.Domain.Schedules
{
    public enum SessionScheduleValidatorResult
    {
        Success,
        Overlapping,
        StartGreaterThanOrEqualToEnd
    }
}