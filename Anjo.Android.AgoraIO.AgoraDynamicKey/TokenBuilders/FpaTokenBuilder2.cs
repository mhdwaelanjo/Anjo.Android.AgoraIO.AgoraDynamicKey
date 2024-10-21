using Anjo.Android.AgoraIO.AgoraDynamicKey.Media;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.TokenBuilders
{
    public class FpaTokenBuilder2
    {
        /**
         * Build the FPA token.
         *
         * @param appId:          The App ID issued to you by Agora. Apply for a new App ID from
         *                        Agora Dashboard if it is missing from your kit. See Get an App ID.
         * @param appCertificate: Certificate of the application that you registered in
         *                        the Agora Dashboard. See Get an App Certificate.
         * @return The FPA token.
         */
        public static string BuildToken(string appId, string appCertificate)
        {
            AccessToken2 accessToken = new AccessToken2(appId, appCertificate, 24 * 3600);
            AccessToken2.Service serviceFpa = new AccessToken2.ServiceFpa();

            serviceFpa.AddPrivilegeFpa(AccessToken2.PrivilegeFpaEnum.PrivilegeLogin, 0);
            accessToken.AddService(serviceFpa);

            return accessToken.Build();
        }
    }
}
