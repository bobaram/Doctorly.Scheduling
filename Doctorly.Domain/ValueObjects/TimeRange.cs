namespace Doctorly.Domain.ValueObjects;

public record TimeRange
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }

    public TimeRange(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException("Start time must be before end time.");

        Start = start;
        End = end;
    }

    public bool Overlaps(TimeRange other) => Start < other.End && other.Start < End;
}
