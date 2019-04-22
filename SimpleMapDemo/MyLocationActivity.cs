using Android.App;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;

using System.Collections.Generic;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V4.Widget;
using Android.Gms.Maps.Model;

namespace SimpleMapDemo
{
    [Activity(Label = "@string/activity_label_mylocation")]
    public class MyLocationActivity : AppCompatActivity, IOnMapReadyCallback
    {
        static readonly string TAG = "MyLocationActivity";

        static readonly int REQUEST_PERMISSIONS_LOCATION = 1000;

        private Toolbar myToolbar;
        private ListView MyListView;
        private List<string> List;
        private string PhoneNumber = "";
        private ManageDrawer manageDrawer;
        private DrawerLayout MyDrawer;
        private int Code = 0;

        static readonly LatLng PasschendaeleLatLng = new LatLng(50.897778, 3.013333);

        GoogleMap theMap;
        public void OnMapReady(GoogleMap googleMap)

        {
            theMap = googleMap;
            if (this.PerformRuntimePermissionCheckForLocation(REQUEST_PERMISSIONS_LOCATION))
            {
                InitializeUiSettingsOnMap();
            }
        }

        void InitializeUiSettingsOnMap()
        {
            theMap.UiSettings.MyLocationButtonEnabled = true;
            theMap.UiSettings.CompassEnabled = true;
            theMap.UiSettings.ZoomControlsEnabled = true;
            theMap.MyLocationEnabled = true;
        
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyLocationLayout);


        }
              private void MyListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, List[e.Position], ToastLength.Short).Show();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == REQUEST_PERMISSIONS_LOCATION)
            {
                if (grantResults.AllPermissionsGranted())
                {
                    // Permissions granted, nothing to do.
                    // Carry on and let the MapFragment do it's own thing.
                    InitializeUiSettingsOnMap();
                }
                else
                {
                    // Permissions not granted!
                    Log.Info(TAG, "The app does not have location permissions");

                    var layout = FindViewById(Android.Resource.Id.Content);
                    Snackbar.Make(layout, Resource.String.location_permission_missing, Snackbar.LengthLong).Show();
                    Finish();
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

    }
}
