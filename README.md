# Vue3D
App for the 'Unity Developer Programming Test' at 900Ibs of Creative.

## Overview
This app was created in 3 business days, using Unity and Vuforia to create a basic augmented reality app experience. It allows you to visualize a [Stereoscopic Camera](https://sketchfab.com/3d-models/stereoscopic-camera-by-heinrich-ernemann-bae7b4c444b341b282e101596fdf8ea6) when you scan the target image with the app.

## Steps On Using
- Print out the [Sample Target Image](https://library.vuforia.com/sites/default/files/vuforia-library/docs/target_samples/unity/mars_target_images.pdf).
- Install and run the app on an Android device.
- Point your device's camera towards the printed out target image, ensuring you're in a well-lit area.

## Controls:
- **One Finger** to move the object around.
- **Pinch/Panning** scales the object down and up.
- **Yellow Slider** controls the rotation speed of the object.
- **Blue Slider** controls the translation speed of the object.
- **RGB Sliders** affect the material on the Object's renderer.
- **Rotation Arrows** rotates the object left or right;
- **Compass (Center of Arrows)** resets the object back. *Except for RGB changes

## Tested On
- Samsung Galaxy Note 10+ (No Issues)
- Samsung Galaxy Note 9 (No Issues)
- Samsung Galaxy Tab A (2016 Version, Model: SM-T580) - Minor Graphical Issue [While tracking the object, rendering would have gray dots appear in the scene, everything was functional and no crashes occurred.]

## Licenses & Attribution
- Rotational Arrows and Compass I modified from [Blindman67's Arrow Listing](https://opengameart.org/content/30-drag-rotate-move-cursors)
- Stereoscopic Camera from [Heinrich Ernemann @ SketchFab](https://sketchfab.com/3d-models/stereoscopic-camera-by-heinrich-ernemann-bae7b4c444b341b282e101596fdf8ea6)
- Unity Version 2019.4.20f1 [https://unity.com/]
- Vuforia Ver: 9.8.8 [https://developer.vuforia.com/] 
