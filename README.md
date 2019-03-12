# Antara.Data.Binary (ADB)
General purpose high performance binary serialization library

## Overview

Antara.Data.Binary is a high performance general purpose binary serialization library. It produces a dense byte array in the easiest way and is flexible enough to cover most cases.

## Features

* High level of control
* Easy to use
* Really easy to use
* It is fast

## Why ADB?

While other libraries provide easier ways to serialize your data, by using attributes, automatic serialization of collections and nested types and others, ADB allows you really low level control over how the serialization happens in the simplest of manners. 

Benchmarking ADB versus most of the libraries with the datasets they provide, you will find ADB to be slower. That is because the data used for the benchmark are generic data that would most likely be a great fit for JSON, or dotNet xml.

ADB is great for game data, financial data and mostly data that do not contain a lot of text. These are the cases where ADB shines and outperforms in speed and size all other libraries.

For more: [Advanced Topics](ADVANCED_TOPICS.md), [Why ADB](WHY_ADB.md), [Examples Games](EXAMPLES_GAMES.md) and [Examples Finance](EXAMPLES_FINANCE.md)

## Competition

**Q:** Why do we need another serialization library? There's already dot net, protobuff, json, etc.  
**A:** Each of these libraries solves the same problem as Antara.Data.Binary but with different approaches, see below:

### dot net

dot net's serialization is created so that it covers everything! This alone makes it hard to use, slow and has a higher learning curve. In general it's not your bread and butter.

### protobuf

protobuff falls under the same category with dot net serialization. Although it is much faster and more dense that dot net. Still Antara.Data.Binary allows for more flexibility, fine grain control and easier to work with.

### json

Json is bloated, its speed is not that great. It's great value comes from the fact that is convenient and flexible, which is something Antara.Data.Binary aspires to be, by also providing high speed and dense format.

## Documentation

- [Basic Usage](BASIC_USAGE.md) 
- [Using in classes](CLASSES.md)  
- [Collections](COLLECTIONS.md)  
- [Versioning](VERSIONING.md)  
- [Advanced Topics](ADVANCED_TOPICS.md)





