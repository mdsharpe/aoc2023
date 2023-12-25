record Component(string Key) {
    public HashSet<Component> Connections { get; init; } = new();
}
