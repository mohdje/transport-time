using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using System;
using TransportTime.BroadcastReceivers;
using XamarinCustomHelper.Activities;
using XamarinCustomHelper.Javascript;

namespace TransportTime.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : WebViewActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var permissions = new string[]
            {
                Manifest.Permission.ReceiveBootCompleted
            };
            this.RequestPermissions(permissions, 1234);
        }
        protected override JavascriptWebViewInterface[] GetJavascriptInterfaces()
        {
            return new JavascriptWebViewInterface[]
            {
               new JSIMainActivity(this),
               new JavascriptPhoneInterface(this)
            };
        }
        
        protected override int GetLayoutResource()
        {
            return Resource.Layout.activity_webview;
        }

        protected override string GetUrl()
        {
            return "file:///android_asset/UI/mainActivity/index.html";
        }

        protected override int GetWebViewId()
        {
            return Resource.Id.webView;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}