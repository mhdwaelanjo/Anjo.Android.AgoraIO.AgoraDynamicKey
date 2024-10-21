using System.Security.Cryptography;
using Anjo.Android.AgoraIO.AgoraDynamicKey.Extensions;
using Anjo.Android.AgoraIO.AgoraDynamicKey.Media;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.TokenBuilders
{
    public class ApaasTokenBuilder
    {
        /**
         * build user room token
         *
         * @param appId          The App ID issued to you by Agora. Apply for a new App ID from
         *                       Agora Dashboard if it is missing from your kit. See Get an App ID.
         * @param appCertificate Certificate of the application that you registered in
         *                       the Agora Dashboard. See Get an App Certificate.
         * @param roomUuid       The room's id, must be unique.
         * @param userUuid       The user's id, must be unique.
         * @param role           The user's role.
         * @param expire         represented by the number of seconds elapsed since now. If, for example, you want to access the
         *                       Agora Service within 10 minutes after the token is generated, set expire as 600(seconds).
         * @return The user room token.
         */
        public static string BuildRoomUserToken(string appId, string appCertificate, string roomUuid, string userUuid, short role, uint expire)
        {
            AccessToken2 accessToken = new AccessToken2(appId, appCertificate, expire);

            var md5 = MD5.Create();
            string chatUserId = md5.ComputeHash(userUuid.GetBytes()).ToString();

            AccessToken2.Service serviceApaas = new AccessToken2.ServiceApaas(roomUuid, userUuid, role);
            serviceApaas.AddPrivilegeApaas(AccessToken2.PrivilegeApaasEnum.PrivilegeRoomUser, expire);
            accessToken.AddService(serviceApaas);

            AccessToken2.Service serviceRtm = new AccessToken2.ServiceRtm(userUuid);
            serviceRtm.AddPrivilegeRtm(AccessToken2.PrivilegeRtmEnum.PrivilegeLogin, expire);
            accessToken.AddService(serviceRtm);

            AccessToken2.Service serviceChat = new AccessToken2.ServiceChat(chatUserId);
            serviceChat.AddPrivilegeChat(AccessToken2.PrivilegeChatEnum.PrivilegeChatUser, expire);
            accessToken.AddService(serviceChat);

            return accessToken.Build();
        }

        /**
         * build user token
         *
         * @param appId          The App ID issued to you by Agora. Apply for a new App ID from
         *                       Agora Dashboard if it is missing from your kit. See Get an App ID.
         * @param appCertificate Certificate of the application that you registered in
         *                       the Agora Dashboard. See Get an App Certificate.
         * @param userUuid       The user's id, must be unique.
         * @param expire         represented by the number of seconds elapsed since now. If, for example, you want to access the
         *                       Agora Service within 10 minutes after the token is generated, set expire as 600(seconds).
         * @return The user token.
         */
        public static string BuildUserToken(string appId, string appCertificate, string userUuid, uint expire)
        {
            AccessToken2 accessToken = new AccessToken2(appId, appCertificate, expire);
            AccessToken2.Service service = new AccessToken2.ServiceApaas(userUuid);

            service.AddPrivilegeApaas(AccessToken2.PrivilegeApaasEnum.PrivilegeUser, expire);
            accessToken.AddService(service);

            return accessToken.Build();
        }

        /**
         * build app token
         *
         * @param appId          The App ID issued to you by Agora. Apply for a new App ID from
         *                       Agora Dashboard if it is missing from your kit. See Get an App ID.
         * @param appCertificate Certificate of the application that you registered in
         *                       the Agora Dashboard. See Get an App Certificate.
         * @param expire         represented by the number of seconds elapsed since now. If, for example, you want to access the
         *                       Agora Service within 10 minutes after the token is generated, set expire as 600(seconds).
         * @return The app token.
         */
        public static string BuildAppToken(string appId, string appCertificate, uint expire)
        {
            AccessToken2 accessToken = new AccessToken2(appId, appCertificate, expire);
            AccessToken2.Service serviceApaas = new AccessToken2.ServiceApaas();

            serviceApaas.AddPrivilegeApaas(AccessToken2.PrivilegeApaasEnum.PrivilegeApp, expire);
            accessToken.AddService(serviceApaas);

            return accessToken.Build();
        }
    }
}
