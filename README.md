# Anjo.Android.AgoraIO.AgoraDynamicKey

---------------------------------

This section delves into the authentication mechanism implemented by the Agora SDK. Equally important, this also provides the requisite code for generating access tokens - Wael Anjo


[![NuGet](https://buildstats.info/nuget/Anjo.Android.AgoraIO.AgoraDynamicKey)](https://www.nuget.org/packages/Anjo.Android.AgoraIO.AgoraDynamicKey)
[![License](https://img.shields.io/github/license/mhdwaelanjo/Anjo.Android.AgoraIO.AgoraDynamicKey)](https://github.com/mhdwaelanjo/Anjo.Android.AgoraIO.AgoraDynamicKey/blob/master/LICENSE)

This page describes the authentication mechanism used by the Agora SDK, as well as providing the related code for generating AccessToken (v2.1.0) or Dynamic Key (v2.0.2 or earlier).
https://github.com/AgoraIO/Tools/tree/master/DynamicKey/AgoraDynamicKey

---------------------------------
### How To Use


## AccessToken

AccessToken is more powerful than the legacy Dynamic Key. It encapsulates several privileges in one token to cover various services provided by Agora.

AccessToken is available as of SDK 2.1.0.

Sample usage,

* For Rtm Token
```c#
private string _appId = "970CA35de60c44645bbae8a215061b33";
private string _appCertificate = "5CFd2fd1755d40ecb72977518be15d3b";
private string _account = "2882341273";
private uint _expireTimeInSeconds = 3600;

uint privilegeExpiredTs = _expireTimeInSeconds + (uint)Utils.getTimestamp();
string token = RtmTokenBuilder.buildToken(_appId, _appCertificate, _account, privilegeExpiredTs);
Console.WriteLine(">> token" + token);
```

* For Rtc Token
```c#
private string _appId = "970CA35de60c44645bbae8a215061b33";
private string _appCertificate = "5CFd2fd1755d40ecb72977518be15d3b";
private string _channelName = "7d72365eb983485397e3e3f9d460bdda";
private string _account = "2882341273";
private uint _uid = 2882341273;
private uint _expireTimeInSeconds = 3600;

uint privilegeExpiredTs = _expireTimeInSeconds + (uint)Utils.getTimestamp();
string token = RtcTokenBuilder.buildTokenWithUserAccount(_appId, _appCertificate, _channelName, _account, RtcTokenBuilder.Role.RolePublisher, privilegeExpiredTs);
Console.WriteLine(">> token" + token);
```

Star on Github if this project helps you: https://github.com/mhdwaelanjo/Anjo.Android.AgoraIO.AgoraDynamicKey

---------------------------------

### Agora Full SDK
You can fine Agora.io Rtc Full SDK for .NET for Android Here : 
> [![NuGet Version](https://img.shields.io/nuget/v/Anjo.Android.AgoraFull)](https://www.nuget.org/packages/Anjo.Android.AgoraFull)

### Agora Lite SDK
You can fine Agora.io Rtc Lite SDK for .NET for Android Here : 
> [![NuGet Version](https://img.shields.io/nuget/v/Anjo.Android.AgoraLite)](https://www.nuget.org/packages/Anjo.Android.AgoraLite)
 
---------------------------------
### Help & Feedback:
- You can subscribe to the channel on the telegram [Anjo Help & Feedback](https://t.me/mhwaelanjo) to learn about the latest updates to my packages on [Nuget.com](https://www.nuget.org/profiles/MHWAELANJO)

### SUPPORT:
- ☕ Buy me a coffee: [By PayPal](https://www.paypal.com/paypalme/mhwaelanjo)