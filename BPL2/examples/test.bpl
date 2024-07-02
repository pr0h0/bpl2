var i:NUMBER = 0;
var s:STRING = "5";
var x:STRING = version;
var y:NUMBER = PI;

print(y);
print(i);
print(x);
print(s);

// var name:STRING = input("name?:");
// name;
// if(name != '') {
//   print(name);
// }
// print(name == '');
// print(typeof(name));

var startTime : NUMBER = time();

// var i: NUMBER = 0;

while(i < 1000) {
  i = i + 1;
}

print(convert(time(), STRING));
print(convert(startTime, STRING));
print(time() - startTime);