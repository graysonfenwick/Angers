# Angers

This was a project worked on and presented in the Summer of 2017 at the University of Angers in France. The purpose of the project was to use Unity, a game engine with integrated physics, and C# to create a tool that students would use to test their differential equations for automatic control. Automatic control is the term used for the automatic balancing of objects by adjusting the environment in which they exist: for example, a crane uses automatic control to prevent excessive swinging of its payload. 

In this project, three environments were created: a rotating beam and a ball, a plate and a ball, and a rotating pendulum with a ball attached on a freely-swinging arm. The beam project and the plate project were both completed, while the rotating pendulum project was ended as the internship came to a close. 

All of the projects use a separate Windows Form through C# that uses shared memory between the Unity software and the Windows Forms. There are two Windows Forms, which uses multithreading to allow the duel adjustment of the shared memory file. One of the Windows Forms allows for the selection of the angle for the beam to rotate to, and the other tracks the changes of the dynamic environmental pieces in the Unity simulation. 

This project worked closely with Dr. Cottenceau of the Engineering Department at the University of Angers. 
