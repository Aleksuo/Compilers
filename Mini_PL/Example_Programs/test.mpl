var nTimes : int := 0;
print "How many times?";
read nTimes;
var x : int;
for x in 0..nTimes do
	print x;
	print " : Hello, World!";
	var y : int;
		for y in 0..nTimes-1 do
		print "sisa";
		end for;
end for;assert(x = nTimes);