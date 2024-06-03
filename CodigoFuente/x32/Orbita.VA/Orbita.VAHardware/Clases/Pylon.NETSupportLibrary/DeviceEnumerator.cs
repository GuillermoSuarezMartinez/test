using System;
using PylonC.NET;
using System.Collections.Generic;

namespace PylonC.NETSupportLibrary
{
    /* Provides methods for listing all available devices. */
    public static class DeviceEnumerator
    {
        /* Data class used for holding device data. */
        public class Device
        {
            public string Name; /* The friendly name of the device. */
            public uint Index; /* The index of the device. */
            public string Serial; /* The serial of the device. */
            public string DeviceClass; /* The type of device of the device. */
            public string DeviceFactory; /* The factory number of the device. */
            public string DeviceVersion; /* The version number of the device. */
            public string FullName; /* The full name of the device. */
            public string ModelName; /* The model name of the device. */
            public string UserDefinedName; /* The user defined name of the device. */
            public string VendorName; /* The vendor name of the device. */
        }

        /* Queries the number of available devices and creates a list with device data. */
        public static List<Device> EnumerateDevices()
        {
            /* Create a list for the device data. */
            List<Device> list = new List<Device>();

            /* Enumerate all camera devices. You must call 
            PylonEnumerateDevices() before creating a device. */
            uint count = Pylon.EnumerateDevices();

            /* Get device data from all devices. */
            for( uint i = 0; i < count; ++i)
            {
                /* Create a new data packet. */
                Device device = new Device();
                /* Get the device info handle of the device. */
                PYLON_DEVICE_INFO_HANDLE hDi = Pylon.GetDeviceInfoHandle(i);
                /* Set the index. */
                device.Index = i;
                /* Get the name. */
                device.Name = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoFriendlyNameKey);
                /* Get the Serial. */
                device.Serial = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoSerialNumberKey);
                /* Get the DeviceClass. */
                device.DeviceClass = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoDeviceClassKey);
                /* Get the DeviceFactory. */
                device.DeviceFactory = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoDeviceFactoryKey);
                /* Get the DeviceVersion. */
                device.DeviceVersion = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoDeviceVersionKey);
                /* Get the FullName. */
                device.FullName = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoFullNameKey);
                /* Get the ModelName. */
                device.ModelName = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoModelNameKey);
                /* Get the UserDefinedName. */
                device.UserDefinedName = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoUserDefinedNameKey);
                /* Get the VendorName. */
                device.VendorName = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoVendorNameKey);
                /* Add to the list. */
                list.Add(device);
            }
            return list;
        }
    }
}
