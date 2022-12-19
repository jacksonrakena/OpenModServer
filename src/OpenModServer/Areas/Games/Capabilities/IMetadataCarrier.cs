namespace OpenModServer.Areas.Games.Capabilities;

public interface IMetadataCarrier
{
    public void BuildMetadataFields(MetadataFieldCollectionBuilder builder);
}