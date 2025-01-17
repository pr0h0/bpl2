/**
  STL in BPL is currently pretty limited and we're wroking on it.
  For now, you can use the following STL functions:
  - print(value:ANY):VOID - prints the value to the console
    Prints value to the console. It uses console.log() under the hood, so it can print any type.

  - input(prompt:STRING):STRING - prompts the user for input
    Prompts the user for input and returns the result as a string. Current implementation uses readline-sync under the hood.
    We found that readline-sync is not very reliable, so we're working on a better solution. 
    You may need to enter value twice for it to work.

  - time():NUMBER - returns the current time in seconds (JavaScript Date.now())
    Returns current timestamp in ms. It uses Date.now() under the hood.

  - typeof(value:ANY):STRING - returns the type of the value
    Returns the type of the value as a string. You can use it to check the type of the value.
    It's uses internal types of BPL, so it may not be the same as JavaScript types.

  - convert(value: ANY, type: TYPE): (value of that type) - converts the value to the given primitive type
    Converts the value to the given type. It's recommended to use it for eg. with input to convert STRING to NUMBER.
    NUMBER uses JS Number(value), STRING uses String(value), BOOLEAN uses Boolean(value).

  Also BPL has some predefined variables you may run into:
  - PI:NUMBER - Math.PI (3.141592653589793)
  - version:STRING - current BPL version

  We're working on adding more STL functions and variables, so stay tuned!
**/

// Example input 
var name:STRING = input("Enter your name: ");

// Example output
print("Hello, " + name + "!");

// Example timestamp
var timestamp: NUMBER = time();
print(`Current timestamp: ${timestamp}`);

// Example typeof
print(`Typeof timestamp: ${typeof(timestamp)}`);

// Example convert
print(`Typeof timestamp: ${typeof(convert(timestamp, STRING))}`);
