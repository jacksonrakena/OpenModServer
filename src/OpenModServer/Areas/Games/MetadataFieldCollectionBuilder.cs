namespace OpenModServer.Areas.Games;

public class MetadataFieldBuilder
{
    public MetadataFieldType FieldType { get; set; }
    public string Name { get; set; }
    public string? HelperText { get; set; }
    public string? Placeholder { get; set; }
    
    
    public MetadataFieldBuilder WithName(string name)
    {
        Name = name;
        return this;
    }
    
    public MetadataFieldBuilder WithHelperText(string helperText)
    {
        HelperText = helperText;
        return this;
    }

    public MetadataFieldBuilder WithPlaceholder(string placeholder)
    {
        Placeholder = placeholder;
        return this;
    }
}
public class MetadataFieldCollectionBuilder
{
    public Dictionary<string, MetadataFieldBuilder> Fields { get; } = new Dictionary<string, MetadataFieldBuilder>();
    public MetadataFieldCollectionBuilder AddField(string id, MetadataFieldType fieldType, Action<MetadataFieldBuilder> builder)
    {
        var b = new MetadataFieldBuilder();
        b.FieldType = fieldType;
        builder(b);
        Fields[id] = b;
        return this;
    }
}