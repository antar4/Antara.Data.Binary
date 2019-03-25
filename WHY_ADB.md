# ![alt text](http://cdn.3volve.io/adb/images/logo/0.5x/logo@0.5x.png "ADB Logo")   Why ADB?

**Q:** I read all of the stuff you wrote and still I'm not sure why should I use ADB over one of the other serializers. After all they are established and as you already said faster in their benchmarks.  
**A:** If you need to serialize generic data with lots of strings for boilerplate application using one of the other serializers is probably your best bet. However if you have millions of items you need to serialize that contain mostly numbers and booleans and speed and size matters, ADB is what you need.

**Q:** Why is that?   
**A:** ADB gives you complete control on how your data is serialized to the bit level, while for most (if not all) serializers you have control at the byte level. That is also the reason these libraries perform faster when it comes to strings and other "full" datatypes.

**Q:** Can you elaborate? I'm not sure what "bit level" means.  
**A:** Sure, I'll give you a an example.  
Lets say you have the following class:

```C#
public class EmployeeDetails {
    public short BirthYear {get; }
    public bool IsMale {get; }
    public byte NumberOfChildren {get; }
    public EMartialStatus MaritaStatus {get; }
    public short VacationHours { get; }
    public short SickLeaveHours { get; }
    public byte OrganizationLevel { get; }
    public DateTime HireDate { get; }
}
enum EMaritalStatus {
    MARRIED = 0,
    SINGLE = 1,
    DIVORCED = 2,
    WIDOWED = 3,
}
```

So, serializing the above class with a serializer would result to the following size:

Variable | Type | Size (bytes) 
------------|--------|-------:
BirthYear|short|2
IsMale|bool|1
NumberOfChildren|byte|1
EMaritalStatus|int|4
VacationHours|short|2
SickLeaveHours|short|2
OrganizationLevel|byte|1
HireDate|DateTime|8
Total|-|**21**

So it would take 21 bytes to serialize this class. Now lets try to serialize with ADB.

Each and every one of these values contain information that has a real world value. They are not just int and short. For each of those we can establish a maximum value and use these bits to serialize:

Variable|Type|Max Value|Size (bits)
-----------|-------|--------------|-------------:
BirthYear|short|2048|11
IsMale|bool|-|1
NumberOfChildren|byte|128|7
EMaritalStatus|int|3|2
VacationHours|short|2048|11
SickLeaveHours|short|4096|12
OrganizationalLevel|byte|64|6
HireDate|DateTime|2048 for year|21
Total|-|-|**71**

A total of 71 bits is enough to store the information. This is just below 9 bytes. A staggering ~58% smaller size. Or simply other serializers would require **2.3** times more space to serialize this information. 

If we were to compare the speed now, you will also find that ADB will outperform other serializers.

So to answer your question, Why ADB? With ADB you can serialize real world values. With other serializers you can serialize computer constructs.

For more please see: [Examples Games](EXAMPLES_GAMES.md), [Examples Finance](EXAMPLES_FINANCE.md)





