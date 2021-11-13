namespace DamnEngine
{
    public interface IBounding
    {
        bool IsOnForwardPlan(Plane plane);

        bool IsOnFrustum(Frustum frustum, ITransformer transformer);
    }
}