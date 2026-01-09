using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.PointOfService.Provider;
using Windows.Devices.Radios;

namespace Yesdude.Bluetooth
{
    public class BluetoothManager
    {
        public static async Task ToggleBluetoothAsync()
        {
            var radios = await Radio.GetRadiosAsync();
            var bluetoothRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth);

            if (bluetoothRadio != null)
            {
                if (bluetoothRadio.State == RadioState.On)
                {
                    // Turn Bluetooth Off
                    await bluetoothRadio.SetStateAsync(RadioState.Off);
                    Console.WriteLine("Bluetooth turned off.");
                }
                else if (bluetoothRadio.State == RadioState.Off)
                {
                    // Turn Bluetooth On
                    await bluetoothRadio.SetStateAsync(RadioState.On);
                    Console.WriteLine("Bluetooth turned on.");
                }
                else
                {
                    Console.WriteLine($"Bluetooth is in an unexpected state: {bluetoothRadio.State}");
                }
            }
            else
            {
                Console.WriteLine("No Bluetooth radio found.");
            }
        }

        public static async Task SetBluetoothState(Boolean enable)
        {
            var radios = await Radio.GetRadiosAsync();
            var bluetoothRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth);

            if (bluetoothRadio != null)
            {
                if (!enable && bluetoothRadio.State == RadioState.On)
                {
                    // Turn Bluetooth Off
                    await bluetoothRadio.SetStateAsync(RadioState.Off);
                    Console.WriteLine("Bluetooth turned off.");
                }
                else if (enable && bluetoothRadio.State == RadioState.Off)
                {
                    // Turn Bluetooth On
                    await bluetoothRadio.SetStateAsync(RadioState.On);
                    Console.WriteLine("Bluetooth turned on.");
                }
                else
                {
                    Console.WriteLine($"Bluetooth is in an unexpected state: {bluetoothRadio.State}");
                }
            }
            else
            {
                Console.WriteLine("No Bluetooth radio found.");
            }
        }


        public static async Task<bool> IsBluetoothEnabledAsync()
        {
            var radios = await Radio.GetRadiosAsync();
            var bluetoothRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth);
            return bluetoothRadio != null && bluetoothRadio.State == RadioState.On;
        }

    }
}
