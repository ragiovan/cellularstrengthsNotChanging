#if ANDROID
using Android.Content;
using Android.OS;
using Android.Telephony;
using System.Diagnostics;

namespace tst.state
{
    public class StateChange : PhoneStateListener, TelephonyCallback.ISignalStrengthsListener, TelephonyCallback.ICellInfoListener, TelephonyCallback.ICellLocationListener
    {
        public EventHandler SignalStrengthsChangedEvent;

        public override void OnSignalStrengthsChanged(SignalStrength signalStrength)
        {
            SignalStrengthsChangedEvent?.Invoke(sender: this, EventArgs.Empty);
        }
    }
}
#endif