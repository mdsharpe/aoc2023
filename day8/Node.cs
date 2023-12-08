class Node {
    public Node(string key)
    {
        Key = key;
        IsGhostStart = key.EndsWith('A');
        IsGhostEnd = key.EndsWith('Z');
    }
    
    public string Key { get; }
    public bool IsGhostStart { get; }
    public bool IsGhostEnd { get; }
    
    public Node? Left { get; set; }
    public Node? Right { get; set; }
}
