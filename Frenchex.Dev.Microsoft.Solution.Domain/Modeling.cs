using System.Collections.ObjectModel;
using System.ComponentModel.Design;

namespace Frenchex.Dev.Microsoft.Solution.Domain
{
    public static class Modeling
    {
        public class Namespace:INameable<Namespace>
        {
            private Namespace()
            {

            }

            public static Namespace New(string name)
            {
                return new Namespace().Named(name);
            }

            public string Name { get; private set; }

            public Namespace Named(string name)
            {
                Name = name;
                return this;
            }

            private List<IType> _types { get; set; } = new();

            public Namespace Add(IType type)
            {
                _types.Add(type);
                return this;
            }
        }

        public interface IDataType : INamed
        {

        }

        public class @String : IDataType
        {
            public string Name => "string";
        }

        public interface IType : IDataType
        {
            Namespace DeclaredIn { get; }
        }

        public abstract class Enum
        {

            public required string Name { get; init; }

            protected Enum()
            {
            }

            public static Enum New(string name, Dictionary<string, int> dictionary)
            {
                return new Indexed()
                {
                    Name = name,
                    Values = dictionary
                };
            }

            public static Enum New(string name, List<string> dictionary)
            {
                return new NonIndexed()
                {
                    Name = name
                           ,
                    Values = dictionary
                };
            }

            public class Indexed : Enum
            {
                public required Dictionary<string, int> Values { get; set; }
            }

            public class NonIndexed : Enum
            {
                public required List<string> Values { get; set; }
            }
        }


        public interface INamed
        {
            string Name { get; }
        }

        public interface INameable<ReturnT>
        {
            string Name { get; }
            ReturnT Named(string name);
        }

        public enum Visibility
        {
            Public,
            Protected,
            Private
        }

        public class Class : INameable<Class>, IType
        {
            private Class()
            {

            }

            public static Class New(string name)
            {
                return new Class().Named(name);
            }

            public string Name { get; private set; }
            public Class Named(string name)
            {
                Name = name;
                return this;
            }

            private readonly List<Interface> _implements = new();
            public IReadOnlyCollection<Interface> Implements => new ReadOnlyCollection<Interface>(_implements);
            public Class Add(Interface implementation)
            {
                _implements.Add(implementation);
                return this;
            }



            private readonly List<Property> _properties = new();
            public IReadOnlyCollection<Property> Properties => new ReadOnlyCollection<Property>(_properties);
            public Class Add(Property property)
            {
                _properties.Add(property);
                return this;
            }



            private List<Method> _methods = new();
            public IReadOnlyCollection<Method> Methods => new ReadOnlyCollection<Method>(_methods);
            public Class Add(Method method)
            {
                _methods.Add(method);
                return this;
            }


            public Visibility Visibility { get; private set; }

            public Class WithVisibility(Visibility visibility)
            {
                Visibility = visibility;
                return this;
            }

            public Class? InheritedClass { get; private set; }

            public Class Inheriting(Class inheriting)
            {
                InheritedClass = inheriting;
                return this;
            }

            public bool IsStatic { get; set; }

            public Class AsStatic()
            {
                IsStatic = true;
                return this;
            }

            public Class AsNonStatic()
            {
                IsStatic = false;
                return this;
            }

            public bool IsAbstract { get; private set; }

            public Class AsAbstract()
            {
                IsAbstract = true;
                return this;
            }

            public Class AsNonAbstract()
            {
                IsAbstract = false;
                return this;
            }

            public bool IsSealed { get; private set; }

            public Class AsSealed()
            {
                IsSealed = true;
                return this;
            }

            public Class AsNonSealed()
            {
                IsSealed = true;
                return this;
            }

            public bool IsFinal { get; private set; }

            public Class AsFinal()
            {
                IsFinal = true;
                return this;
            }

            public Class AsNonFinal()
            {
                IsFinal = false;
                return this;
            }

            public Namespace DeclaredIn { get; private set; }
            public Class AsDeclaredIn(Namespace @namespace)
            {
                DeclaredIn = @namespace;
                @namespace.Add(this);
                return this;
            }
        }

        public class Interface : INameable<Interface>, IType
        {
            private Interface()
            {

            }

            public static Interface New(string name)
            {
                return new Interface().Named(name);
            }

            public string Name { get; private set; }
            public Interface Named(string name)
            {
                Name = name;
                return this;
            }


            private List<MethodDeclaration> _methods = new();
            public IReadOnlyCollection<MethodDeclaration> Methods => new ReadOnlyCollection<MethodDeclaration>(_methods);
            public Interface Add(MethodDeclaration method)
            {
                _methods.Add(method);
                method.AsDeclaredIn(this);
                return this;
            }

            public Namespace DeclaredIn { get; private set; }
            public Interface AsDeclaredIn(Namespace @namespace)
            {
                DeclaredIn = @namespace;
                @namespace.Add(this);
                return this;
            }
        }

        public class MethodDeclaration : INameable<MethodDeclaration>
        {
            private MethodDeclaration()
            {

            }

            public static MethodDeclaration New(string name)
            {
                return new MethodDeclaration().Named(name);
            }

            public string Name { get; private set; }
            public MethodDeclaration Named(string name)
            {
                Name = name;
                return this;
            }

            public Interface DeclaredIn { get; private set; }
            public MethodDeclaration AsDeclaredIn(Interface @interface)
            {
                DeclaredIn = @interface;
                return this;
            }

            public IType ReturnType { get; private set; }
            public MethodDeclaration ReturnsType(IType type)
            {
                ReturnType = type;
                return this;
            }

            public bool IsTask { get; private set; }

            public MethodDeclaration AsTask()
            {
                IsTask = true;
                return this;
            }

            public MethodDeclaration AsNonTask()
            {
                IsTask = false;
                return this;
            }
        }

        public class Property : INameable<Property>
        {
            private Property()
            {

            }

            public static Property New(string name)
            {
                return new Property().Named(name);
            }

            public string Name { get; set; }
            public Property Named(string name)
            {
                Name = name;
                return this;
            }

            public bool IsStatic { get; set; }

            public Property AsStatic()
            {
                IsStatic = true;
                return this;
            }

            public Property AsNonStatic()
            {
                IsStatic = false;
                return this;
            }
        }

        public class Method : INameable<Method>
        {
            private Method()
            {

            }

            public static Method New(string name)
            {
                return new Method().Named(name);
            }

            public string Name { get; set; }
            public Method Named(string name)
            {
                Name = name;
                return this;
            }

            public Class DeclaredIn { get; private set; }

            public Method AsDeclaredIn(Class declaredIn)
            {
                DeclaredIn = declaredIn;
                return this;
            }


            public bool IsStatic { get; set; }

            public Method AsStatic()
            {
                IsStatic = true;
                return this;
            }

            public Method AsNonStatic()
            {
                IsStatic = false;
                return this;
            }


            public IType ReturnType { get; private set; }
            public Method ReturnsType(IType type)
            {
                ReturnType = type;
                return this;
            }

            public bool IsTask { get; private set; }

            public Method AsTask()
            {
                IsTask = true;
                return this;
            }

            public Method AsNonTask()
            {
                IsTask = false;
                return this;
            }
            
            public bool IsAsync { get; private set; }

            public Method AsAsync()
            {
                IsAsync = true;
                return this;
            }
            public Method AsNonAsync()
            {
                IsAsync = false;
                return this;
            }

            public Func<Namespace, Class, Method, string> BodyGenerator { get; private set; }
            public Method WithBody(Func<Namespace, Class, Method, string> bodyGenerator)
            {
                BodyGenerator = bodyGenerator;
                return this;
            }

            public string GetBody()
            {
                return string.Empty;
            }

            public class Parameter : INameable<Parameter>
            {
                private Parameter()
                {

                }

                public static Parameter New(string name)
                {
                    return new Parameter().Named(name);
                }

                public string Name { get; private set; }
                public Parameter Named(string name)
                {
                    Name = name;
                    return this;
                }

                public Method ParamaterOf { get; private set; }

                public Parameter AsParameterOf(Method method)
                {
                    ParamaterOf = method;
                    return this;
                }

                private IDataType _type { get; set; }
                public Parameter OfType(IDataType type)
                {
                    _type = type;
                    return this;
                }

                public Interface OfTypeAsInterface()
                {
                    return (Interface)_type;
                }

                public Class OfTypeAsClass()
                {
                    return (Class)_type;
                }
            }

            private readonly List<Parameter> _parameters = new();
            public IReadOnlyCollection<Parameter> Parameters => new ReadOnlyCollection<Parameter>(_parameters);

            public Method WithParameter(Parameter ofType)
            {
                _parameters.Add(ofType);
                ofType.AsParameterOf(this);
                return this;
            }
        }
    }
}
