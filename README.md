# RangeQueryDotNet

- is a middleware for ASP.NET Core
- is a cross cutting way to enable client queries and tranforms over all endpoints in your app

## What? How?

The [`range` http header](https://tools.ietf.org/html/rfc7233) is traditionally used for getting byte of a file, but also says:

>   ...devices ...
>   might benefit from being able to request only a subset of a larger
>   representation, such as a single page of a very large document, or
>   the dimensions of an embedded image.

and

> Although the range request mechanism is designed to allow for
> extensible range types, this specification only defines requests for
> byte ranges.

So thats cool - you can use the range header to define your own way to request a subset of a response representation.

Here's an article ["Range header, I choose you!"](http://otac0n.com/blog/2012/11/21/range-header-i-choose-you.html ) about using it for pagination, which provides a good background on using custom range headers.

So, for example, [JMESPath](https://jmespath.org/), is a popular query language for json with [a great .NET implementation, JmesPath.Net](https://github.com/jdevillard/JmesPath.Net).

By putting JMESPath queries into the range header, and applying them to any json responses, we can enable our frontend developers to get back json data in the format they want. Who needs GraphQL anyway? ;)

But you could use OData queries, or GraphQL-like queries, or JsonPath or whatever you like. NB The examples will use JmesPath to demonstrate. 

## Middleware

Once registered, it will intercept outgoing applicaton/json responses, and check for a header like `range: somequerykind=somequery`. 

If that header is present, then the middleware will apply the query to the outgoing response, allowing a client to shape the response any way it wishes.

## Install

```
$ nuget install-package RangeQueryDotNet.AspNetCore
```

## Configure

In Startup.cs:

```
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ...
            var rangeType = "jmesrange";
            Func<(string query, string json), string> transform = x => new JmesPath().Transform(x.json, x.query);
            app.UseRangeQueryMiddleware(rangeType, transform);
            ...
        }
```


## Usage Example 

Taken from the RangeQueryDotNet.Example project, running on localhost:5000:

Curl the raw /locations endpoint:

```
$ curl -i -k https://localhost:5001/locations ; echo
HTTP/2 200
date: Wed, 09 Dec 2020 13:31:30 GMT
content-type: application/json
server: Kestrel
content-length: 145

{"locations":[{"name":"Seattle","state":"WA"},{"name":"New York","state":"NY"},{"name":"Bellevue","state":"WA"},{"name":"Olympia","state":"WA"}]}


```
You get a pretty standard json reponse. Now curl with a `range: jmesrange locations[?state == 'WA'].name` header:

```
$ curl -i -k -H "range: jmesrange=locations[?state == 'WA'].name" https://localhost:5001/locations ; echo
HTTP/2 206
date: Wed, 09 Dec 2020 13:34:32 GMT
content-type: application/json
content-range: locations[?state == 'WA'].name
server: Kestrel
content-length: 32

["Seattle","Bellevue","Olympia"]

```

The output has been munged by JMESRange! Pretty neat!
