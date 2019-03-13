# ![alt text](http://cdn.3volve.io/adb/images/logo/0.5x/logo@0.5x.png "ADB Logo")   Basic Usage

The central object in Antara.Data.Binary is the `BVector` class. It is your one stop shop for 99% of the operations in Antara.Data.Binary. It allows you to add and read data.

The basic concept of `BVector` is that you add data to it and if you know where you added it, you can read it back! 

**Q:** Great! And how do I use it?  
**A:** I'll give you some examples below:

- Adding an integer

```C#
BVector d = new BVector();
d.Add(1234); // adding
```

**Q:** You added an integer, can you do anything with it?  
**A:** Yes, I can convert it to a byte[]

```c#
byte[] b = d.ToBytes(); // serializing
```

**A:** And now lets get our integer back from the byte array

```c#
BVector d2 = new BVector(b); // deserializing
int myValue = d2.GetInt(); // reading
```

**Q:** I noticed you used a simple `Add` to put the number in the `BVector` but you used `GetInt` to read it, why is that?  
**A:** The `BVector` itself **only** stores the actual information. There is no metadata to describe what it holds. This means when you want to get your data back you need to know exactly what you added and where. The `BVector` actually uses **100%** of the space to store your data.

**Q:** Nice, and how do you add more information?

```C#
BVector d = new BVector();
d.Add1(true);
d.Add(1234);
d.Add("Foo");
d.Add((long)6);
d.Add((byte)64);

bool boolVal = d.Get1();
int intVal = d.GetInt();
string stringVal = d.GetString();
long longVal = d.GetLong();
byte byteVal = d.GetByte();
```

**A:** Hope this answers your question  
**Q:** Yes, and what datatypes are supported?  
**A:** Look here: [Supported data types](SUPPORTED_DATA_TYPES.md)

**Q:** Does it support `nullable` types?  
**A:** Yes it does, and the amazing with nullable types is that if the value is `null` it only requires 1 bit to stores the value, same as boolean values

**Q:** Can it do anything else? You talked about high level of control  
**A:** Sure, look at this example. Lets assume I have an enumeration or value that I *know* can only take values from 0-6. What data type should I use for this value?  
**Q:** You should probably go with `byte` and only use 1 bytes (8 bits)  
**A:** Not really, I actually need only 3 bits:

```c#
BVector d = new BVector();
int myVal = 2;
d.Add(myVal, 3);

int readMyVal = d.GetInt(3);
```

**Q:** What is the '3' in `d.Add(myVal, 3)`   
**A:** It actually tells the serializer that this value should only take 3 bits!  
**Q:** And what happens if I later want to change this to fit larger numbers? Got you!  
**A:** Not really, check the [Versioning](VERSIONING.md) section

Continue to [Usage in Classes](CLASSES.md) if you want to see how to use it in classes efficiently.







