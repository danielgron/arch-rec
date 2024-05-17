public struct Edge
{
    public int Source { get; }
    public int Target { get; }
    public int Weight { get; }

    public Edge(int source, int target, int weight)
    {
        Source = source;
        Target = target;
        Weight = weight;
    }
}