## AR.Drone [![Build Status](https://travis-ci.org/Ruslan-B/AR.Drone.png)](https://travis-ci.org/Ruslan-B/AR.Drone)

The AR.Drone 2.0 controlling library for C#/.NET and Mono, with video support.  
Built over the original [AR.Drone SDK](https://projects.ardrone.org) 2.0.1 - using lastest drone firmware.

Thanks to [Yury Rozhdestvensky](https://github.com/yur) from [robodem.com](http://robodem.com) we got ability to create command chains, 
thus basic autopilot is avalible now, for details please check ```Avionics.Autopilot``` module and simple example in demo application either.

If case you are looking for Windows RT/Windows Phone support please check this project [ARDrone2Windows](https://github.com/ARDrone2Windows/SDK).

## Documentation

[Wiki](https://github.com/Ruslan-B/AR.Drone/wiki).

## Dependencies

[FFmpeg.AutoGen](https://github.com/Ruslan-B/FFmpeg.AutoGen) - .NET wrapper for FFmpeg.  

## Status

This library is stable now. All major features are supported - video, configuration and control.  
Please note that this library still under development, 
so please don't be suprised if you find some functionality missing or undocumented.  

## Build

How to build from scratch:  
- Clone this:

```bash
git clone git://github.com/Ruslan-B/AR.Drone.git   
cd AR.Drone   
git submodule update --init   
```  

- For the **video support** please review: **[Usage](https://github.com/Ruslan-B/FFmpeg.AutoGen#Usage)** section of the [FFmpeg.AutoGen](https://github.com/Ruslan-B/FFmpeg.AutoGen) project.

- Build AR.Drone solution with MonoDevelop, VS2010 or VS2012.

Please note: for opening solution in VS2010 you should have *Microsoft Visual Studio 2010 Service Pack 1* installed.

## Usage

The solution includes Winform application - AR.Drone.WinApp, it provides minimalistic interface 
for controling and displaying video from the AR.Drone 2.0.

##Contributions
```Avionics.Autopilot``` module by [Yury Rozhdestvensky](https://github.com/yur) from [robodem.com](http://robodem.com).  

##License

Copyright 2013 Ruslan Balanukhin ruslan.balanukhin@gmail.com

GNU Lesser General Public License (LGPL) version 3 or later.  
http://www.gnu.org/licenses/lgpl.html

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
