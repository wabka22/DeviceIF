ThinkGearNET
v1.1
by Brian Peek (http://www.brianpeek.com/)

This is a simple wrapper library around Neurosky's P/Invoke definitions that
allow you to work with the Neurosky MindSet headset a bit more easily.  The
library can be setup to poll asynchronously and raise events when new data
is available, or it can be polled whenever data is required, such as in the
Update method of an XNA application.

Look at the included ThinkGearNETTest application for an example on how to
use the library in a regular Windows application.

**************
- IMPORTANT! -
**************
Copy the thinkgear.dll file located in the ThinkGearNET_Extras directory to 
the output directory of your application.  This is the native ThinkGear SDK 
which this library wraps.  If this file is not included in the output 
directory, your application will fail to run!

If you have questions or issues, please contact me through my site linked
above.  Thanks!

Changelog
---------
- Version 1.1
	- Added support for blink detection