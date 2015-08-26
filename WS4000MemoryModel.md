# Introduction #

As described elsewhere we have access to the internal memory of the WS4000 weather station through the USB interface. This page gives a description of the memory layout and what part of the memory we should read to get to the information we want.

The information described here is based on the source code of the [PYWWS project](http://code.google.com/p/pywws/)


# Details #

The memory is divided in two major sections
  * The first 256 bytes (0 - 0xFF) contain global data
  * The rest of the memory (0x100 - 0xFFFF) contains snapshots of the weather.

## Weather Snapshots ##

The weather snapshots are blocks of 16 bytes that contains the weather info for a specific interval. The interval can be specified on the WS4000 through the PC application and defaults to 30 minutes.

One of those snapshots contains the current weather. It is updated every 48 seconds.

Once the specified interval in the WS4000 is over that snapshot is updated a last time to contain the data for that interval and a new 'current snapshot' is started empty.

All the snapshots are held in a circular buffer. A pointer to the current snapshot is held in the Global section. When the interval is completed this pointer will be incremented (by 16), wrapping around to the start of the snapshot memory when the top is reached.

## Global Data ##

The Global Data section contains a lot of information, including things like the alarm settings of the WS4000. For this application only a few are relevant and are described here.

  * **Data Count**. This is the number of snapshots in the memory, including the _Current_ snapshot. When the WS4000 is first started or reset, the count will be 1. If the memory is full, it will be 4080.
  * **Current Position**. This is the offset in the WS4000 memory of the _Current_ snapshot. This will be incremented (by 16) every time an interval 'expires'.
  * **Current date/time**. This is the date and time associated with the current snapshot.