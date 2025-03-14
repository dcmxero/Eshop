namespace DTOs.Common;

public class DataResultDto<T>
{
    /// <summary>
    /// Gets or sets the data of type T.
    /// </summary>
    public List<T> Data { get; set; } = [];

    /// <summary>
    /// Gets or sets the total count of the data.
    /// </summary>
    public int Count { get; set; }
}