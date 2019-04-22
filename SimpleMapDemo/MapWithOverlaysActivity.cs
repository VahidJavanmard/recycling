using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
//using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace SimpleMapDemo
{
    [Activity(Label = "@string/activity_label_mapwithoverlays")]
    public class MapWithOverlaysActivity : AppCompatActivity, IOnMapReadyCallback
    {
        private static readonly LatLng InMaui = new LatLng(36.297580, 59.606015);
        private static readonly LatLng LeaveFromHereToMaui = new LatLng(58.768410, -94.164963);
        private static readonly LatLng[] LocationForCustomIconMarkers =
        {
            //new LatLng(40.741773, -74.004986),
            //new LatLng(41.051696, -73.545667),
            //new LatLng(41.311197, -72.902646)
        };
        private GoogleMap googleMap;
        private string gotMauiMarkerId;
        private MapFragment mapFragment;
        private Marker polarBearMarker;
        private GroundOverlay polarBearOverlay;

        //private readonly Toolbar myToolbar;
        private ListView MyListView;
        private List<string> List;
        private ManageDrawer manageDrawer;
        private DrawerLayout MyDrawer;
        //private readonly int Code = 0;

        public void OnMapReady(GoogleMap map)
        {
            googleMap = map;
            googleMap.MyLocationEnabled = true;

            //AddMonkeyMarkersToMap();
            //AddInitialPolarBarToMap();

            // Animate the move on the map so that it is showing the markers we added above.
            googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(InMaui, 18));

            // Setup a handler for when the user clicks on a marker.
            //googleMap.MarkerClick += MapOnMarkerClick;
        }


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MapWithOverlayLayout);

            mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            //MyListView = FindViewById<ListView>(Resource.Id.MyListView);
            //List = new List<string>()
            //{

            //    "اطلاعات کاربری",
            //    "تاریخچه",
            //    "آدرس های منتخب",
            //    "پیام ها",
            //    "پشتیبانی",
            //    "تنظیمات",
            //    "درباره ما",
            //    "خروج",


            //};
            //MyListView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, List);
            ////MyListView.ItemClick += MyListView_ItemClick;
            //MyDrawer = FindViewById<DrawerLayout>(Resource.Id.MyDrawer);
            //MyListView.Tag = 0;
            //manageDrawer = new ManageDrawer(this, MyDrawer, Resource.String.openDrawer, Resource.String.closeDrawe);

            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.MyToolBar);
            //SetSupportActionBar(toolbar); 

            //MyDrawer.SetDrawerListener(manageDrawer);
            //SupportActionBar.SetHomeButtonEnabled(true);
            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowTitleEnabled(true);
            //manageDrawer.SyncState();


        }

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.ItemId)
        //    {
        //        case Android.Resource.Id.Home:
        //            {
        //                MyDrawer.CloseDrawer(MyListView);
        //                manageDrawer.OnOptionsItemSelected(item);
        //                break;
        //            }
        //    }
        //    return base.OnOptionsItemSelected(item);
        //}

        protected override void OnPause()
        {
            // Pause the GPS - we won't have to worry about showing the 
            // location.
            googleMap.MyLocationEnabled = true;

            googleMap.MarkerClick -= MapOnMarkerClick;
            googleMap.InfoWindowClick -= HandleInfoWindowClick;

            base.OnPause();
        }

        private void AddInitialPolarBarToMap()
        {
            MarkerOptions markerOptions = new MarkerOptions()
                                .SetSnippet("Click me to go on vacation.")
                                .SetPosition(LeaveFromHereToMaui)
                                .SetTitle("Goto Maui");
            polarBearMarker = googleMap.AddMarker(markerOptions);
            polarBearMarker.ShowInfoWindow();

            gotMauiMarkerId = polarBearMarker.Id;

            PositionPolarBearGroundOverlay(LeaveFromHereToMaui);
        }

        /// <summary>
        ///     Add three markers to the map.
        /// </summary>
        private void AddMonkeyMarkersToMap()
        {
            for (int i = 0; i < LocationForCustomIconMarkers.Length; i++)
            {
                BitmapDescriptor icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.monkey);
                MarkerOptions markerOptions = new MarkerOptions()
                                    .SetPosition(LocationForCustomIconMarkers[i])
                                    .SetIcon(icon)
                                    .SetSnippet($"This is marker #{i}.")
                                    .SetTitle($"Marker {i}");
                googleMap.AddMarker(markerOptions);
            }
        }

        private void HandleInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            CircleOptions circleOptions = new CircleOptions();
            circleOptions.InvokeCenter(InMaui);
            circleOptions.InvokeRadius(100.0);
        }

        private void MapOnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
        {
            markerClickEventArgs.Handled = true;

            Marker marker = markerClickEventArgs.Marker;
            if (marker.Id.Equals(gotMauiMarkerId))
            {
                PositionPolarBearGroundOverlay(InMaui);
                googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(InMaui, 13));
                gotMauiMarkerId = null;
                polarBearMarker.Remove();
                polarBearMarker = null;
            }
            else
            {
                Toast.MakeText(this, $"You clicked on Marker ID {marker.Id}", ToastLength.Short).Show();
            }
        }

        private void PositionPolarBearGroundOverlay(LatLng position)
        {
            if (polarBearOverlay == null)
            {
                BitmapDescriptor polarBear = BitmapDescriptorFactory.FromResource(Resource.Drawable.polarbear);
                GroundOverlayOptions groundOverlayOptions = new GroundOverlayOptions()
                                           .InvokeImage(polarBear)
                                           .Anchor(0, 1)
                                           .Position(position, 150, 200);
                polarBearOverlay = googleMap.AddGroundOverlay(groundOverlayOptions);
            }
            else
            {
                polarBearOverlay.Position = InMaui;
            }
        }


    }
}
