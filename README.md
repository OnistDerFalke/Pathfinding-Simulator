# Simple Pathfinder
Simple pathfinder with AStar algorithm implemented for Unity3D.

## Overview
### Perspective and orthographic camera modes

#### Perspective camera
![image](https://github.com/OnistDerFalke/simple_pathfinder/assets/75864407/eb932439-289b-4029-b8f4-4c8fe1f9565e)

#### Orthographic camera
![image](https://github.com/OnistDerFalke/simple_pathfinder/assets/75864407/d47303a4-4327-4650-bfb5-55a741154ade)

## Controls
* [Esc] - Close application
* [H] - Hide/Show UI
* [LMB] (Left Mouse Button) - Changes tile type (normal - green color, obstacle - red color)
* [RMB (Right Mouse Button) + Q] - Sets path start on cursor position (white circle).
* [RMB (Right Mouse Button) + E] - Sets path end on cursor position (black circle).
* [P] - Switch camera projection (perspective, orthographic). Perspective camera has 60 deg rotation to show that app runs in 3D mode.
* [W,S,A,D or Arrows] - Move left, right, forward, backward.
* [Mouse scroll] - Zoom, but works only on projection camera, in orthographics whole map is visible.

## Buttons
### Clear path
Clears path leaving only start, end and obstacles.
### Find path
Generates the shortest path using AStar algorithm.
### Apply
Applies changes on map size and removes all obstacles and move start and end to the corners.

![image](https://github.com/OnistDerFalke/simple_pathfinder/assets/75864407/6d1ec527-32c1-496d-9c9f-9f8b36820156)


## Pathfinding

Algorithm used in this solution is AStar. Algorithm was implemented from scratch. It uses Manhattan Distance heuristics. It was proven that it is more effective than standard Euclidean Distance for discrete grids. Algorithm was tested on bigger maps with random obstacles. It works as fast that there is no possibility to check efficiency on larger map because of limited memory for tile grid in this project. AStar algorithm is one of most popular heuristic pathfinding algorithm. It works really fast with well chosen heuristics and gives possibility to work with different movement costs.

AStar returns shortest path from start to end. It uses specific implementation of the binary heap because it makes helps find nodes with the lowest costs quickly during graph exploration. 

## References
[1] *A Systematic Literature Review of A\* Pathfinding*, Daniel Foeada, Alifio Ghifaria, Marchel Budi Kusumaa, Novita Hanafiahb, Eric
Gunawanb. Source: https://www.sciencedirect.com/science/article/pii/S1877050921000399.

[2] *Application of A-Star Algorithm on Pathfinding Game* Ade Candra1, Mohammad Andri Budiman and Rahmat Irfan Pohan. Source: https://iopscience.iop.org/article/10.1088/1742-6596/1898/1/012047/meta
