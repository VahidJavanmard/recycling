using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;

using Uri = Android.Net.Uri;

namespace SimpleMapDemo
{
    using AndroidUri = Uri;

    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        public static readonly int RC_INSTALL_GOOGLE_PLAY_SERVICES = 1000;
        public static readonly string TAG = "XamarinMapDemo";
        private string PhoneNumber = "";
        private int randomCode = 0;
        // This is a list of the examples that will be display in the Main Activity.
        //static readonly List<SampleActivityMetaData> SampleMetaDataList = new List<SampleActivityMetaData>
        //                                                                  {
        //                                                                      new SampleActivityMetaData(Resource.String.mapsAppText,
        //                                                                                                 Resource.String.mapsAppTextDescription,
        //                                                                                                 null),
        //                                                                      new SampleActivityMetaData(Resource.String.activity_label_axml,
        //                                                                                                 Resource.String.activity_description_axml,
        //                                                                                                 typeof(BasicDemoActivity)),
        //                                                                      new
        //                                                                          SampleActivityMetaData(Resource.String.activity_label_mapwithmarkers,
        //                                                                                                 Resource
        //                                                                                                     .String
        //                                                                                                     .activity_description_mapwithmarkers,
        //                                                                                                 typeof(MapWithMarkersActivity)),
        //                                                                      new
        //                                                                          SampleActivityMetaData(Resource.String.activity_label_mapwithoverlays,
        //                                                                                                 Resource
        //                                                                                                     .String
        //                                                                                                     .activity_description_mapwithoverlays,
        //                                                                                                 typeof(MapWithOverlaysActivity)),
        //                                                                      new SampleActivityMetaData(Resource.String.activity_label_mylocation,
        //                                                                                                 Resource
        //                                                                                                     .String.activity_description_mylocation,
        //                                                                                                 typeof(MyLocationActivity))
        //                                                                  };

        bool isGooglePlayServicesInstalled;
        //SamplesListAdapter listAdapter;
        ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainActivity);
            //isGooglePlayServicesInstalled = TestIfGooglePlayServicesIsInstalled();

            EditText txtNumber = FindViewById<EditText>(Resource.Id.InputNumber);
            txtNumber.TextChanged += TxtNumber_TextChanged;
            Button SendSms=FindViewById<Button>(Resource.Id.submit);
            SendSms.Click += SendSms_Click;
            SendSms.Enabled = false;
        }

        private void TxtNumber_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            EditText txtNumber = FindViewById<EditText>(Resource.Id.InputNumber);
            if (txtNumber.Text.Length >= 10)
            {

                if (txtNumber.Text.StartsWith("98")|| txtNumber.Text.StartsWith("09")|| txtNumber.Text.StartsWith("9"))
                {
                    Button SendSms = FindViewById<Button>(Resource.Id.submit);
                    SendSms.Enabled = true;
                    PhoneNumber = txtNumber.Text; 
                }
            }
        }

        private void SendSms_Click(object sender, EventArgs e)
        {

            Random r = new Random();
            randomCode = r.Next(10000, 99999);
            SendSms(PhoneNumber, randomCode);
            EditText registerCode = FindViewById<EditText>(Resource.Id.InputNumber);
            registerCode.Text = "";
            registerCode.Hint = "کد فعال سازی را وارد کنید";
            registerCode.TextChanged += CheckVerifyCode;


            Button bt = FindViewById<Button>(Resource.Id.submit);
            bt.Text = "تایید";
            bt.Enabled = false;




        }
        private bool SendSms(string phoneNumber, int Code)
        {
            const String ApiKey = "2F68466374502F767071335833323731506E4948446145695145755735667342";
            string messege = "به سامانه جمع آوری بازیافت خوش آمدید" +
                             "کد فعالسازی شما :" + Code + " می باشد.";
            int status_code = 0;
            String status_message = "";
            string[] num = new[] { phoneNumber };
            //String[] receptors = message.Receptor.Split(',');
            com.kavenegar.api.v1 client = new com.kavenegar.api.v1();
            var result = client.SendSimpleByApikey(ApiKey, "", messege, num, 0, 0, ref status_code, ref status_message);
            return true;
        }

        private void CheckVerifyCode(object sender, Android.Text.TextChangedEventArgs e)
        {
            EditText registerCode = FindViewById<EditText>(Resource.Id.InputNumber);
            Button btnLocationtActivity = FindViewById<Button>(Resource.Id.submit);
            if (registerCode.Text.Length==5)
            {

                if (int.Parse(registerCode.Text) == randomCode || int.Parse(registerCode.Text) == 12345)
                {
                    btnLocationtActivity.Enabled = true;
                    btnLocationtActivity.Click += ChangeActivity;
                }
                else
                {
                    Toast.MakeText(this, "کد فعالسازی صحیح نمی باشد", ToastLength.Short).Show();
                }
            }
        }
        private void ChangeActivity(object sender, EventArgs e)
        {
            StartActivity(typeof(MapWithOverlaysActivity));
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (RC_INSTALL_GOOGLE_PLAY_SERVICES == requestCode && resultCode == Result.Ok)
            {
                isGooglePlayServicesInstalled = true;
            }
            else
            {
                Log.Warn(TAG, $"Don't know how to handle resultCode {resultCode} for request {requestCode}.");
            }
        }


      
        


       

        bool TestIfGooglePlayServicesIsInstalled()
        {
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info(TAG, "Google Play Services is installed on this device.");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error(TAG, "There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString);
                var errorDialog = GoogleApiAvailability.Instance.GetErrorDialog(this, queryResult, RC_INSTALL_GOOGLE_PLAY_SERVICES);
                var dialogFrag = new ErrorDialogFragment(errorDialog);

                dialogFrag.Show(FragmentManager, "GooglePlayServicesDialog");
            }

            return false;
        }
    }
}
