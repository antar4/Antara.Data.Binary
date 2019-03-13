# ![alt text](http://cdn.3volve.io/adb/images/logo/0.5x/logo@0.5x.png "ADB Logo")   Versioning

Versioning is important for data that you intend to store. As all developers know, the requirements of any software change over time and so does the data that serves it, which makes it imperative to be able to distinguish different versions stored.



## Strategy

The strategy is read all - write last. To be able to read all versions but always write in the last version. This is achieved by adding the version at the beginning of the each object to store.

Assume the following class:

```C#
    public class Person
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
```

In order to add a version we would do the following change:

```C#
internal void AddToBinary(BVector d)
{
    d.Add(BINARY_VERSION, BINARY_VERSION_LEN); // add the binary version first
    d.Add(Id);
    d.Add(Name);
    d.Add1(IsAdult);
}
internal void ReadFromBinary(BVector d)
{
    d.GetInt(BINARY_VERSION_LEN); // we dont really care about the version now
    Id = d.GetInt();
    Name = d.GetString();
    IsAdult = d.Get1();
}
const int BINARY_VERSION = 1;
const int BINARY_VERSION_LEN = 4;
```

**Q:** What is `d.Add(BINARY_VERSION, **BINARY_VERSION_LEN**)`  
**A:** It means we are adding the version and using 4 bits to do so.   
**Q:** Why 4 bits?  
**A:** 4 Bits will allow us for 16 different versions of the data which is usually more than enough for the lifetime of a product, and we are not consuming extra bytes for it.

**Q:** Ok, and how do we 'add' a new version?  

```C#
public class Person
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public bool IsAdult { get; private set; }
    public int Age {get; private set;} // new property

    internal void AddToBinary(BVector d)
    {
        d.Add(BINARY_VERSION, BINARY_VERSION_LEN);
        d.Add(Id);
        d.Add(Name);
        d.Add1(IsAdult);
        d.Add(Age); // new property - always write last version
    }
    internal void ReadFromBinary(BVector d)
    {
        int version = d.GetInt(BINARY_VERSION_LEN);
        Id = d.GetInt();
        Name = d.GetString();
        IsAdult = d.Get1();
        if (version >= 2) {
            Age = d.GetInt();
        } else {
            Age = -1; // default value
        }
    }
    const int BINARY_VERSION = 2; // changed version to 2
    const int BINARY_VERSION_LEN = 4;
}
```

**Q:** That's nice, but what happens if we in the future discover we need more than 16 versions? or whatever number we originally chose?
**A:** That's easy to fix, as long as you decide it one version before the max. When you reach version 15 you will need to add another 4 bits for the second version:

```C#
internal void AddToBinary(BVector d)
{
    d.Add(BINARY_VERSION, BINARY_VERSION_LEN);
    d.Add(BINARY_VERSION_2, BINARY_VERSION_LEN);
	...
    ...
    ...
}
internal void ReadFromBinary(BVector d)
{
    int version = d.GetInt(BINARY_VERSION_LEN);
    if (version == 15) {
        version += d.GetInt(BINARY_VERSION_LEN);
    }
    ...
    ...
    ...
}
const int BINARY_VERSION = 15; 
const int BINARY_VERSION_2 = 0; // new version
const int BINARY_VERSION_LEN = 4;
```

It is not too elegant, but it's the only option.