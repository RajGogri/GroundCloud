using GroundCloud.Contracts;

namespace GroundCloud.Impl
{
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