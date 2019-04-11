using GroundCloud.Contracts;

namespace GroundCloud.Contracts
{
    public abstract class AbstractGroundCloudFactory
    {
       protected abstract ICloud CreateCloud();

       protected abstract IGround CreateGround();
    }
}
