<img src="assets/logo.png" alt="HeadRest" width="200"/>

# HeadRest
A REST based headless approach for Umbraco.

HeadRest converts your Umbraco front end into a REST API by passing ModelsBuilder models through a mapping function to create serializable ViewModels and returning them as JSON payloads. 

Out of the box HeadRest is configured to use UmbracoMapper to perform it's mappings, however you can define your own custom mapper function to use other model mappers such as AutoMapper.

## Installation

### Nuget

    PM> Install-Package Our.Umbraco.HeadRest

## Configuration
In order to configure HeadRest you will first of all need to create an Umbraco composer:

````csharp
    public class HeadRestConfigComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            // Configuration goes here
        }
    }

    public void Terminate() { }
````

From within the `Compose` method, you can then configure your endpoint(s) via the `ConfigureEndpoint` method on a new instance of the HeadRest service:
````csharp 
    ...
    new HeadRest().ConfigureEndpoint(...);
    ...
````

### Basic Configuration
For the most basic implementation, the following minimal configuration is all that is needed:
````csharp 
    new HeadRest().ConfigureEndpoint(new HeadRestOptions {
        ViewModelMappings = new HeadRestViewModelMap()
            .For(HomePage.ModelTypeAlias).MapTo<HomePageViewModel>()
            ...
    });
````

This will create an API endpoint at the path `/`, and will be anchored to the first content at the root of the site. It will use the list of `ViewModelMappings` provided to lookup the viewmodel to map a given node to.

### Advanced Configuration
For a more advanced implementation, the following configuration shows all the supported options.
````csharp 
    new HeadRest().ConfigureEndpoint("/api/", "/root//nodeTypeAlias[1]", new HeadRestOptions {
        Mode = HeadRestEndpointMode.Dedicated,
        ControllerType = typeof(HeadRestController),
        Mapper = ctx => AutoMapper.Map(ctx.Content, ctx.ContentType, ctx.ViewModelType),
        ViewModelMappings = new HeadRestViewModelMap()
            .For(HomePage.ModelTypeAlias)
                .If(x => x.Request.HeadRestRouteParam("altRoute") == "init")
                .MapTo<InitViewModel>()
            .For(HomePage.ModelTypeAlias).MapTo<HomePageViewModel>()
            .Default().MapTo<BasicViewModel>(),
        CustomRouteMappings = new HeadRestRouteMap()
            .For("/(?<altRoute>init)/?$").MapTo("/")
    });
````
This will create an endpoint at the url `/api/`, and will be anchored to the node at the XPath `/root//nodeTypeAlias[1]`. In addition, the supplied controller will be used to handle the HeadRest requests and the supplied mapper function will be used to perform the mapping. It will use the list of `ViewModelMappings` provided to lookup the viewmodel to map a given node to. Lastly, a custom route is defined to map the url`/api/init/` to the `/api/` URL, storing the string `"init"`, captured via the route pattern, which is then used in the `ViewModelMappings` to map `/api/init/` request to the `InitViewModel` instead of the default `HomePageViewModel`.

### Configuration Options
* __basePath : string__   
  _[optional, default:"/"]_  
  The base path from which your API will be accessible from.
* __rootNodeXPath : string__   
  _[optional, default:"/root/*[@isDoc][1]"]_  
  The XPath statement for the root node from which to anchor your API endpoint to.
* __Mode : HeadRestEndpointMode__   
  _[optional, default:HeadRestEndpointMode.Dedicated]_  
  The mode in which the headrest endpoint will run. Can be either `HeadRestEndpointMode.Dedicated`, in which case the generated endpoint URLs will be the primary URLs for the relevant content nodes, or `HeadRestEndpointMode.Mixed`, in which case node will maintain their primary URLs and the endpoint URLs will be added to the nodes "Other URLs" collection.
* __ControllerType : Type__   
  _[optional, default:typeof(HeadRestController)]_  
  The Controller to use to service the API requests. Controllers must inherit from `HeadRestController`. Useful to add extra FilterAttributes to the request such as AuthU and the `OAuth` attribute.
* __Mapper : Func<HeadRestMapperContext, object>__   
  _[optional, default:ctx => UmbracoMapper.Map<ViewModelType>(ctx.Content)]_  
  A function to perform the map between the nodes ModelsBuilder model and it's associated ViewModel. Defaults to using the build in UmbracoMapper.
* __ViewModelMappings : HeadRestViewModelMap__   
  _[required, default:null]_  
  A fluent list of mappings to determine which ViewModel a given content type should be mapped to. Multiple mappings can be defined for the same content type by defining a condition for the mapping via the flient `.If(...)` interface. Any conditional mappings should be defined before any non-conditional (fallback) mappings. You can set a default map that will be used if no other matching maps are found by using the `.ForEverythingElse().MapTo<Type>()` syntax. If a default map is defined, it must be the last map defined.
* __CustomRouteMappings : HeadRestRouteMap__   
  _[optional, default:null]_  
  A fluent list of custom route mappings to map any custom routes to a standard built in route. Routes are defined as regex patterns with any captured paramters being made accessible via a `HeadRestRouteParam` extension on the standard `Request` object. Usefull to support multiple routes pointing to the same endpoint URL, or for things like pagination URLs.

**NB** Whilst the `ViewModelMappings` tells HeadRest which ViewModel to map a content model to, it does *not* tell it how to actually map the properties over. For this you will need to instruct the model mapper using it's predefined mapping approach, for example, with UmbracoMapper you will want to define your mappings via a `MapDefinition` class registered via a composer like so:
````csharp 
    public class MyHeadRestMapDefinition : IMapDefinition
    {
        public void DefineMaps(IUmbracoMapper mapper)
        {
            mapper.Define<FromType, ToType>(
                (frm, ctx) => ...,      // Constructor function
                (frm, to, ctx) => ...   // Map function
        }
    }
    
    public class MyHeadRestMapDefinisionComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            composition.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
                .Add<MyHeadRestMapDefinition>();
        }
    }
````
## How is HeadRest different to Umbraco Headless?
| | HeadRest | Umbraco Headless |
|-|----------|---------------|
| Configuration | HeadRest requires configuring via a code based API so requires C# knowledge | Zero config required |
| Maintenance | You are responsible for applying security patches / updates | Automatic updates |
| Scalability | You are responsible for making the API scalable for your needs | Automatic scalability |
| API | Fixed API based on compiled ViewModels | Flexible query language |
| Security | You are responsible for securing your API (Suggest [AuthU](https://github.com/mattbrailsford/umbraco-authu)) | Security baked in |
| Back office UI | Potentially unused features visible | Unused features disabled |
| Supports use of 3rd party packages | Yes | No |
| Supports mixed install | Yes. You can have a headless API + website in one install | No. Headless only install |
| Supports custom backend code | Yes | No |
| Client libraries available | None. Roll your own | .NET Framework, .NET Core, Node.js |
| Hosting | Installs to any Umbraco version (9.0+) | Umbraco Cloud service only |
| Support | Limited community support | HQ Supported |

## Contributing To This Project

Anyone and everyone is welcome to contribute. Please take a moment to review the [guidelines for contributing](CONTRIBUTING.md).

* [Bug reports](CONTRIBUTING.md#bugs)
* [Feature requests](CONTRIBUTING.md#features)
* [Pull requests](CONTRIBUTING.md#pull-requests)

## License

Copyright &copy; 2019 Matt Brailsford, Outfield Digital Ltd 

Licensed under the [MIT License](LICENSE)

