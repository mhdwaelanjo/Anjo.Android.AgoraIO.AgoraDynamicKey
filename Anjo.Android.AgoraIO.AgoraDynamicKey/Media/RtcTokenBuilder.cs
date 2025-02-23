namespace Anjo.Android.AgoraIO.AgoraDynamicKey.Media
{
    public class RtcTokenBuilder
    {
        // Role
        public enum Role
        {
            RoleAttendee = 0,
            RolePublisher = 1,
            RoleSubscriber = 2,
            RoleAdmin = 101
        }

        // buildTokenWithUid
        // appID: The App ID issued to you by Agora. Apply for a new App ID from
        //        Agora Dashboard if it is missing from your kit. See Get an App ID.
        // appCertificate:	Certificate of the application that you registered in
        //                  the Agora Dashboard. See Get an App Certificate.
        // channelName:Unique channel name for the AgoraRTC session in the string format
        // uid: User ID. A 32-bit unsigned integer with a value ranging from
        //      1 to (2^32-1). uid must be unique.
        // role: Role_Publisher = 1: A broadcaster (host) in a live-broadcast profile.
        //       Role_Subscriber = 2: (Default) A audience in a live-broadcast profile.
        // privilegeExpireTs: represented by the number of seconds elapsed since
        //                    1/1/1970. If, for example, you want to access the
        //                    Agora Service within 10 minutes after the token is
        //                    generated, set expireTimestamp as the current
        //                    timestamp + 600 (seconds)./
        public static string BuildTokenWithUid(string appId, string appCertificate, string channelName, uint uid, Role role, uint privilegeExpiredTs)
        {
            string uidStr = uid.ToString();
            if (uid == 0)
            {
                uidStr = "";
            }

            return BuildTokenWithUserAccount(appId, appCertificate, channelName, uidStr, role, privilegeExpiredTs);
        }

        // buildTokenWithUserAccount
        // appID: The App ID issued to you by Agora. Apply for a new App ID from
        //        Agora Dashboard if it is missing from your kit. See Get an App ID.
        // appCertificate:	Certificate of the application that you registered in
        //                  the Agora Dashboard. See Get an App Certificate.
        // channelName:Unique channel name for the AgoraRTC session in the string format
        // userAccount: The user account.
        // role: Role_Publisher = 1: A broadcaster (host) in a live-broadcast profile.
        //       Role_Subscriber = 2: (Default) A audience in a live-broadcast profile.
        // privilegeExpireTs: represented by the number of seconds elapsed since
        //                    1/1/1970. If, for example, you want to access the
        //                    Agora Service within 10 minutes after the token is
        //                    generated, set expireTimestamp as the current
        //                    timestamp + 600 (seconds)./
        public static string BuildTokenWithUserAccount(string appId, string appCertificate, string channelName, string userAccount, Role role, uint privilegeExpiredTs)
        {
            AccessToken accessToken = new AccessToken(appId, appCertificate, channelName, userAccount);
            accessToken.AddPrivilege(Privileges.KJoinChannel, privilegeExpiredTs);

            if (role == Role.RoleAttendee || role == Role.RolePublisher || role == Role.RoleAdmin)
            {
                accessToken.AddPrivilege(Privileges.KPublishAudioStream, privilegeExpiredTs);
                accessToken.AddPrivilege(Privileges.KPublishVideoStream, privilegeExpiredTs);
                accessToken.AddPrivilege(Privileges.KPublishDataStream, privilegeExpiredTs);
            }

            return accessToken.Build();
        }
    }
}