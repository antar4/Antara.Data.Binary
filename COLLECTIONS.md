# Serializing collections

Serializing collections with Antara.Data.Binary follows the same logic as nested classes. 

There is a way to add Lists that use the basic data types (bool, int, byte, string etc.) but the most efficient way is to manually add them. For objects the only way to do so is manually. 

## Serializing/Deserializing an `int` list

### Setup

```C#
var myList = Enumerable.Range(0, 1000);
BVector d = new BVector();
```

### Serialize

```C#
d.AddLen((uint)myList.Count());
foreach (var intValue in myList)
{
    d.Add(intValue);
}
```

### Deserialize

```C#
var len = d.GetLen();
for (int i = 0; i < len; i++)
{
    myList.Add(d.GetInt());
}
```

**Q:** What is this `.AddLen()` and `.GetLen()`?  
**A:** This is a special function that you can use to add the length of an array. More at [Structure](STRUCTURE.md)



## Serializing/Deserializing an object list

### Setup

```c#
// class    
class Person
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public bool IsAdult { get; private set; }

    internal void AddToBinary(BVector d)
    {
        d.Add(Id);
        d.Add(Name);
        d.Add1(IsAdult);
    }
    internal void ReadFromBinary(BVector d)
    {
        Id = d.GetInt();
        Name = d.GetString();
        IsAdult = d.Get1();
    }
}

// vars
List<Person> myList = new List<Person>();
BVector d = new BVector();
```



### Serialize

```C#
d.AddLen((uint)myList.Count());
foreach (var p in myList)
{
    p.AddToBinary(d);
}
```



### Deserialize

```c#
var len = d.GetLen();
for (int i = 0; i < len; i++)
{
    Person p = new Person();
    p.ReadFromBinary(d);
    myList.Add(p);
}
```



## Serializing/Deserializing a Dictionary

### Setup

```c#
var myDict = new Dictionary<int, string>();
BVector d = new BVector();
```



### Serialize

```c#
d.AddLen((uint)myDict.Count);
foreach (var key in myDict.Keys)
{
    d.Add(key);
    d.Add(myDict[key]);
}
```



### Deserialize

```C#
var len = d.GetLen();
for (int i = 0; i < len; i++)
{
    myDict.Add(d.GetInt(), d.GetString());
}
```



**Q:** That seems kind of tedious. Why don't you use attributes, interfaces or some other way to automatically serialize objects, lists etc.?  
**A:** Antara.Data.Binary is meant to be a high performance dense serialization library. Doing what you propose would not allow you to do so. For more look at [Advanced Topics](ADVANCED_TOPICS.md), [Why ADB](WHY_ADB.md), [Examples Games](EXAMPLES_GAMES.md) and [Examples Finance](EXAMPLES_FINANCE.md)







