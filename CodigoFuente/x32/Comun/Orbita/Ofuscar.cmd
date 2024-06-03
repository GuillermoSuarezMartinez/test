@echo off
del C:\TeamFoundation\Orbita\General\Dlls\x32\Orbita\Orbita.BBDD.dll /q/f
"C:\Archivos de programa\Red Gate\SmartAssembly 5\SmartAssembly.com" /build .\Orbita.BBDD32.saproj
verpatch C:\TeamFoundation\Orbita\General\Dlls\x32\Orbita\Orbita.BBDD.dll /s company "Orbita Software" /s copyright "Copyright © Orbita Ingenieria SW 2012"

del C:\TeamFoundation\Orbita\General\Dlls\x32\Orbita\Orbita.Comunicaciones.dll /q/f
"C:\Archivos de programa\Red Gate\SmartAssembly 5\SmartAssembly.com" /build .\Orbita.Comunicaciones32.saproj
verpatch C:\TeamFoundation\Orbita\General\Dlls\x32\Orbita\Orbita.Comunicaciones.dll /s company "Orbita Software" /s copyright "Copyright © Orbita Ingenieria SW 2012"

pause
