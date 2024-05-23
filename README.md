# Simple Pathfinder
Simple pathfinder with AStar algorithm implemented for Unity3D.

## Perspective and orthographic camera modes

### Perspective camera
Useful if we want to see the map in 3D mode and more details on character or tiles. User can scroll in this mode.
![image](https://github.com/OnistDerFalke/simple_pathfinder/assets/75864407/33fcf975-08e9-4c53-b560-2da2214c8dab)


### Orthographic camera
Default. Usefull when we want to see map almost in 2D mode. No scroll option available.
![image](https://github.com/OnistDerFalke/simple_pathfinder/assets/75864407/245fea80-cc7a-4898-aeed-7c17754b2e58)


## Controls
* [Esc] - Close application
* [H] - Hide/Show UI
* [LMB] (Left Mouse Button) - Changes tile type (normal - green color, obstacle - red color)
* [RMB (Right Mouse Button) + Q] - Sets path start on cursor position (white circle).
* [RMB (Right Mouse Button) + E] - Sets path end on cursor position (black circle).
* [P] - Switch camera projection (perspective, orthographic). Perspective camera has 60 deg rotation to show that app runs in 3D mode.
* [W,S,A,D or Arrows] - Move left, right, forward, backward.
* [Mouse scroll] - Zoom, but works only on projection camera, in orthographics whole map is visible.

## Animations
User can play an animation where character walk from start point to end the path which was created by AStar algorithm. Player cannot change start and end points or obstacles during the animation. Player can see the path marked on the map or not during animation.

![image](https://github.com/OnistDerFalke/simple_pathfinder/assets/75864407/01ff2f51-b52f-4e89-a310-4162c68e3d65)

## General Buttons
### Clear path
Clears path leaving only start, end and obstacles.
### Find path
Generates the shortest path using AStar algorithm. After adding obstacles or start/end points, need to be pressed to update the path.
### Apply
Applies changes on map size and removes all obstacles and move start and end to the corners.
### Animation buttons
Green to start animation, red to stop animation.


## Pathfinding

Algorithm used in this solution is AStar. Algorithm was implemented from scratch. It uses Manhattan Distance heuristics. It was proven that it is more effective than standard Euclidean Distance for discrete grids. Algorithm was tested on bigger maps with random obstacles. It works as fast that there is no possibility to check efficiency on larger map because of limited memory for tile grid in this project. AStar algorithm is one of most popular heuristic pathfinding algorithm. It works really fast with well chosen heuristics and gives possibility to work with different movement costs.

AStar returns shortest path from start to end. It uses specific implementation of the binary heap because it helps to find nodes with the lowest costs quickly during graph exploration. 

## References
[1] *A Systematic Literature Review of A\* Pathfinding*, Daniel Foeada, Alifio Ghifaria, Marchel Budi Kusumaa, Novita Hanafiahb, Eric
Gunawanb. Source: https://www.sciencedirect.com/science/article/pii/S1877050921000399.

[2] *Application of A-Star Algorithm on Pathfinding Game* Ade Candra, Mohammad Andri Budiman and Rahmat Irfan Pohan. Source: https://iopscience.iop.org/article/10.1088/1742-6596/1898/1/012047/meta
