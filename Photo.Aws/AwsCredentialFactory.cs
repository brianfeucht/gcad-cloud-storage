using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
namespace Photo.Aws
{
    public class AwsCredentialFactory
    {
        private static AWSCredentials cachedCredentials = null;

        public AWSCredentials Credentials()
        {
            if(cachedCredentials == null)
            {
                // TODO: Pull this out of code and configure credential via PS script or IAM.
                //Amazon.Util.ProfileManager.RegisterProfile("gcad-demo", "", "");
                cachedCredentials = new StoredProfileAWSCredentials("gcad-demo");
            }

            return cachedCredentials;
        }
    }
}
