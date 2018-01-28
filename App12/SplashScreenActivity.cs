using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.SafetyNet;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System.Threading.Tasks;

namespace App12
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }      
        
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();

            
            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle("Confirm delete");
            alert.SetMessage("Lorem ipsum dolor sit amet, consectetuer adipiscing elit.");
            alert.SetPositiveButton("Delete", (senderAlert, args) => {
                Toast.MakeText(this, "Deleted!", ToastLength.Short).Show();
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
           
        }

        async void SimulateStartup()
        {
          
            
            var googleApiClient = new GoogleApiClient.Builder(this)
              .EnableAutoManage(this, 1, result => {
                //  Toast.MakeText(this, "Failed to connect to Google Play Services", ToastLength.Long).Show();
              })
              .AddApi(SafetyNetClass.API)
              .Build();

            var nonce = Nonce.Generate(); // Should be at least 16 bytes in length.
            var r = await SafetyNetClass.SafetyNetApi.AttestAsync(googleApiClient, nonce);

            // Get the JSON from the JWS result by decoding
            var decodedResult = r.DecodeJwsResult(nonce);

         

            await Task.Delay(1000);             
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

    }
}