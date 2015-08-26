# Introduction #

The WS4000 uses the HID USB interface for communication with the PC. This page describes the implementation of the USB driver used in the wesather-logger project.

The implementation is heavily based on this [codeproject article](http://www.codeproject.com/KB/cs/USB_HID.aspx). In fact all the low-level code is taken directly from that implementation. The high-level interface is still based on that article but slightly generalized and extended with the WS4000 specific interface.

In the source for our project I have split the USB interface in two separate libraries.
  * The `UsbLibrary` is basically the [codeproject](http://www.codeproject.com/KB/cs/USB_HID.aspx) implementation.
  * The `WeatherLoggerLib` contains the WS4000 specific extensions to `UsbLibrary`.

# The implementation #
The following picture shows the classes in `UsbLibrary`
<wiki:gadget url="http://creately.com/player/gadget/createlyplayer.xml" height="1000" width="1000" border="0" up\_did="gnybuyyb1" up\_dlogo="true" up\_dtitle="Embedding in Google Code" up\_bgcolor="#EEEEEE" />

The classes above the line re in `UsbLibrary`, the ones below the line are in `WeatherLoggerLib`.
## `UsbLibrary` ##
At the heart of the library are the (abstract) class `HIDDevice` and `UsbHidPort`. These class implement all of the low-level interfacing to a HID USB device.
### `UsbHidPort` ###
This class is in the form of a `Component`. It is part of the Windows (GUI) message loop and will detect addition and removal of HID USB devices.