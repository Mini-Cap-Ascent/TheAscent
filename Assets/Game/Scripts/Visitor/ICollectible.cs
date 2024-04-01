// The interface for your collectibles
public interface ICollectible
{
    void Accept(IPlayerVisitor visitor);
}
