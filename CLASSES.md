# Serializing Classes

Serializing classes is as simple as adding the data they hold to a `BVector` and returning the byte[].

**Q:** Can you give an example?  
**A:** Assuming the following class

```c#
class Person {
    public int Id {get; private set;}
    public string Name {get; private set;}
    public bool IsAdult {get; private set;}
}
```

The simplest way to serialize is this:

```c#
public byte[] ToBytes() {
    BVector d = new BVector();
    d.Add(Id);
    d.Add(Name);
    d.Add1(IsAdult);
    return d.ToBytes();
}
```

And deserialize

```c#
public Person(byte[] b) {
    BVector d = new BVector(b);
    Id = d.GetInt();
    Name = d.GetString();
    IsAdult = d.Get1();
}
```



However this is not the best way to do it. In order to achieve nesting and serialization of collections here's the recommended way:

```c#
// Serialization
public byte[] ToBytes() {
    BVector d = new BVector();
    AddToBinary(d);
    return d.ToBytes();
}
internal void AddToBinary(BVector d) {
    d.Add(Id);
    d.Add(Name);
    d.Add1(IsAdult);
}
```

```c#
// Deserialization
public Person(byte[] b) {
    ReadFromBinary(new BVector(b));
}
internal void ReadFromBinary(BVector d) {
    Id = d.GetInt();
    Name = d.GetString();
    IsAdult = d.Get1();
}
// If you dont want to create a constructor
public static Person FromBytes(byte[] b) {
    Person p = new Person();
    p.ReadFromBinary(new BVector(b));
    return p;
}
```

**Q:** And why would I do that? First implementation seems adequate enough.  
**A:** This would allow you to serialize/deserialize nested classes and collections.  
**Q:** Elaborate please  
**A:** Assume another class

```c#
class Address {
    public Address() {}
    public int Id {get; private set;}
    public string Street {get; private set;}
    
    public void AddToBinary(BVector d) {
        d.Add(Id);
        d.Add(Street);
    }
    public void ReadFromBinary(BVector d) {
        Id = d.GetInt();
        Street = d.GetString();
    }
    // I will omit the rest of the methods to reduce clutter
}
```

And the `Person` class:

```c#
class Person {
    public int Id {get; private set;}
    public string Name {get; private set;}
    public bool IsAdult {get; private set;}
    public Address Address {get; private set;}
    
    public void AddToBinary(BVector d) {
    	d.Add(Id);
    	d.Add(Name);
    	d.Add1(IsAdult);
        Address.AddToBinary(d);
    }
    
    public void ReadFromBinary(BVector d) {
        Id = d.GetInt();
        Name = d.GetString();
        IsAdult = d.Get1();
        Address = new Address();
        Address.ReadFromBinary(d);
    }
}
```

Since the data in the `BVector` are stored in sequence, and every time you read from it, the pointer advances you can keep passing the reference to nested objects the same way as you added them to begin with.

Check out how to [Serialize Collections](COLLECTIONS.md) or [Version](VERSIONING.md) you classes to allow for future changes.