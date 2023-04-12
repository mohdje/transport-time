using System;
using Android.App;
using Android.Content;
using Android.OS;
using System.Threading.Tasks;
using TransportTime.Activities;
using System.IO;
using TransportTime.Business;

namespace TransportTime
{
    [Activity(Theme = "@style/AppTheme.Splash", MainLauncher = true, NoHistory = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
          
            Task startupWork = new Task(() =>
            {
                Business.PrimService.Init();      
            });

            startupWork.ContinueWith(t =>
            {
                XamarinCustomHelper.Activities.ActivitiesTransitionManager.SwitchToActivity(
                    this, 
                    typeof(MainActivity), 
                    true, 
                    null, 
                    Resource.Animation.abc_fade_in, 
                    Resource.Animation.abc_fade_out);
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }
    }
}