using System;
using System.Runtime.InteropServices;
using UsbSimulator;

namespace Ladeskab
{
    public class ChargeControl : IChargeControl
    {
        private readonly IUsbCharger _usbCharger;
        private readonly IDisplay _display;

        private enum ChargerState
        {
            Idle,
            TrickleChargeFullyCharged,
            IsCharging,
            OverCurrentFail
        };
        private ChargerState _chargerState = ChargerState.Idle;

        // Constants
        private const double MaxChargingCurrent = 500.000;
        private const double MinChargingCurrent = 5.000;
        private const double ZeroChargingCurrent = 0.000;
        

        // Attributes
        private double _chargingCurrent = 0.0;
        public double ReadChargingCurrent = -1.0;
        private bool _deviceToChargeIsConnected = false;
        
        // Methods
        public ChargeControl(IDisplay display, IUsbCharger usbCharger)
        {
            // Directly Use Display:
            _display = display;

            // Directly Use USB Charger:
            _usbCharger = usbCharger;

            // Attach to ChargingCurrentUpdate events from USB Charger:
            this._usbCharger.ChargingCurrentEvent += OnChargeCurrentUpdate;
        }

        public void StartCharge()
        {
            _chargerState = ChargerState.Idle;
            _usbCharger.StartCharge();
        }

        public void StopCharge()
        {
            _chargerState = ChargerState.Idle;
            _usbCharger.StopCharge();
        }

        public bool IsConnected()
        {
            return _usbCharger.Connected;
        }

        private void OnChargeCurrentUpdate(object sender, CurrentEventArgs e)
        {
            _chargingCurrent = e.Current;
            ReadChargingCurrent = _chargingCurrent;

            EvaluateChargerState();
            UpdateDisplay();
        }
        
        // OnChargeCurrentUpdate Helper Functions
        private void EvaluateChargerState()
        {
            if (_chargingCurrent > MaxChargingCurrent)
            {
                _usbCharger.StopCharge();
                _chargerState = ChargerState.OverCurrentFail;
                _deviceToChargeIsConnected = true;
            }
            else if (_chargingCurrent < MaxChargingCurrent &&
                     _chargingCurrent > MinChargingCurrent)
            {
                _chargerState = ChargerState.IsCharging;
                _deviceToChargeIsConnected = true;
            }
            else if (_chargingCurrent < MinChargingCurrent &&
                     _chargingCurrent > ZeroChargingCurrent)
            {
                _chargerState = ChargerState.TrickleChargeFullyCharged;
                _deviceToChargeIsConnected = true;
            }
            else
            {
                // Persist OverCurrent State Message in Display
                // until manually reset is performed by invoking StartCharge()
                if (_chargerState == ChargerState.OverCurrentFail) return;

                _chargerState = ChargerState.Idle;
                _deviceToChargeIsConnected = false;
            }
        }
        private void UpdateDisplay()
        {
            switch (_chargerState)
            {
                case ChargerState.OverCurrentFail:
                    _display.OverCurrentFail();
                    break;

                case ChargerState.Idle:
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
