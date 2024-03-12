# Class-C-Subnetting

This C# program allows users to perform Class C subnetting by inputting an IP address, the number of network bits, 
and the desired number of hosts per subnet. Here's a brief breakdown:

Main method : 
which initializes a Subneting object and calls the subnettingMenu method to interact with the user.

The subnettingMenu method :
repeatedly prompts the user for input until they choose to exit. 
It collects the IP address, network bits, and desired number of hosts per subnet.

The Subneting class : 
handles the subnetting logic. It calculates and stores details for each subnet. including ,
  network address
  subnet mask
  broadcast address
  usable IP range
  and default gateway.

Methods like setNA, setSubnetMask, setBA, setRange, and setDG are responsible for setting up subnet details based on user input.
Additional utility methods like getnearestNumber and getSubnetMask help in determining subnet mask values based on the number of hosts.
