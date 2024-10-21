﻿using Anjo.Android.AgoraIO.AgoraDynamicKey.Media;

namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Rtm
{
    public class RtmTokenBuilder
    {
        public enum Role
        {
            RoleRtmUser = 1,
        }

        // buildToken
        // appID: The App ID issued to you by Agora. Apply for a new App ID from
        //        Agora Dashboard if it is missing from your kit. See Get an App ID.
        // appCertificate:	Certificate of the application that you registered in
        //                  the Agora Dashboard. See Get an App Certificate.
        // userAccount: The user account.
        // privilegeExpireTs: represented by the number of seconds elapsed since
        //                    1/1/1970. If, for example, you want to access the
        //                    Agora Service within 10 minutes after the token is
        //                    generated, set expireTimestamp as the current
        //                    timestamp + 600 (seconds)./
        public static string BuildToken(string appId, string appCertificate, string userAccount, uint privilegeExpiredTs)
        {
            AccessToken accessToken = new AccessToken(appId, appCertificate, userAccount, "");
            accessToken.AddPrivilege(Privileges.KRtmLogin, privilegeExpiredTs);

            return accessToken.Build();
        }
    }
}
