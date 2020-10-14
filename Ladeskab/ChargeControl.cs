using System;
using UsbSimulator;

namespace Ladeskab
{
    public class ChargeControl : IChargeControl
    {
        private readonly IUsbCharger _usbCharger;
        private readonly IDisplay _display;


        private enum ChargerState
        {
            Ready,
            TrickleChargeFullyCharged,
            IsCharging,
            OverCurrentFail
        };
        private ChargerState _chargerState = ChargerState.Ready;

        // Constants:
        private const double MaxChargingCurrent = 500.0;
        private const double MinChargingCurrent = 5.0;
        

        // Attributes
        private double _chargingCurrent = 0.0;
        public bool UsbChargerIsConnected = false;
        
        // Methods
        
        public ChargeControl(IDisplay display, IUsbCharger usbCharger)
        {
            // Directly Use Display:
            _display = display;

            // Directly Use USB Charger:
            _usbCharger = usbCharger;

            // Attach to ChargingCurrentUpdate events from USB Charger:
            _usbCharger.ChargingCurrentEvent += OnChargingCurrentUpdate;
        }

        public void StartCharge()
        {
            _usbCharger.StartCharge();
            _chargerState = ChargerState.Ready;
        }

        public void StopCharge()
        {
            _usbCharger.StopCharge();
            _chargerState = ChargerState.Ready;
        }

        public bool IsConnected()
        {
            return UsbChargerIsConnected;
        }

        private void OnChargingCurrentUpdate(object sender, CurrentEventArgs e)
        {
            _chargingCurrent = e.Current;

            EvaluateChargerState();
            UpdateDisplay();
        }
        
        // OnChargingCurrentUpdate Helper Functions
        private void EvaluateChargerState()
        {
            if (_chargingCurrent > MaxChargingCurrent)
            {
                _usbCharger.StopCharge();
                _chargerState = ChargerState.OverCurrentFail;
                UsbChargerIsConnected = true;
            }
            else if (_chargingCurrent > MinChargingCurrent &&
                     _chargingCurrent < MaxChargingCurrent)
            {
                _chargerState = ChargerState.IsCharging;
                UsbChargerIsConnected = true;
            }
            else if (_chargingCurrent > 0.0 &&
                     _chargingCurrent < MinChargingCurrent)
            {
                _chargerState = ChargerState.TrickleChargeFullyCharged;
                UsbChargerIsConnected = true;
            }
            else if (_chargerState != ChargerState.OverCurrentFail)
            {
                _chargerState = ChargerState.Ready;
                UsbChargerIsConnected = false;
            }
        }
        private void UpdateDisplay()
        {
            switch (_chargerState)
            {
                case ChargerState.OverCurrentFail:
                    _display.OverCurrentFail();
                    break;

                case ChargerState.Ready:
                    _display.NotCharging();
                    break;

                case ChargerState.TrickleChargeFullyCharged:
                    _display.FullyCharged();
                    break;

                case ChargerState.IsCharging:
                    _display.IsCharging();
                    break;
                default:
                    // ?
                    break;
            }
            
        }




    }

}
