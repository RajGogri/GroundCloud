using GroundCloud.Contracts;

namespace GroundCloud.Impl
{
    public abstract class AbstractGroundCloudFactory
    {
       protected abstract ICloud CreateCloud();

       protected abstract IGround CreateGround();
    }

    public class GroundCloudFactory : AbstractGroundCloudFactory
    {
        protected override ICloud CreateCloud()
        {
            return new HttpClientCloud();
        }

        protected override IGround CreateGround()
        {
            return new LitedbGround();
        }
    }
}
