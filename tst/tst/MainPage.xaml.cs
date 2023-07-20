namespace tst;

#if ANDROID
using Android.Content;
using Android.Telephony;
using tst.state;
public partial class MainPage : ContentPage
{
    private Context myContext = MainApplication.Context;
    private StateChange StateChangeListener = new StateChange();
    private TelephonyManager tpMgr;
    int count = 0;

	public MainPage()
	{
		InitializeComponent();

        tpMgr = (TelephonyManager)myContext.GetSystemService(Context.TelephonyService);
        StateChangeListener.SignalStrengthsChangedEvent += OnSignalStrengthsChanged;
    }

    private async Task<PermissionStatus> RequestCoarseLocationPermissions()
    {
        PermissionStatus coarseLocationPermission = PermissionStatus.Unknown;
        try
        {
            // Check Coarse Location permission status
            coarseLocationPermission = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (coarseLocationPermission == PermissionStatus.Granted)
                return coarseLocationPermission;

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                await Shell.Current.DisplayAlert("Needs Permissions", "Location is required in order to obtain signal information from base station. Please enable this for the app to work properly.", "OK");
            }

            coarseLocationPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await Shell.Current.DisplayAlert("Error", "Error when getting permissions. Please send us a message so we can fix this!", "OK");
        }
        return coarseLocationPermission;
    }

    private async Task<PermissionStatus> RequestOtherPermissions()
    {
        PermissionStatus networkStatePermission = await Permissions.CheckStatusAsync<Permissions.NetworkState>();
        PermissionStatus fineLocationPermission = PermissionStatus.Unknown;
        try
        {
            // Check Network State permission status
            fineLocationPermission = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();

            if (networkStatePermission == PermissionStatus.Granted)
                return networkStatePermission;

            if (Permissions.ShouldShowRationale<Permissions.LocationAlways>())
            {
                await Shell.Current.DisplayAlert("Needs Permissions", "Network permissions is required in order to obtain signal information from base station. Please enable this for the app to work properly.", "OK");
            }

            networkStatePermission = await Permissions.RequestAsync<Permissions.LocationAlways>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await Shell.Current.DisplayAlert("Error", "Error when getting permissions. Please send us a message so we can fix this!", "OK");
        }

        try
        {
            // Check Network State permission status
            networkStatePermission = await Permissions.CheckStatusAsync<Permissions.NetworkState>();

            if (networkStatePermission == PermissionStatus.Granted)
                return networkStatePermission;

            if (Permissions.ShouldShowRationale<Permissions.NetworkState>())
            {
                await Shell.Current.DisplayAlert("Needs Permissions", "Network permissions is required in order to obtain signal information from base station. Please enable this for the app to work properly.", "OK");
            }

            networkStatePermission = await Permissions.RequestAsync<Permissions.NetworkState>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await Shell.Current.DisplayAlert("Error", "Error when getting permissions. Please send us a message so we can fix this!", "OK");
        }

        return networkStatePermission;
    }

    protected override async void OnAppearing()
    {
        var locationPermission = await RequestCoarseLocationPermissions();
        await RequestOtherPermissions();

        if (locationPermission == PermissionStatus.Granted)
        {
            //tpMgr.RegisterTelephonyCallback(includeLocationData: 1, myContext.MainExecutor, statechange);
            tpMgr.Listen(StateChangeListener, PhoneStateListenerFlags.SignalStrengths);
        }
    }

    private void OnSignalStrengthsChanged(object sender, EventArgs e)
    {
        var signals = tpMgr.AllCellInfo;
        foreach (CellInfo signal in signals)
        {
            // TODO: Create generic function since most if not all operations are reproduced for LTE/5G/etc.
            double r;
            switch (signal)
            {
                case CellInfoLte LTE:
                    if (LTE.IsRegistered)
                        r = LTE.CellSignalStrength.Rsrp;
                    break;
            }
        }
    }

    private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}
#else

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

    }
}
#endif
