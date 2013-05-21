## AR.Drone

The AR.Drone 2.0 controlling library for C#/.NET and Mono, with video support.  
Built over original AR.Drone SDK 2.0.1 - using lastest drone firmware.

## Dependencies

[AI.Core](/Ruslan-B/AI.Core) - Set of core libraries for autonomous agent support.  
[FFmpeg.AutoGen](/Ruslan-B/FFmpeg.AutoGen) - .NET wrapper for FFmpeg.  

## Status

This library is still under heavy development, 
so please don't be suprised if you find some functionality missing or undocumented.  
Drone controling, access to configuaration, video streaming support are working fine.

Things todo:  
[ ] Multi-configuration support;  
[ ] Wifi managment (only on Windows so far).  

## Build

How to build from scratch:  
- Clone this:

```bash
git clone git://github.com/Ruslan-B/AR.Drone.git   
cd AR.Drone   
git submodule update --init   
```  

- For video support please review [FFmpeg.AutoGen](/Ruslan-B/FFmpeg.AutoGen).

- Build AR.Drone solution with MonoDevelop, VS2010 or VS2012.

## Usage

The solution includes Winform application - AR.Drone.WinApp, it is includes minimalistic interface 
for control and display video from AR.Drone 2.0.

## License

Copyright 2013 Ruslan Balanukhin ruslan.balanukhin@gmail.com

GNU General Public License (GPL) version 3 or later.  
http://www.gnu.org/licenses/gpl.html

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