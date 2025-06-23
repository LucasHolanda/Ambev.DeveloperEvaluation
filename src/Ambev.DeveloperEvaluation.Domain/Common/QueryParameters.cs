namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public class QueryParameters
    {
        public int _page { get; set; } = 1;
        public int _size { get; set; } = 10;

        public string? _order { get; set; }

        public Dictionary<string, string> Filters { get; set; } = new();
    }
}
